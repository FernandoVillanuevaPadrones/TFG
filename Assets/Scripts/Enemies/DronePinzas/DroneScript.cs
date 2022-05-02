using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class DroneScript : BaseEnemyNav
{
    [SerializeField] private float walkRadius;
    [SerializeField] private int limitOfWonders = 5;

    private int totalWonders = 0;
    private Animator animator;

    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        Wonder();
    }


    private void Update()
    {   /*
        if (playerInRoom)
        {*/

        if (totalWonders < limitOfWonders)
        {
            totalWonders++;
            float dist = navAgent.remainingDistance;
            if (dist != Mathf.Infinity && navAgent.pathStatus == NavMeshPathStatus.PathComplete && GetComponent<NavMeshAgent>().remainingDistance == 0)//Arrived.
            {
                Wonder();
            }
        }
        else
        {
            totalWonders = 0;
            animator.SetBool("Spin", false); 
        }
        
    }

    public void IdleFinished()
    {
        animator.SetBool("Spin", true);
    }

    void Wonder()
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
