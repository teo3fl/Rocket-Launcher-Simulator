using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RocketLauncher : MonoBehaviour
{
    [SerializeField]
    private GameObject rocketPrefab;
    [SerializeField]
    private Transform t_launcherModel;
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private GameObject go_target;

    [SerializeField]
    private float f_fireCooldown = 1f;
    [SerializeField]
    private float f_aimTime = 2f;
    [SerializeField]
    private float f_targetRotationSpeed = 1f;
    [SerializeField]
    private float f_maxTargetAngleDifference = 30f;
    [SerializeField]
    private Color emptyTargetColor;
    [SerializeField]
    private Color targetAquiredColor;
    [SerializeField]
    private Color targetLockedColor;

    private bool canFire = true;
    private bool inEnabled = true;

    enum TargetState { Lost, Aquired, Locked };

    private TargetState currentTargetState = TargetState.Lost;
    private TargetState CurrentTargetState
    {
        get { return currentTargetState; }
        set
        {
            currentTargetState = value;
            RefreshUiTargetColor();
        }
    }
    private bool IsPlaneInSight
    {
        get
        {
            var airplane = AirplaneManager.Instance.airplane;
            if (airplane == null)
                return false;

            if (CurrentTargetState == TargetState.Locked)
                return true;

            // let dir = the straight line from the launcher to the target
            // if the angle between dir and the forward of the launcher is greater than a given value
            // then the target was lost
            Vector3 forward = t_launcherModel.transform.TransformDirection(Vector3.right) * 1000;
            Debug.DrawRay(t_launcherModel.transform.position + modelPossitionOffset, forward, Color.green);

            Vector3 dir = airplane.transform.position - t_launcherModel.transform.position;
            Debug.DrawRay(t_launcherModel.transform.position, dir, Color.red);
            return Physics.Raycast(t_launcherModel.transform.position, forward, Mathf.Infinity, planeLayer);
        }
    }
    [SerializeField]
    private Vector3 modelPossitionOffset = new Vector3(0, 0, 0);
    private int planeLayer = 1 << 8;
    private int planeForwardSpaceLayer = 1 << 7;

    private float f_elapsedAimTime = 0f;

    public delegate void RocketFired();
    public static RocketFired onRocketFired;

    private void Start()
    {
        RefreshUiTargetColor();
    }

    void Update()
    {
        if (IsPlaneInSight)
        {
            CurrentTargetState = TargetState.Aquired;
            f_elapsedAimTime += Time.deltaTime;
            if (f_elapsedAimTime >= f_aimTime)
            {
                CurrentTargetState = TargetState.Locked;
            }
        }
        else
        {
            f_elapsedAimTime = 0;
            CurrentTargetState = TargetState.Lost;
        }

        UpdateTargetPosition();
        UpdateInput();
    }

    private void UpdateTargetPosition()
    {
        var airplane = AirplaneManager.Instance.airplane;
        if (airplane != null && IsPlaneInSight)
        {
            Vector3 wantedPos = Camera.main.WorldToScreenPoint(airplane.transform.position);
            go_target.transform.position = new Vector3(wantedPos.x, wantedPos.y, 0);
            go_target.transform.Rotate(0, 0, f_targetRotationSpeed * Time.deltaTime);
        }
    }

    private void UpdateInput()
    {
        if (!enabled)
            return;

        if (Input.GetKeyDown(KeyCode.Mouse0) && canFire)
        {
            Fire();
            StartCoroutine(FireCooldown());
        }
    }

    private IEnumerator FireCooldown()
    {
        yield return new WaitForSeconds(f_fireCooldown);
        canFire = true;
        t_launcherModel.GetComponent<AudioSource>().Play(); // reload sound
    }

    private void Fire()
    {
        canFire = false;
        Vector3 forward = t_launcherModel.transform.TransformDirection(Vector3.right) * 1000;

        var rocket = Instantiate(rocketPrefab, spawnPoint.position, Quaternion.LookRotation(forward, Vector3.up)).GetComponent<Rocket>();
        //var rocket = Instantiate(rocketPrefab, spawnPoint.position, transform.rotation).GetComponent<Rocket>();
        var airplane = AirplaneManager.Instance.airplane;
        rocket.target = airplane && CurrentTargetState == TargetState.Locked && Physics.Raycast(t_launcherModel.transform.position, forward, Mathf.Infinity, planeForwardSpaceLayer) ? airplane.transform : null;

        onRocketFired?.Invoke();
    }

    private void RefreshUiTargetColor()
    {
        var airplane = AirplaneManager.Instance.airplane;
        if (airplane == null || !airplane.Renderer.isVisible)
        {
            go_target.GetComponent<Image>().color = emptyTargetColor;
            return;
        }

        switch (CurrentTargetState)
        {
            case TargetState.Lost:
                go_target.GetComponent<Image>().color = emptyTargetColor;
                break;
            case TargetState.Aquired:
                go_target.GetComponent<Image>().color = targetAquiredColor;
                break;
            case TargetState.Locked:
                go_target.GetComponent<Image>().color = targetLockedColor;
                break;
        }
    }

    public void Enable(bool isEnabled)
    {
        enabled = isEnabled;
    }
}
