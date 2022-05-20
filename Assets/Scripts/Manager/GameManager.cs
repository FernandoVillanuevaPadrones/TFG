using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Player _playerScript;
    [SerializeField] private Weapon _weaponScript;

    [Header("Materials")]
    [SerializeField] private Material _velociMaterial;
    public static Material velociMaterial;
    [SerializeField] private static float showSpeed = 0.005f;

    public static bool canVelociBehaviour = true;
    public static bool startVelociBehaviour = false;



    [Header("Generator")]
    [SerializeField] private MazeGenerator generator;

    public static int currentLevel = 1;

    private static int roomScore = 5;
    private static int enemyScore = 10;
    private static int bossScore = 20;

    private static int totalScore = 0;

    public static bool roomsPlaced = false;


    public static bool savedGame = false;
    public static bool roomCleared = true;

    public static bool closeDoors = false;
    public static bool openDoors = false;

    public static float musicLevel = 1f;
    public static float soundEffectLevel = 1f;

    private void Awake()
    {
        if (PlayerPrefs.GetFloat("MusicLevel") == 0f)
        {
            PlayerPrefs.SetFloat("MusicLevel", musicLevel);
            PlayerPrefs.SetFloat("EffectLevel", soundEffectLevel);
        }
        else
        {
            musicLevel = PlayerPrefs.GetFloat("MusicLevel");
            soundEffectLevel = PlayerPrefs.GetFloat("EffectLevel");
        }
    }



    private void Start()
    {
        closeDoors = false;
        openDoors = false;

        velociMaterial = _velociMaterial;
        totalScore = PlayerPrefs.GetInt("Score");
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "FirstScene")
        {
            if (closeDoors)
            {
                generator.CloseAllDoors();
                closeDoors = false;
            }
            else if (openDoors)
            {
                generator.OpenAllDoors();
                openDoors = false;
            }

            if (canVelociBehaviour && startVelociBehaviour)
            {
                Debug.Log("HOLASA");
                StartCoroutine(ShowVelocirraptors());
                StartCoroutine(StartVelociBehaviour());
                startVelociBehaviour = false;
            }
            else if(!canVelociBehaviour)
            {
                StopAllCoroutines();
                canVelociBehaviour = true;
                StartCoroutine(HideVelocirraptos());
            }

        }
        
    }

    
    public IEnumerator StartVelociBehaviour()
    {
        while (canVelociBehaviour)
        {
            yield return new WaitForSeconds(Random.Range(7, 10));
            StartCoroutine(HideVelocirraptos());
            yield return new WaitForSeconds(Random.Range(7, 10));
            StartCoroutine(ShowVelocirraptors());
        }

    }



    public static IEnumerator HideVelocirraptos()
    {
        //velociMaterial.SetFloat("DissolveProgressFloat", 1f);

        var currentFloat = velociMaterial.GetFloat("DissolveProgressFloat");
        while (currentFloat <= 1f)
        {
            currentFloat = velociMaterial.GetFloat("DissolveProgressFloat");
            velociMaterial.SetFloat("DissolveProgressFloat", currentFloat + showSpeed);
            yield return new WaitForSeconds(0f);
        }

        yield return null;
    }

    public static IEnumerator ShowVelocirraptors()
    {
        var currentFloat = velociMaterial.GetFloat("DissolveProgressFloat");
        while (currentFloat >= 0.01f)
        {
            currentFloat = velociMaterial.GetFloat("DissolveProgressFloat");
            velociMaterial.SetFloat("DissolveProgressFloat", currentFloat - showSpeed);
            yield return new WaitForSeconds(0f);
        }
        yield return null;
    }

    public static void ChangeScore(string type)
    {
        switch (type)
        {
            case "room":
                totalScore += roomScore;
                break;

            case "enemy":
                totalScore += enemyScore;
                break;

            case "boss":
                totalScore += bossScore;
                break;

        }
    }

    public static int GetScore()
    {
        return totalScore;
    }

    public void NextLevel()
    {
        _playerScript.SaveStats();
        _weaponScript.SaveStats();
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
        SceneManager.LoadScene("Generator");
    }

    public void MusicLevelChanged(Slider musicSlider)
    {
        musicLevel = musicSlider.value;
        PlayerPrefs.SetFloat("MusicLevel", musicLevel); 
    }

    public void SoundEffectLevelChanged(Slider effectSlider)
    {
        soundEffectLevel = effectSlider.value;
        PlayerPrefs.SetFloat("EffectLevel", soundEffectLevel);

    }


}
