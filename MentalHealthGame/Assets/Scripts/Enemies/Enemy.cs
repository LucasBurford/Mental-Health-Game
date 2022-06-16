using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Player player;
    public NavMeshAgent ai;

    public enum States
    {
        Idle,
        Chasing,
        Attacking
    }
    public States currentState;

    public float maxHealth;
    public float currentHealth;
    public float aggroDistance;
    public float attackFromDistance;
    public float attackResetTime;
    public float aiStoppingDistance;

    public bool canAttack;
    public bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        ai = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;
        ai.stoppingDistance = aiStoppingDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            CheckState();
        }

        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            Die();
        }
    }

    private void CheckState()
    {
        switch (currentState)
        {
            case States.Idle:
                {
                    ai.isStopped = true;
                    CheckDistance();
                }
                break;
            case States.Chasing:
                {
                    ai.isStopped = false;
                    ai.SetDestination(player.transform.position);

                    if (Vector3.Distance(transform.position, player.transform.position) <= attackFromDistance)
                    {
                        currentState = States.Attacking;
                    }
                }
                break;
            case States.Attacking:
                {
                    ai.isStopped = true;

                    if (canAttack)
                    {
                        canAttack = false;
                        Attack();
                        StartCoroutine(WaitToResetAttack());
                    }
                }
                break;
        }
    }

    private void Attack()
    {

    }

    private void CheckDistance()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= aggroDistance)
        {
            currentState = States.Chasing;
        }
        else
        {
            currentState = States.Idle;
        }
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    IEnumerator WaitToResetAttack()
    {
        yield return new WaitForSeconds(attackResetTime);
        canAttack = true;
    }
}
