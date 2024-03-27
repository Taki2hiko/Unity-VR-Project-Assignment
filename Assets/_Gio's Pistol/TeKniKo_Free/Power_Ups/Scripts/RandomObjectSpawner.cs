using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObjectSpawner : MonoBehaviour
{
    public GameObject[] powerUp;
    public bool stopSpawning = false;
    public float spawnTime;
    public float spawnDelay;
    private float timeLimit=0;

    public Transform spawnPoint;

    void Start()
    {
        InvokeRepeating("SpawnObject", spawnTime, spawnDelay);
    }

    public void SpawnObject()
    {
        int rand = Random.Range(0, powerUp.Length);

        Instantiate(powerUp[rand], spawnPoint.position, transform.rotation);
        if(timeLimit>=120f)
        {
            CancelInvoke("SpawnObject");
        }
        timeLimit += spawnDelay;
        
    }
}
