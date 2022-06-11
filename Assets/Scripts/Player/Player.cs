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
    [SerializeField] private TextMeshProUGUI highestScoreText;

    [Header("HealthHUD")]
    [SerializeField] private TextMeshProUGUI[] lifeLineTexts;
    [SerializeField] private LifeLine[] lifeLineScripts;

    [Header("MazeGenerator")]
    [SerializeField] private MazeGenerator mazeScript;

    [Header("MazeGenerator")]
    [SerializeField] private GameObject shieldGB;

    [SerializeField] private GameObject heliGB;

    private AudioSource audioSource;

    private bool invincible = false;

    private float _currentHealth;
    private float _currentSpeed;

    // 0 false, 1 true
    [HideInInspector]
    public static int showMapUpgrade = 0;

    private int hasHeli = 0;

    private void Start()
    {
        //Saved Game = 0 (no game saved/like False), == 1 saved Game
 
        if (PlayerPrefs.GetInt("SavedGame") == 0)
        {
            _currentHealth = _health;
            _currentSpeed = _speed;
            showMapUpgrade = 0;
            hasHeli = 0;
        }
        else
        {
            _currentHealth = PlayerPrefs.GetFloat("PlayerHealth");
            _currentSpeed = PlayerPrefs.GetFloat("PlayerSpeed");
            showMapUpgrade = PlayerPrefs.GetInt("ShowMapUpgrade");
            hasHeli = PlayerPrefs.GetInt("HasHeli");
            if (hasHeli == 1)
            {
                heliGB.SetActive(true);
                heliGB.transform.localScale = Vector3.one;
                RobotHelicoptero.canLookCamera = true;
            }
        }



        foreach (var text in lifeLineTexts)
        {
            text.text = _currentHealth.ToString();

        }
      
        _actionContinuous.moveSpeed = _currentSpeed;
        _useObjectInput.action.started += UseObject;

        _menuInput.action.started += PauseMenu;

        audioSource = GetComponent<AudioSource>();

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
            var hasSelection = "LeftHand";
            if (_leftDirectInteractor.hasSelection)
            {
                _currentObject = _leftDirectInteractor.selectTarget.GetComponent<Object>();
            }
            else
            {
                _currentObject = _rightDirectInteractor.selectTarget.GetComponent<Object>();
                hasSelection = "RightHand";
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
                        case ScriptableObjectConsumable.SpecialType.HelicRobot:
                            heliGB.SetActive(true);
                            if (hasSelection == "LeftHand")
                            {
                                RobotHelicoptero.onLeftHand = true;
                            }
                            else
                            {
                                RobotHelicoptero.onRightHand = true;
                            }
                            RobotHelicoptero.doInitialAnimation = true;
                            hasHeli = 1;
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

        else if ((_leftDirectInteractor.hasSelection && _leftDirectInteractor.interactablesSelected[0].transform.CompareTag("FirstAid"))
            || (_rightDirectInteractor.hasSelection && _rightDirectInteractor.interactablesSelected[0].transform.CompareTag("FirstAid")))
        {
            ChangeHealth(50f);
            if (_leftDirectInteractor.hasSelection)
            {
                Destroy(_leftDirectInteractor.selectTarget.gameObject);
            }
            else
            {
                Destroy(_rightDirectInteractor.selectTarget.gameObject);
            }
        }
    }

    public void ChangeSpeed(float changeSpeed)
    {
        _actionContinuous.moveSpeed += changeSpeed;
        _actionContinuous.moveSpeed = Mathf.Clamp(_actionContinuous.moveSpeed, 1f, 3f);
    }

    public void ChangeHealth(float num)
    {


        if (!invincible)
        {
            invincible = true;
            var lastHealth = _currentHealth;
            _currentHealth += num;
            _currentHealth = Mathf.Clamp(_currentHealth, 0f, _health);

            if (lastHealth > _currentHealth) // Was Damaged
            {
                audioSource.Play();

            }

            foreach (var text in lifeLineTexts)
            {
                text.text = _currentHealth.ToString();

            }
            foreach (var script in lifeLineScripts)
            {
                script.ChangeLineSpeed(_currentHealth);
            }

            if (_currentHealth <= 30f)
            {
                var audioManager = FindObjectOfType<OwnAudioManager>();

                if (!audioManager.SeeIfPlaying("HeartBeat"))
                {
                    audioManager.Play("HeartBeat");
                }

            }
            else
            {
                var audioManager = FindObjectOfType<OwnAudioManager>();
                if (audioManager.SeeIfPlaying("HeartBeat"))
                {
                    audioManager.Stop("HeartBeat");
                }
            }

            if (_currentHealth <= 0f)
            {
                FindObjectOfType<OwnAudioManager>().Stop("HeartBeat");
                Dead();
                PauseGame();

            }
            else if (lastHealth > _currentHealth) // Was Damaged
            {
                StartCoroutine(StartInvincible());
            }

        }

    }

    IEnumerator StartInvincible()
    {
        shieldGB.SetActive(true);
        yield return new WaitForSeconds(1f);
        shieldGB.SetActive(false);
        yield return new WaitForSeconds(0.75f);
        shieldGB.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        shieldGB.SetActive(false);
        yield return new WaitForSeconds(0.25f);
        shieldGB.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        shieldGB.SetActive(false);
        invincible = false;

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

        var highestScore = PlayerPrefs.GetInt("HighestScore");

        if (highestScore <= GameManager.GetScore())
        {
            highestScore = GameManager.GetScore();
            PlayerPrefs.SetInt("HighestScore", GameManager.GetScore());
        }

        highestScoreText.text = "Highest Score: " + highestScore;


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
        PlayerPrefs.SetInt("HasHeli", hasHeli);
        PlayerPrefs.SetInt("SavedGame", 1);

        var highestScore = PlayerPrefs.GetInt("HighestScore");

        if (highestScore <= GameManager.GetScore())
        {
            highestScore = GameManager.GetScore();
            PlayerPrefs.SetInt("HighestScore", GameManager.GetScore());
        }
    }
}
