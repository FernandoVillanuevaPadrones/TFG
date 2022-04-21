using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using System;

public class Player : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private float _health = 100;
    [SerializeField] private float _speed = 1.5f;

    [Header("Gun")]
    [SerializeField] private Weapon _weaponScript;
 
    private ActionBasedContinuousMoveProvider _actionContinuous => gameObject.transform.Find("Locomotion System").GetComponent<ActionBasedContinuousMoveProvider>();

    private XRDirectInteractor _directInteractor => gameObject.transform.Find("Camera Offset/LeftHand/LeftHand Controller").GetComponent<XRDirectInteractor>();

    [Header("Inputs")]
    [SerializeField] private InputActionReference _useObjectInput;
    [SerializeField] private InputActionReference _menuInput;

    [Header("Menu")]
    [SerializeField] private GameObject menuGB;
    [SerializeField] private GameObject menuGBOffset;


    private float _currentHealth;
    private float _currentSpeed;

    private void Start()
    {
        _currentHealth = _health;
        _currentSpeed = _speed;

        _actionContinuous.moveSpeed = _currentSpeed;
        _useObjectInput.action.started += UseObject;

        _menuInput.action.started += PauseMenu;
    

    }

    private void FixedUpdate()
    {

        menuGBOffset.transform.localEulerAngles = new Vector3(0, Camera.main.transform.localEulerAngles.y, 0);
    }
    private void PauseMenu(InputAction.CallbackContext obj)
    {
        menuGB.SetActive(!menuGB.activeSelf);

        if (menuGB.activeSelf)
        {
            /*
            var position = new Vector3(Camera.main.transform.position.x, 1.151f, Camera.main.transform.position.z);
            menuGB.transform.position = position + Camera.main.transform.forward * 2.4f;
            
            menuGB.transform.rotation = Quaternion.LookRotation(menuGB.transform.position - Camera.main.transform.position);
            */
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    private void UseObject(InputAction.CallbackContext obj)
    {
        if (!_directInteractor.hasSelection || !_directInteractor.interactablesSelected[0].transform.CompareTag("ConsumableObject"))
        {
            return;
        }

        
       

        Object _currentObject = _directInteractor.selectTarget.GetComponent<Object>();
        float _amountChange = _currentObject._ammountToChange;

        switch (_currentObject.type)
        {
            case Object.ObjectType.fireRate:
                _weaponScript.ChangeFireRate(_amountChange);
                break;
            case Object.ObjectType.damage:
                _weaponScript.ChangeDamage(_amountChange);
                break;
            case Object.ObjectType.speed:
                ChangeSpeed(_amountChange);
                break;
            case Object.ObjectType.health:
                ChangeHealth(_amountChange);
                break;
            case Object.ObjectType.cooldown:
                break;
            case Object.ObjectType.special:
                switch (_currentObject.special)
                {
                    case Object.SpecialType.Nothing:
                        break;
                    case Object.SpecialType.DoubleShoot:
                        _weaponScript.ChangeNumberShoots(2);
                        break;
                    case Object.SpecialType.TripleShoot:
                        _weaponScript.ChangeNumberShoots(3);
                        break;
                    case Object.SpecialType.Infinity:
                        break;
                    case Object.SpecialType.BombShoot:
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }

        Destroy(_directInteractor.selectTarget.gameObject);
    }

    public void ChangeSpeed(float changeSpeed)
    {
        _actionContinuous.moveSpeed += changeSpeed;
        _actionContinuous.moveSpeed = Mathf.Clamp(_actionContinuous.moveSpeed, 1f, 3f);
    }

    public void ChangeHealth(float num)
    {
        _currentHealth += num;
        Debug.Log("Current health: " + _currentHealth);       
        _currentHealth = Mathf.Clamp(_currentHealth, 0f, 100f);

        if (_currentHealth <= 0f)
        {
            Debug.Log("MUERTO, HACER CODIGO");
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Room"))
        {
            
            other.transform.GetComponentInParent<RoomScript>().CloseDoors();
        }
        else if (other.transform.CompareTag("Capsule"))
        {
            other.transform.GetComponentInParent<Capsule>().NextAnimState();
        }
    }
}
