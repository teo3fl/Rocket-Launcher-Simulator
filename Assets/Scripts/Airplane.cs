using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airplane : MonoBehaviour
{
    [SerializeField]
    private float f_movementSpeed = 20f;
    [SerializeField]
    private float f_fallAngle = 45f;
    [SerializeField]
    private float f_descendRate = 0.5f;

    [SerializeField]
    private GameObject go_workingTrailRenderers;
    [SerializeField]
    private GameObject go_damagedTrailRenderers;
    [SerializeField]
    private GameObject go_damageExplosions;
    [SerializeField]
    private GameObject go_destroyExplosion;

    private bool isHit = false;
    Vector3 ground = new Vector3(1, 0, 1);

    void Update()
    {
        if (isHit)
        {
            Vector3 targetDirection = ground - transform.position;
            var angle = Vector3.Angle(targetDirection, transform.forward) % 91f;
            if (angle < f_fallAngle)
            {
                float singleStep = f_descendRate * Time.deltaTime;
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDirection);
                //Debug.Log("Rotated plane");
            }
        }

        transform.Translate(Vector3.forward * f_movementSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Rocket>() != null)
        {
            if (isHit)
                return;

            isHit = true;
            Debug.Log("Plane was hit by rocket");
            go_workingTrailRenderers.transform.DetachChildren();
            go_damagedTrailRenderers.SetActive(true);
            go_damageExplosions.SetActive(true);
        }
        else if (other.gameObject.GetComponent<Terrain>() != null)
        {
            Debug.Log("Plane hit the ground");
            go_workingTrailRenderers.transform.DetachChildren();
            go_destroyExplosion.transform.SetParent(null);
            go_destroyExplosion.SetActive(true);
            Destroy(go_destroyExplosion, 5f);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Plane hit bounds");
            go_workingTrailRenderers.transform.DetachChildren();
            if (go_damagedTrailRenderers.activeInHierarchy)
                go_damagedTrailRenderers.transform.DetachChildren();
            Destroy(gameObject);
        }
    }
}
