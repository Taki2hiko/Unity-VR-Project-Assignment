using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NormalSpawner : MonoBehaviour
{
    [SerializeField] private float countdown;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private TextMeshProUGUI TimerText;
    [SerializeField] private TextMeshProUGUI WavesText;
    [SerializeField] private TextMeshProUGUI EnemyCountText;
    [SerializeField] private TextMeshProUGUI lobbyText;
    [SerializeField] Animator animator;

    public GameObject lobbyPoint;

    private PlayerController playerController;

    public Waves[] Wave;
    public int currentWaveIndex = 0;

    private bool IsFinish = false;
    private bool readyToCountDown;
    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        readyToCountDown = true;

        for (int i = 0; i < Wave.Length; i++)
        {
            Wave[i].enemiesLeft = Wave[i].enemies.Length;
        }
    }
    private void Update()
    {
        TimerText.SetText("Time:" + countdown);
        WavesText.SetText("Wave " + (currentWaveIndex + 1));

        if (currentWaveIndex >= Wave.Length && !IsFinish)
        {
            Debug.Log("You survived every wave!");
            playerController.gameObject.transform.position = lobbyPoint.transform.position;
            lobbyText.SetText("You Win!\n Do you want to reset?");
            playerController.SetController();
            playerController.SetButton();
            IsFinish = true;
        }
        else if (currentWaveIndex <= Wave.Length)
        {
            EnemyCountText.SetText("Enemy: " + Wave[currentWaveIndex].enemiesLeft);
        }

        if (readyToCountDown == true)
        {
            countdown -= Time.deltaTime;
        }

        if (countdown <= 0 && playerController.IsStart)
        {
            animator.SetBool("IsClose", false);

            readyToCountDown = false;

            countdown = Wave[currentWaveIndex].timeToNextWave;

            StartCoroutine(SpawnWave());
        }

        if (Wave[currentWaveIndex].enemiesLeft == 0)
        {

            readyToCountDown = true;

            currentWaveIndex++;
        }
    }
    private IEnumerator SpawnWave()
    {
        if (currentWaveIndex < Wave.Length)
        {
            for (int i = 0; i < Wave[currentWaveIndex].enemies.Length; i++)
            {
                GameObject Enemy = Instantiate(Wave[currentWaveIndex].enemies[i], spawnPoint.transform);

                Enemy.transform.SetParent(spawnPoint.transform);

                yield return new WaitForSeconds(Wave[currentWaveIndex].timeToNextEnemy);
            }

            StartCoroutine(CloseGate());
        }
    }

    private IEnumerator CloseGate() 
    {
        yield return new WaitForSeconds(3);
        animator.SetBool("IsClose", true);

    }
}

[System.Serializable]
public class Waves
{
    public GameObject[] enemies;
    public float timeToNextEnemy;
    public float timeToNextWave;

    [SerializeField] public int enemiesLeft;
}

