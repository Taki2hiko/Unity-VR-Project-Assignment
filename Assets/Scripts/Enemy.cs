using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, ITakeDamage
{
    enum State
    {
        Idle,
        Chase,
        Attack,
        Dead,
    }

    [Header("Enemy Weapon")]
    [SerializeField] private GameObject weapon;

    private Animator animator;
    private NavMeshAgent agent;

    private HealthSystem healthSystem;
    private NormalSpawner waveSpawner;
    private PlayerController player;
    private GameObject target;
    private State state;

    private bool Attacking = false;
    [SerializeField] private bool IsDead = false;
    //private int RandomNum = 0;
    public float EnemyDamage = 10;
    [SerializeField] private float attackRange = 5f;

    // Start is called before the first frame update
    void Start()
    {
        waveSpawner = GetComponentInParent<NormalSpawner>();
        healthSystem = GetComponent<HealthSystem>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player");
        player = FindObjectOfType<PlayerController>();

        healthSystem.SetMaxHealth(100);
        healthSystem.SetHealth(100);
    }

    // Update is called once per frame
    void Update()
    {
        // If health is 0, then die
        if (healthSystem.GetHealth() <= 0 && !IsDead)
        {
            waveSpawner.Wave[waveSpawner.currentWaveIndex].enemiesLeft--;
            gameObject.GetComponent<Collider>().enabled = false;
            state = State.Dead;
            Destroy(gameObject, 3);
        }
        else if (target && !IsDead) //If health is not 0, keep chasing target
        {
            //animator.SetBool("IsIdle", false);
            float distance = Vector3.Distance(transform.position, target.transform.position);

            // Attack target if the target is within attack range
            if (distance < attackRange && !Attacking)
            {
                state = State.Attack;
            }
            else if (distance > attackRange && !Attacking) // Chase target if the target is not within attack range
            {
                animator.ResetTrigger("IsAttacking");
                state = State.Chase;
            }
            else if (!target) // Stay Idle if there is no target
            {
                state = State.Idle;
            }
        }

        switch (state)
        {
            case State.Chase:
                agent.isStopped = false;
                animator.SetBool("IsRunning", true);
                agent.SetDestination(target.transform.position);
                break;

            case State.Attack:
                agent.isStopped = true;
                animator.SetBool("IsRunning", false);
                StartCoroutine(StartAttacking());
                break;

            case State.Idle:
                agent.isStopped = true;
                animator.SetBool("IsIdle", true);
                break;

            case State.Dead:
                IsDead = true;
                agent.isStopped = true;
                animator.SetBool("IsDead", true);
                break;
        }
    }

    IEnumerator StartAttacking()
    {
        Attacking = true;

        //animator.SetInteger("AttackType", RandomNum);
        animator.SetTrigger("IsAttacking");
        yield return new WaitForSeconds(1.5f);
        animator.ResetTrigger("IsAttacking");

        Attacking = false;
    }

    public void takeDamage(float gunDamage)
    {
        healthSystem.Damage(gunDamage);
        Debug.Log(healthSystem.GetHealth());
    }

    public void ZombieEnableCollider()
    {
        //GameObject currentWeapon = weapon[animator.GetInteger("AttackType")];

        weapon.GetComponent<Collider>().enabled = true;

    }

    public void ZombieDisableCollider()
    {
        //GameObject currentWeapon = weapon[animator.GetInteger("AttackType")];

        weapon.GetComponent<Collider>().enabled = false;
        //RandomNum = Random.Range(0, 2);
        player.isHit = false;
    }
}
