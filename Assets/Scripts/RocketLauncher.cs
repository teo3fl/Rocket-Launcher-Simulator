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
            // let dir = the straight line from the launcher to the target
            // if the angle between dir and the forward of the launcher is greater than a given value
            // then the target was lost
            Vector3 forward = t_launcherModel.transform.TransformDirection(Vector3.right) * 1000;
            Debug.DrawRay(t_launcherModel.transform.position + modelPossitionOffset, forward, Color.green);

            var airplane = AirplaneManager.Instance.airplane;
            if (airplane == null)
                return false;

            Vector3 dir = airplane.transform.position - t_launcherModel.transform.position;
            Debug.DrawRay(t_launcherModel.transform.position, dir, Color.red);
            return Physics.Raycast(t_launcherModel.transform.position, forward, Mathf.Infinity, planeLayer);
        }
    }
    [SerializeField]
    private Vector3 modelPossitionOffset = new Vector3(0, 0, 0);
    private int planeLayer = 1 << 8;

    private float f_elapsedAimTime = 0f;


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
            go_target.transform.position = wantedPos;
            go_target.transform.Rotate(0, 0, f_targetRotationSpeed * Time.deltaTime);
        }
    }

    private void UpdateInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Fire();
            StartCoroutine(FireCooldown());
        }
    }
    private IEnumerator FireCooldown()
    {
        yield return new WaitForSeconds(f_fireCooldown);
        canFire = true;
    }

    private void Fire()
    {
        canFire = false;

        var rocket = Instantiate(rocketPrefab, spawnPoint.position, transform.rotation).GetComponent<Rocket>();
        var airplane = AirplaneManager.Instance.airplane;
        rocket.target = airplane && CurrentTargetState == TargetState.Locked? airplane.transform : null;
    }

    private void RefreshUiTargetColor()
    {
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
}
