using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuxilliaryCollider : MonoBehaviour
{
    [SerializeField]
    private Rocket rocket;

    private BoxCollider collider;

    private void Start()
    {
        collider = GetComponent<BoxCollider>();
        StartCoroutine(ManageCollider());
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("rocket hit terrain");
        rocket.SelfDestruct();
    }


    private IEnumerator ManageCollider()
    {
        collider.enabled = false;
        yield return new WaitForSeconds(0.5f);
        collider.enabled = true;
    }
}
