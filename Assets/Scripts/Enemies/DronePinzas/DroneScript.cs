using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class DroneScript : BaseEnemyNav
{
    [Header("Drone Settings")]
    [SerializeField] private float walkRadius;
    [SerializeField] private int limitOfWonders = 5;
    [SerializeField] private int probGoToPlayer = 5;
    [SerializeField] private CapsuleCollider collider;

    private int totalWonders = 0;
    private Animator animator;

    private bool spin = false;
    private bool randomPos = false;

    private AudioSource audioSource;

    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        collider.enabled = false;
    }


    private void Update()
    {
        audioSource.volume = GameManager.soundEffectLevel;
        if (!randomPos)
        {
            //needed to give the enemy a random pos inside the room. Can not be done earlier bc of navagent and room position changing
            randomPos = true;
            transform.position = GetRandomPos(transform.position, 7f);
        }

        if (playerInRoom)
        {
            //so the player can not kill the boss from outside of the room
            collider.enabled = true;

            float dist = navAgent.remainingDistance;
            if (totalWonders < limitOfWonders && spin)
            {
                if (dist != Mathf.Infinity && navAgent.pathStatus == NavMeshPathStatus.PathComplete && GetComponent<NavMeshAgent>().remainingDistance == 0)//Arrived.
                {
                    if (!audioSource.isPlaying)
                    {
                        audioSource.Play();
                    }
                    Wonder();
                    totalWonders++;
                }
            }
            else if (spin != false && dist != Mathf.Infinity && navAgent.pathStatus == NavMeshPathStatus.PathComplete && GetComponent<NavMeshAgent>().remainingDistance == 0)
            {
                totalWonders = 0;
                animator.SetBool("Spin", false);
                spin = false;

                audioSource.Stop();
            }
        }
    }

    public void IdleFinished()
    {
        if (playerInRoom)
        {

            animator.SetBool("Spin", true);
            spin = true;


        }
    }

    void Wonder()
    {
        if (Random.Range(0, probGoToPlayer) == 0)
        {
            GoToPlayer();
        }
        else
        {

            Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
            Vector3 finalPosition = hit.position;
            navAgent.destination = finalPosition;
            transform.LookAt(finalPosition);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            other.transform.GetComponentInParent<Player>().ChangeHealth(GetCurrentDamageStat());
        }
    }
}
