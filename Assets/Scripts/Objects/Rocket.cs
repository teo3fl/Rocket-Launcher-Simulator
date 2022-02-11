using System.Collections;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField]
    private float f_viewAngle = 20f;
    [SerializeField]
    private float f_topSpeed = 2300f;
    [SerializeField]
    private float f_acceleration = 300f;
    [SerializeField]
    private float f_startingSpeed = 50f;
    private float f_movementSpeed = 0f;
    [SerializeField]
    private Transform trail;
    [SerializeField]
    private GameObject explosion;
    [SerializeField]
    private GameObject go_impactSoundPlayer;

    public Transform target;

    bool lostTarget = false;
    private bool LostTarget
    {
        get
        {
            if (lostTarget)
                return lostTarget;

            if (target == null)
            {
                lostTarget = true;
            }
            else
            {
                // let dir = the straight line from the rocket to the target
                // if the angle between dir and the forward of the rocket is greater than a given value
                // then the target was lost
                
                Vector3 dir = target.transform.position - transform.position;
                var angle = Vector3.Angle(dir, transform.forward) % 91f;
                if (angle > f_viewAngle)
                {
                    lostTarget = true;
                    Debug.Log("Target lost");
                }
            }

            return lostTarget;
        }
    }

    public delegate void FollowingTarget();
    public static FollowingTarget onFollowingTarget;


    void Start()
    {
        if (target == null)
            lostTarget = true;
        else
            onFollowingTarget?.Invoke();

        f_movementSpeed = f_startingSpeed;

        StartCoroutine(FollowTarget());
    }

    void Update()
    {
        if(f_movementSpeed < f_topSpeed)
        {
            f_movementSpeed += f_acceleration * Time.deltaTime;
            f_movementSpeed %= f_topSpeed;
        }
        transform.Translate(Vector3.forward * f_movementSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        var plane = other.GetComponent<Airplane>();
        if (plane != null)
        {
            if (target == null) // the rocket hit the jet collider anyway
                return;
            else
                Debug.Log("Rocket hit a plane");
        }

        if (other.GetComponent<Terrain>())
            Debug.Log("Rocket hit terrain");
        else
            Debug.Log("Rocket hit bounds");

        SelfDestruct();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Rocket hit an object (collider)");
        SelfDestruct();
    }

    public void SelfDestruct()
    {
        go_impactSoundPlayer.SetActive(true);
        go_impactSoundPlayer.transform.SetParent(null);
        Destroy(go_impactSoundPlayer, 10f);
        trail.SetParent(null);
        explosion.SetActive(true);
        explosion.transform.SetParent(null);
        Destroy(explosion, 4f);
        Destroy(gameObject);
    }

    private IEnumerator FollowTarget()
    {
        yield return new WaitForSeconds(0.3f);
        while (!LostTarget)
        {
            Vector3 targetDirection = target.position - transform.position;
            float singleStep = f_movementSpeed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);

            yield return 0;
        }
    }
}
