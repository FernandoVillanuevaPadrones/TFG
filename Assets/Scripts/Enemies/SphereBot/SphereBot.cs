using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereBot : BaseEnemyNav
{

    [Header("Sphere Stats")]
    [SerializeField] private float rollingSpeed = 1f;
    [SerializeField] private float walkSpeed = 1f;
    [SerializeField] private float attackInterval = 1f;
    [SerializeField] private float projectileSpeed = 1f;
    [SerializeField] private SphereProjectile projectile;
    [SerializeField] private float projectileDamage = -20f;
    [SerializeField] private float rollingDamage = -10f;


    [SerializeField] private BoxCollider boxCollider;



    [SerializeField] private Transform[] projectPointsAttack;
    [SerializeField] private Transform[] projectPointsJumpAttack;

    private bool attacking = false;
    private bool rolling = false;
    private bool running = false;
    private bool isDoingAction = false;

    private int randomAction;
    private int lastAction = -1;
    private Animator animator;

    public override void Start()
    {
        base.Start();

        animator = GetComponentInChildren<Animator>();

        projectileDamage *= LevelMultiplier();
        rollingDamage *= LevelMultiplier();
        boxCollider.enabled = false;
    }



    public override void Update()
    {
        base.Update();

        if (!isDoingAction && playerInRoom )
        {
            isDoingAction = true;
            rolling = false;
            running = false;
            //so the player can not kill the boss from outside of the room
            boxCollider.enabled = true;
            Stop();

            randomAction = Random.Range(0, 4);

            if (lastAction != -1)
            {
                while (lastAction == randomAction)
                {
                    randomAction = Random.Range(0, 4);
                }
            }

            lastAction = randomAction;

            Debug.Log(randomAction);

            if (randomAction == 0)
            {
                animator.SetBool("Rolling", true);
                ChangeAgentSpeed(rollingSpeed);
                rolling = true;
                GoToPlayer();

                StartCoroutine(WaitForNextRolling());

            }
            else if (randomAction == 1)
            {
                animator.SetBool("Attack", true);
            }
            else if (randomAction == 2)
            {
                animator.SetBool("Jump", true);
            }
            else
            {
                animator.SetBool("Run", true);
                ChangeAgentSpeed(walkSpeed);
                running = true;
                
            }


        }


        if (rolling)
        {
            //When rolling the enemy will go to the player pos, but not all the time, just to the current position, and when reaching to the next one etc
            if (navAgent.remainingDistance <= 0.1)
            {
                StartCoroutine(WaitForNextRolling());
            }
        }
        else if (running) //In contrast, when running it will follow the player
        {
            GoToPlayer();
        }
    }

    IEnumerator WaitForNextRolling()
    {           
        yield return new WaitForSeconds(0.7f);
        GoToPlayer();
                     
    }




    IEnumerator ShootProjectiles()
    {
        while (attacking)
        {
            if (animator.GetBool("Attack") || animator.GetBool("Run"))
            {
                foreach (var point in projectPointsAttack)
                {
                    SphereProjectile instance = Instantiate(projectile, point.position, point.rotation);
                    instance.enemyParent = gameObject;
                    instance.projectileDamage = projectileDamage;
                    instance.Launch(projectileSpeed); 
                }
            }
            else if (animator.GetBool("Jump"))
            {
                foreach (var point in projectPointsJumpAttack)
                {
                    SphereProjectile instance = Instantiate(projectile, point.position, point.rotation);
                    instance.enemyParent = gameObject;
                    instance.projectileDamage = projectileDamage;
                    instance.Launch(projectileSpeed);
                }
            }

            yield return new WaitForSeconds(attackInterval);
        }
        yield return null;

    }

    //Called from animation
    public void AttackState(bool state)
    {
        attacking = state;

        if (attacking)
        {
            StartCoroutine(ShootProjectiles());
        }
    }

    public void FinishedAnimation(string boolName)
    {
        isDoingAction = false;
        animator.SetBool(boolName, false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            other.gameObject.GetComponentInParent<Player>().ChangeHealth(rollingDamage);
        }
    }
}
