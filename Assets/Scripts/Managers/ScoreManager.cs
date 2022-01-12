using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI tmp_fired;
    [SerializeField]
    private TMPro.TextMeshProUGUI tmp_hit;

    private uint fired = 0;
    private uint hit = 0;

    void Start()
    {
        Airplane.onPlaneHit += OnPlaneHit;
        RocketLauncher.onRocketFired += OnRocketFired;
    }

    private void OnRocketFired()
    {
        fired++;
        tmp_fired.text = "Fired: " + fired;
    }

    private void OnPlaneHit()
    {
        hit++;
        tmp_hit.text = "Hit: " + hit;
    }
}
