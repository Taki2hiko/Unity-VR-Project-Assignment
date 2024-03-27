using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyBoss : MonoBehaviour, ITakeDamage
{
    enum State
    {
        Idle,
        Chase,
        Attack,
        Dead,
    }

    [Header("Enemy Weapon")]
    [SerializeField] private GameObject[] weapon;

    private Animator animator;
    private Rigidbody rb;
    private NavMeshAgent agent;

    private HealthSystem healthSystem;
    private NormalSpawner waveSpawner;
    private PlayerController player;
    private GameObject target;
    private State state;

    private bool Attacking = false;
    private int RandomNum = 0;
    public float BossDamage = 10;
    [SerializeField] private bool IsDead = false;
    [SerializeField] private float attackRange = 5f;

    // Start is called before the first frame update
    void Start()
    {
        waveSpawner = GetComponentInParent<NormalSpawner>();
        healthSystem = GetComponent<HealthSystem>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player");
        player = FindObjectOfType<PlayerController>();

        healthSystem.SetMaxHealth(500);
        healthSystem.SetHealth(500);
    }

    // Update is called once per frame
    void Update()
    {
        if (healthSystem.GetHealth() <= 0 && !IsDead)
        {
            gameObject.GetComponent<Collider>().enabled = false;
            state = State.Dead;
            Destroy(gameObject, 3);
        }
        else if (target && !IsDead)
        {
            animator.SetBool("IsIdle", false);
            float distance = Vector3.Distance(transform.position, target.transform.position);

            if (distance < attackRange && !Attacking)
            {
                state = State.Attack;
            }
            else if (distance > attackRange && !Attacking)
            {
                animator.ResetTrigger("IsAttacking");
                state = State.Chase;
            }
            else if (!target)
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
                waveSpawner.Wave[waveSpawner.currentWaveIndex].enemiesLeft--; // this can't be put here
                break;
        }
    }


    IEnumerator StartAttacking()
    {
        Attacking = true;

        animator.SetInteger("AttackType", RandomNum);
        animator.SetTrigger("IsAttacking");
        //yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        yield return new WaitForSeconds(1.2f);
        animator.ResetTrigger("IsAttacking");
        //RandomNum = Random.Range(0, 2);
        //animator.SetBool("IsAttacking", false);

        Attacking = false;
    }

    public void takeDamage(float pistolDamage)
    {
        healthSystem.Damage(pistolDamage);
        Debug.Log(healthSystem.GetHealth());
    }

    public void EnableCollider()
    {
        GameObject currentWeapon = weapon[animator.GetInteger("AttackType")];

        currentWeapon.GetComponent<Collider>().enabled = true;

    }

    public void DisableCollider()
    {
        GameObject currentWeapon = weapon[animator.GetInteger("AttackType")];

        currentWeapon.GetComponent<Collider>().enabled = false;
        RandomNum = Random.Range(0, 2);
        player.isHit = false;
    }

    public void OnAttackAnimationEnd()
    {

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 2);
    }
}
