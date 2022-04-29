using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BaseEnemyNav : MonoBehaviour
{
    [SerializeField] private float _health;
    [SerializeField] private float _damage;
    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private float navAgentOffset;


    private float _currentHealth;
    private float _currentDamage;
    private float _currentSpeed;

    private Transform player => GameObject.Find("XR Origin").transform;
    [HideInInspector]
    public NavMeshAgent navAgent;

    public virtual void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.baseOffset = navAgentOffset;

        RestartStats();
        
    }

    private void RestartStats()
    {
        _currentHealth = _health;
        _currentDamage = _damage;
        _currentSpeed = _speed;
        navAgent.speed = _currentSpeed;
    }

    public void DoDamage(float damage)
    {

        _currentHealth -= damage;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _health);

        if (_currentHealth == 0)
        {
            GameManager.ChangeScore("enemy");
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

    private Vector3 GetPlayerPos()
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



}
