using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] private float _health;
    [SerializeField] private float _damage;
    [SerializeField] private float _speed;

    private float _currentHealth;
    private float _currentDamage;
    private float _currentSpeed;

    private Transform player => GameObject.Find("XR Origin").transform;
    public virtual void Start()
    {
        RestartStats();    
        
    }

    private void RestartStats()
    {
        _currentHealth = _health;
        _currentDamage = _damage;
        _currentSpeed = _speed;
    }

    public void DoDamage(float damage) {

        _currentHealth -= damage;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _currentHealth);

        if (_currentHealth == 0)
        {
            Destroy(gameObject);
        }
    }

    public void ChangeStat(float amount, bool increase, string type) {

        switch (type)
        {
            case "damage":
                if (increase) _currentDamage += amount;
                else _currentDamage -= amount;
                break;

            case "speed":
                if (increase) _currentSpeed += amount;
                else _currentSpeed -= amount;
                break;
        }        
    }
    public float GetCurrentDamageStat() { return _currentDamage; }
    public float GetCurrentHealthStat() { return _currentHealth; }
    public float GetCurrentSpeedStat() { return _currentSpeed; }
    public float GetInitialDamageStat() { return _damage; }
    public float GetInitialHealthStat() { return _health; }
    public float GetInitialSpeedStat() { return _speed; }

    public virtual Vector3 GetPlayerPos() {
        return new Vector3(player.position.x, 0, player.position.z);
    }

}
