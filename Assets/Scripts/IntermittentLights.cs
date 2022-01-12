using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntermittentLights : MonoBehaviour
{
    [SerializeField]
    private GameObject light1;
    [SerializeField]
    private GameObject light2;

    [SerializeField]
    private float f_duration = 3f;
    
    void Start()
    {
        StartCoroutine(UpdateLights());
    }

    private IEnumerator UpdateLights()
    {
        while (true)
        {
            light1.SetActive(false);
            light2.SetActive(true);

            Swap();
            yield return new WaitForSeconds(f_duration);
        }
    }

    private void Swap()
    {
        var temp = light1;
        light1 = light2;
        light2 = temp;
    }
}
