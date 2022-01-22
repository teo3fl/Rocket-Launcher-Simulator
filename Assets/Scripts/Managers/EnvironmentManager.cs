using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField]
    private Material daySkybox;
    [SerializeField]
    private Material nightSkybox;
    [SerializeField]
    private Light sceneDirectionalLight;
    [SerializeField]
    private float f_daylightIntensity = 2;
    [SerializeField]
    private float f_nightlightIntensity = 0.5f;

    private bool isDay = true;


    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();

        if(Input.GetKeyDown(KeyCode.E))
        {
            isDay = !isDay;
            UpdateEnvironment();
        }
    }

    private void UpdateEnvironment()
    {
        if(isDay)
        {
            RenderSettings.skybox = daySkybox;
            sceneDirectionalLight.intensity = f_daylightIntensity;
        }
        else
        {
            RenderSettings.skybox = nightSkybox;
            sceneDirectionalLight.intensity = f_nightlightIntensity;
        }
    }
}
