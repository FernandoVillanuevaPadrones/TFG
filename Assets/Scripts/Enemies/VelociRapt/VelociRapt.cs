using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelociRapt : BaseEnemy
{
    [SerializeField] private float alertRadius = 1f;
    [SerializeField] private float turnSpeed = 1f;
    [SerializeField] private float timeToTurn;
   


    public enum State { Idle, Attack, Alert, Turn, Running, Turning}

    public State currentState = State.Idle;

    private Animator animator;
    private string threatenString = "Threaten";
    private string runString = "Run";
    private Vector3 objetivePos;


    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();

    }

    void Update()
    {
        
        if (currentState == State.Attack)
        {
            objetivePos = base.GetPlayerPos();
            transform.LookAt(objetivePos);
            currentState = State.Running;
        }
        else if (currentState == State.Running)
        {
            transform.position = Vector3.MoveTowards(transform.position, objetivePos, base.GetCurrentSpeedStat() * Time.deltaTime);
            if (transform.position == objetivePos)
            {
                currentState = State.Turn;
                animator.SetBool(threatenString, false);
                animator.SetBool(runString, false);

            }
        }
        else if (currentState == State.Turn)
        {

            StartCoroutine(Timer(timeToTurn));
            currentState = State.Turning;

        }
        else if (currentState == State.Turning)
        {
            objetivePos = base.GetPlayerPos();
            // Determine which direction to rotate towards
            Vector3 targetDirection = objetivePos - transform.position;

            // The step size is equal to speed times frame time.
            float singleStep = turnSpeed * Time.deltaTime;

            // Rotate the forward vector towards the target direction by one step
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

            // Draw a ray pointing at our target in
            Debug.DrawRay(transform.position, newDirection, Color.red);

            // Calculate a rotation a step closer to the target and applies rotation to this object
            transform.rotation = Quaternion.LookRotation(newDirection);
        }

    }

    private IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);
        currentState = State.Running;
        objetivePos = base.GetPlayerPos();
        animator.SetBool(runString, true);
    }

    

    public void GetAlerted()
    {
        //transform.LookAt(base.GetPlayerPos()..transform);
        currentState = State.Alert;
        animator.SetBool(threatenString, true);
        animator.SetBool(runString, true);
    }

    public void AlertOthers()
    {
        GameObject parent = transform.parent.gameObject;

        for (int i = 0; i < parent.transform.childCount; i++)
        {
            var script = parent.transform.GetChild(i).GetComponent<VelociRapt>();
            if (script.currentState == State.Idle)
            {
                script.GetAlerted();
            }
           
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, alertRadius);
    }

    public void ChangeState(State state) {
        currentState = state;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("entra");
        Debug.Log(other.transform.tag);
        if (other.transform.CompareTag("Player"))
        {
            other.gameObject.GetComponentInParent<Player>().ChangeHealth(base.GetCurrentDamageStat());
        }
    }

}
