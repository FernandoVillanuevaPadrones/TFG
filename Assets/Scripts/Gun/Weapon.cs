using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Weapon : MonoBehaviour
{
    [Header("Gun Stats")]
    [SerializeField] private float _damage;
    [SerializeField] private float _shootForce;
    [SerializeField] private float _fireRate;



    [Header("Fire points")]
    [SerializeField] protected Transform _mainFirePoint;
    [SerializeField] protected Transform _leftFirePoint;
    [SerializeField] protected Transform _rightFirePoint;

    [Header("Bullet Prefabs")]
    [SerializeField] private Projectile _bulletPrefab;

    [Header("Controls")]
    [SerializeField] private InputActionReference _shootInput;


    private WaitForSeconds _waitShoot;
    

    //---------- Shoot Changes --------------
    [Range(1,3)]
    [SerializeField]private int _numberShoots = 1;



    private float _currentDamage;
    private float _currentShootForce;
    private float _currentFireRate;


    protected virtual void Awake()
    {
        _shootInput.action.started += ShootTrigger;
        _shootInput.action.canceled += StopTrigger;
    }
    private void Start()
    {
        _waitShoot = new WaitForSeconds(1 / _fireRate);

        RestartStats();
    }

    private void ShootTrigger(InputAction.CallbackContext obj)
    {        
        StartCoroutine(Shoot());
    }
    private void StopTrigger(InputAction.CallbackContext obj)
    {
        StopAllCoroutines();
    }
    private IEnumerator Shoot()
    {
        while (true)
        {
            switch (_numberShoots)
            {
                case 1:
                    Projectile projectileInstance = Instantiate(_bulletPrefab, _mainFirePoint.position, _mainFirePoint.rotation);
                    projectileInstance.Init(this);
                    projectileInstance.Launch();
                    break;
                case 2:
                    Projectile projectileInstance2 = Instantiate(_bulletPrefab, _leftFirePoint.position, _leftFirePoint.rotation);
                    projectileInstance2.Init(this);
                    projectileInstance2.Launch();
                    Projectile projectileInstance3 = Instantiate(_bulletPrefab, _rightFirePoint.position, _rightFirePoint.rotation);
                    projectileInstance3.Init(this);
                    projectileInstance3.Launch();
                    break;

                case 3:
                    Projectile projectileInstance4 = Instantiate(_bulletPrefab, _mainFirePoint.position, _mainFirePoint.rotation);
                    projectileInstance4.Init(this);
                    projectileInstance4.Launch();
                    Projectile projectileInstance5 = Instantiate(_bulletPrefab, _leftFirePoint.position, _leftFirePoint.rotation);
                    projectileInstance5.Init(this);
                    projectileInstance5.Launch();
                    Projectile projectileInstance6 = Instantiate(_bulletPrefab, _rightFirePoint.position, _rightFirePoint.rotation);
                    projectileInstance6.Init(this);
                    projectileInstance6.Launch();
                    break;
                
            }            
            yield return _waitShoot;
        }                  
    }
    public float GetShootingForce()
    {
        return _currentShootForce;
    }
    public float GetDamage()
    {
        return _currentDamage;
    }
    public void ChangeNumberShoots(int num)
    {
        _numberShoots = Mathf.Clamp(num, 1, 3);
    }
    public void ChangeFireRate(float num) {
        _currentFireRate += num;
        _waitShoot = new WaitForSeconds(1 / _fireRate);
    }
    public void ChangeDamage(float num) {
        _currentDamage += num;
    }

    public void RestartStats()
    {
        _currentDamage = _damage;
        _currentShootForce = _shootForce;
        _currentFireRate = _fireRate;
    }
}
