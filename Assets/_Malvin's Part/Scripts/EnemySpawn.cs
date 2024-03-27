using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{

    [SerializeField] private float speed;

    private float countdown = 5f;

    private NormalSpawner waveSpawner;

    private void Start()
    {
        waveSpawner = GetComponentInParent<NormalSpawner>();
    }
    private void Update()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime);

        countdown -= Time.deltaTime;

        if (countdown <= 0)
        {
            Destroy(gameObject);

            waveSpawner.Wave[waveSpawner.currentWaveIndex].enemiesLeft--;
        }
    }
}
