using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneManager : MonoBehaviour
{
    public static AirplaneManager Instance { get; private set; }

    [SerializeField]
    private GameObject airplanePrefab;
    [SerializeField]
    private Transform spawnLocation;

    private Airplane airplane;

    void Start()
    {
        Instance = this;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && airplane == null)
        {
            airplane = Instantiate(airplanePrefab, spawnLocation.position,spawnLocation.rotation).GetComponent<Airplane>();
        }
    }
}
