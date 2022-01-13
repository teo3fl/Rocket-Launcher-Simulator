using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneManager : MonoBehaviour
{
    public static AirplaneManager Instance { get; private set; }

    [SerializeField]
    private GameObject[] airplanePrefabs;
    [SerializeField]
    private Transform spawnLocations;

    public Airplane airplane { get; private set; }


    void Start()
    {
        Instance = this;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && airplane == null)
        {
            var spawn = GetRandomSpawn();
            airplane = Instantiate(GetRandomAirplanePrefab(), spawn.position,spawn.rotation).GetComponent<Airplane>();
        }
    }

    private Transform GetRandomSpawn()
    {
        int index = Random.Range(0, spawnLocations.childCount - 1);
        return spawnLocations.GetChild(index);
    }

    private GameObject GetRandomAirplanePrefab()
    {
        int index = Random.Range(0, airplanePrefabs.Length - 1);
        return airplanePrefabs[index];
    }
}
