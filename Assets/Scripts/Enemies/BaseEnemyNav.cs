using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BaseEnemyNav : MonoBehaviour
{
    [Header("BASE ENEMY STATS")]
    [SerializeField] private float _health;
    [SerializeField] private float _damage;
    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private float navAgentOffset;
    [SerializeField] private GameObject firstAidPrefab;
    [SerializeField] private int probabilityAid = 10;
    public int numberOfEnemiesSameRoom = 3;


    private float _currentHealth;
    private float _currentDamage;
    private float _currentSpeed;

    [HideInInspector]
    public Transform player => GameObject.Find("XR Origin/Camera Offset/Main Camera").transform;

    [HideInInspector]
    public NavMeshAgent navAgent;

    [HideInInspector]
    public bool playerInRoom = false;


    public virtual void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();

        //It starts disabled because when creating the maze the nav Agent prevents the object to move the enemy correctly and enabling it later fixes it
        navAgent.enabled = true;
        navAgent.baseOffset = navAgentOffset;

        RestartStats();
        
    }

    public void SetNavOffset(float num)
    {
        navAgent.baseOffset = num;
    }

    public void PlayerInRoom()
    {
        playerInRoom = true;
    }
    private void RestartStats()
    {
        _currentHealth = _health * LevelMultiplier();
        _currentDamage = _damage * LevelMultiplier();
        _currentSpeed = _speed;
        navAgent.speed = _currentSpeed;
    }

    public float LevelMultiplier()
    {
        return (PlayerPrefs.GetInt("Level") - 1 )/10f + 1  ;
    }

    public void DoDamage(float damage)
    {

        _currentHealth -= damage;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _health);

        if (_currentHealth == 0)
        {
            GameManager.ChangeScore("enemy");

            if (Random.Range(0, probabilityAid) == 0)
            {
                Instantiate(firstAidPrefab, transform.position, Quaternion.identity);

            }

            FindObjectOfType<OwnAudioManager>().Play("DieEnemy");
            Destroy(gameObject);
        }
    }

    public void ChangeStat(float amount, bool increase, string type)
    {

        switch (type)
        {
            case "damage":
                if (increase) _currentDamage += amount;
                else _currentDamage -= amount;
                break;
            //this can be used for slowing or speed up the enemy
            case "speed":
                if (increase) _currentSpeed += amount;
                else _currentSpeed -= amount;

                navAgent.speed = _currentSpeed;

                break;
        }
    }
    public float GetCurrentDamageStat() { return _currentDamage; }
    public float GetCurrentHealthStat() { return _currentHealth; }
    public float GetCurrentSpeedStat() { return _currentSpeed; }
    public float GetInitialDamageStat() { return _damage; }
    public float GetInitialHealthStat() { return _health; }
    public float GetInitialSpeedStat() { return _speed; }

    //to change directly the sped of the enemy
    public void ChangeAgentSpeed(float speed)
    {
        _currentSpeed = speed;
        navAgent.speed = speed;
    }

    public Vector3 GetPlayerPos()
    {
        return new Vector3(player.position.x, 0, player.position.z);
    }

    public virtual void GoToPlayer()
    {
        Debug.Log("Go to");
        navAgent.destination = GetPlayerPos();
        navAgent.isStopped = false;
    }

    public virtual void Stop()
    {
        navAgent.isStopped = true;
    }

    public Vector3 GetRandomPos(Vector3 center, float range)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            Debug.DrawRay(navAgent.destination, Vector3.up, Color.blue, 10.0f);
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                return hit.position;
                
            }
        }
        return  Vector3.zero;
    }



}
