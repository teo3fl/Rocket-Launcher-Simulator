using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airplane : MonoBehaviour
{
    [SerializeField]
    private float f_movementSpeed = 20f;


    void Update()
    {
        transform.Translate(Vector3.forward * f_movementSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Plane hit");
        // start descending

        // generate smoke particles
        // rotate until it forms a certain angle with the ground
    }
}
