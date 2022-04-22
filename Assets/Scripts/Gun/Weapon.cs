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



    private WaitForSeconds _waitShoot;
    

    //---------- Shoot Changes --------------
    [Range(1,3)]
    [SerializeField]private int _numberShoots = 1;



    private float _currentDamage;
    private float _currentShootForce;
    private float _currentFireRate;
    private int _currentNumberShoots;
    private bool _shooting = false;

    private GameObject leftBoquilla;
    private GameObject rightBoquilla;

    protected virtual void Awake()
    {
        
    }
    private void Start()
    {

        //Saved Game = 0 (no game saved/like False), == 1 saved Game
        if (PlayerPrefs.GetInt("SavedGame") == 0)
        {
            RestartStats();
        }
        else
        {
            _currentDamage = PlayerPrefs.GetFloat("GunDamage");
            _currentShootForce = PlayerPrefs.GetFloat("GunShootForce");
            _currentFireRate = PlayerPrefs.GetFloat("GunFireRate");
            _currentNumberShoots = PlayerPrefs.GetInt("GunNumberShoots");
        }

        

        _waitShoot = new WaitForSeconds(1 / _currentFireRate);

        

        leftBoquilla = transform.Find("BoquillaIzq").gameObject;
        rightBoquilla = transform.Find("BoquillaDer").gameObject;

        leftBoquilla.SetActive(false);
        rightBoquilla.SetActive(false);
    }

    public void ShootTrigger()
    {
        if (!_shooting)
        {
            _shooting = true;
            StartCoroutine(Shoot());
        }
    }
    public void StopTrigger()
    {
        StopAllCoroutines();
        _shooting = false;
    }
    private IEnumerator Shoot()
    {
        Debug.Log("Shoot: " + _currentNumberShoots);
        while (true)
        {
            switch (_currentNumberShoots)
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
        if (_currentNumberShoots == 1 && num != 1)
        {
            leftBoquilla.SetActive(true);
            rightBoquilla.SetActive(true);
        }
        else if (_currentNumberShoots != 1 && num == 1)
        {
            leftBoquilla.SetActive(false);
            rightBoquilla.SetActive(false);
        }

        _currentNumberShoots = Mathf.Clamp(num, 1, 3);

        
    }
    public void ChangeFireRate(float num) {
        _currentFireRate += num;
        _waitShoot = new WaitForSeconds(1 / _currentFireRate);
    }
    public void ChangeDamage(float num) {
        _currentDamage += num;
    }

    public void RestartStats()
    {
        _currentDamage = _damage;
        _currentShootForce = _shootForce;
        _currentFireRate = _fireRate;
        _currentNumberShoots = _numberShoots;
    }

    public void SaveStats()
    {
        PlayerPrefs.SetFloat("GunDamage", _currentDamage);
        PlayerPrefs.SetFloat("GunShootForce", _currentShootForce);
        PlayerPrefs.SetFloat("GunFireRate", _currentFireRate);
        PlayerPrefs.SetInt("GunNumberShoots", _currentNumberShoots);
    }
}
