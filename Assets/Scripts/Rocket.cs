using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField]
    private float f_viewAngle = 20f;
    [SerializeField]
    private float f_movementSpeed = 100f;

    public GameObject target;

    bool lostTarget = false;
    private bool LostTarget
    {
        get
        {
            if (lostTarget)
                return lostTarget;

            // let dir = the straight line from the rocket to the target
            // if the angle between dir and the forward of the rocket is greater than a given value
            // then the target was lost
            Vector3 dir = transform.position - target.transform.position;
            if (Vector3.Angle(dir, transform.forward) > f_viewAngle)
            {
                lostTarget = true;
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
        if(LostTarget)
        {
            // move forward
            transform.Translate(Vector3.forward * f_movementSpeed * Time.deltaTime);
        }
        else
        {
            // move towards target
            float step = f_movementSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
        }
    }
}
