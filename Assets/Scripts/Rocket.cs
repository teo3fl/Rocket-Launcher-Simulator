using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField]
    private float f_viewAngle = 20f;
    [SerializeField]
    private float f_movementSpeed = 100f;
    [SerializeField]
    private Transform trail;
    [SerializeField]
    private GameObject explosion;

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


    void Start()
    {
        if (target == null)
            lostTarget = true;
    }

    void Update()
    {
        if (!LostTarget)
        {
            Vector3 targetDirection = target.position - transform.position;
            float singleStep = f_movementSpeed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }

        transform.Translate(Vector3.forward * f_movementSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Rocket hit a plane or a bound");
        SelfDestruct();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Rocket hit an object (collider)");
        SelfDestruct();
    }

    private void SelfDestruct()
    {
        trail.SetParent(null);
        explosion.SetActive(true);
        explosion.transform.SetParent(null);
        Destroy(explosion, 4f);
        Destroy(gameObject);
    }
}
