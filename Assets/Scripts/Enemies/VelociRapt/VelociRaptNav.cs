using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelociRaptNav : BaseEnemyNav
{
    [Header("VELOC STATS")]
    [SerializeField] private float alertRadius = 1f;
    [SerializeField] private float timeToTurn;
    [SerializeField] private float showSpeed = 0.005f;
    [SerializeField] private Material velociMaterial;

    [Header("Sounds")]
    [SerializeField] private AudioSource alertSource;
    [SerializeField] private AudioSource stepsSource;

    public enum State { Idle, Attack, Alert, Turn, Running }

    public State currentState = State.Idle;

    private Animator animator;
    private string threatenString = "Threaten";
    private string runString = "Run";
    private bool randomPos = false;


    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        velociMaterial.SetFloat("DissolveProgressFloat", 1f);
        
    }

    void Update()
    {
        alertSource.volume = GameManager.soundEffectLevel;
        stepsSource.volume = GameManager.soundEffectLevel;

        if (!randomPos)
        {
            //needed to give the enemy a random pos inside the room. Can not be done earlier bc of navagent and room position changing
            randomPos = true;
            transform.position = GetRandomPos(transform.position, 7f);
        }
        if (playerInRoom)
        {
            if (currentState == State.Attack)
            {
                //GoToPlayer();
                navAgent.destination = GetRandomPos(GetPlayerPos(), 0.2f);

                stepsSource.loop = true;
                stepsSource.Play();
                currentState = State.Running;
            }
            else if (currentState == State.Running)
            {
                if (navAgent.remainingDistance <= 0.2f)
                {
                    currentState = State.Turn;
                    animator.SetBool(threatenString, false);
                    animator.SetBool(runString, false);

                }
            }
            else if (currentState == State.Turn)
            {
                //Llamarlo solo una vez
                if (stepsSource.isPlaying)
                {
                    StartCoroutine(Timer(timeToTurn));

                }

            }

        }



    }


    private IEnumerator Timer(float time)
    {
        stepsSource.Stop();
        yield return new WaitForSeconds(time);
        currentState = State.Running;
        //GoToPlayer();

        navAgent.destination = GetRandomPos(GetPlayerPos(), 0.2f);
        
        animator.SetBool(runString, true);
        stepsSource.Play();

    }

    public void GetAlerted()
    {
        //transform.LookAt(base.GetPlayerPos()..transform);
        //currentState = State.Alert;
        animator.SetBool(threatenString, true);
        animator.SetBool(runString, true);

        alertSource.PlayDelayed(1f);
    }

    public void AlertOthers()
    {
        GameObject parent = transform.parent.gameObject;

        for (int i = 0; i < parent.transform.childCount; i++)
        {
            var script = parent.transform.GetChild(i).GetComponent<VelociRaptNav>();
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

    //Called from animation event
    public void ChangeState(State state)
    {
        currentState = state;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            other.gameObject.GetComponentInParent<Player>().ChangeHealth(base.GetCurrentDamageStat());
        }
    }

}
