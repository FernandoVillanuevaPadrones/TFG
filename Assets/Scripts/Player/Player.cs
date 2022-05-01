using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using System;
using TMPro;

public class Player : MonoBehaviour
{

    [Header("Player Stats")]
    [SerializeField] private float _health = 100;
    [SerializeField] private float _speed = 1.5f;

    [Header("Gun")]
    [SerializeField] private Weapon _weaponScript;
 
    private ActionBasedContinuousMoveProvider _actionContinuous => gameObject.transform.Find("Locomotion System").GetComponent<ActionBasedContinuousMoveProvider>();

    private XRDirectInteractor _leftDirectInteractor => gameObject.transform.Find("Camera Offset/LeftHand/LeftHand Controller").GetComponent<XRDirectInteractor>();
    private XRDirectInteractor _rightDirectInteractor => gameObject.transform.Find("Camera Offset/RightHand/RightHand Controller").GetComponent<XRDirectInteractor>();

    [Header("Inputs")]
    [SerializeField] private InputActionReference _useObjectInput;
    [SerializeField] private InputActionReference _menuInput;

    [Header("Menu")]
    [SerializeField] private GameObject menuGB;
    [SerializeField] private GameObject menuGBOffset;
    [SerializeField] private GameObject deadMenu;
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("HealthHUD")]
    [SerializeField] private TextMeshProUGUI lifeLineText;
    [SerializeField] private LifeLine lifeLineScript;

    [Header("MazeGenerator")]
    [SerializeField] private MazeGenerator mazeScript;


    private float _currentHealth;
    private float _currentSpeed;

    // 0 false, 1 true
    [HideInInspector]
    public static int showMapUpgrade = 0;

    private void Start()
    {
        //Saved Game = 0 (no game saved/like False), == 1 saved Game
        if (PlayerPrefs.GetInt("SavedGame") == 0)
        {
            _currentHealth = _health;
            _currentSpeed = _speed;
            showMapUpgrade = 0;
        }
        else
        {
            _currentHealth = PlayerPrefs.GetFloat("PlayerHealth");
            _currentSpeed = PlayerPrefs.GetFloat("PlayerSpeed");
            showMapUpgrade = PlayerPrefs.GetInt("ShowMapUpgrade");
        }

        if (showMapUpgrade == 1)
        {
            mazeScript.ShowAllMap();
        }

        //QUitar
        _currentHealth = _health;
        _currentSpeed = _speed;


        lifeLineText.text = _currentHealth.ToString();


        

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
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    private void UseObject(InputAction.CallbackContext obj)
    {

        if ((_leftDirectInteractor.hasSelection && _leftDirectInteractor.interactablesSelected[0].transform.CompareTag("ConsumableObject")) 
            || (_rightDirectInteractor.hasSelection && _rightDirectInteractor.interactablesSelected[0].transform.CompareTag("ConsumableObject")))
        {
            Object _currentObject;

            if (_leftDirectInteractor.hasSelection && _leftDirectInteractor.interactablesSelected[0].transform.CompareTag("ConsumableObject"))
            {
                _currentObject = _leftDirectInteractor.selectTarget.GetComponent<Object>();
            }
            else
            {
                _currentObject = _rightDirectInteractor.selectTarget.GetComponent<Object>();
            }

            
            float _amountChange = _currentObject._ammountToChange;

            switch (_currentObject.type)
            {
                case ScriptableObjectConsumable.ObjectType.fireRate:
                    _weaponScript.ChangeFireRate(_amountChange);
                    break;
                case ScriptableObjectConsumable.ObjectType.damage:
                    _weaponScript.ChangeDamage(_amountChange);
                    break;
                case ScriptableObjectConsumable.ObjectType.speed:
                    ChangeSpeed(_amountChange);
                    break;
                case ScriptableObjectConsumable.ObjectType.health:
                    ChangeHealth(_amountChange);
                    break;
                case ScriptableObjectConsumable.ObjectType.cooldown:
                    break;
                case ScriptableObjectConsumable.ObjectType.special:
                    switch (_currentObject.special)
                    {
                        case ScriptableObjectConsumable.SpecialType.Nothing:
                            break;
                        case ScriptableObjectConsumable.SpecialType.DoubleShoot:
                            _weaponScript.ChangeNumberShoots(2);
                            break;
                        case ScriptableObjectConsumable.SpecialType.TripleShoot:
                            _weaponScript.ChangeNumberShoots(3);
                            break;
                        case ScriptableObjectConsumable.SpecialType.Infinity:
                            _weaponScript.InfinityShoots();
                            break;
                        case ScriptableObjectConsumable.SpecialType.BombShoot:
                            break;
                        case ScriptableObjectConsumable.SpecialType.ShowMap:
                            showMapUpgrade = 1;
                            PlayerPrefs.SetInt("ShowMapUpgrade", 1);
                            mazeScript.ShowAllMap();
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }

            Destroy(_currentObject.gameObject);
        }
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
        _currentHealth = Mathf.Clamp(_currentHealth, 0f, _health);

        lifeLineText.text = _currentHealth.ToString();
        lifeLineScript.ChangeLineSpeed(_currentHealth);

        if (_currentHealth <= 0f)
        {
            Dead();
            PauseGame();

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Room"))
        {
           other.transform.GetComponentInParent<RoomScript>().PlayerInRoom();
        }
        else if (other.transform.CompareTag("Capsule"))
        {
            other.transform.GetComponentInParent<Capsule>().NextAnimState();
        }
    }


    private void Dead()
    {
        deadMenu.SetActive(true);
        scoreText.text = "Score: " + GameManager.GetScore().ToString();


        //Needed in order to not get an error due to having twice the same function added to the button
        _useObjectInput.action.started -= UseObject;
        _menuInput.action.started -= PauseMenu;

        //Dead so next game must be new
        PlayerPrefs.SetInt("SavedGame", 0);
    }

    public void SaveStats()
    {
        _useObjectInput.action.started -= UseObject;
        _menuInput.action.started -= PauseMenu;
        PlayerPrefs.SetFloat("PlayerHealth", _currentHealth);
        PlayerPrefs.SetFloat("PlayerSpeed", _currentSpeed);
        PlayerPrefs.SetInt("Score", GameManager.GetScore());
        PlayerPrefs.SetInt("ShowMapUpgrade", showMapUpgrade);
        PlayerPrefs.SetInt("SavedGame", 1);
    }
}
