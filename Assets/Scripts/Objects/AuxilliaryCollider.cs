using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuxilliaryCollider : MonoBehaviour
{
    [SerializeField]
    private Rocket rocket;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("rocket hit terrain");
        rocket.SelfDestruct();
    }
}
