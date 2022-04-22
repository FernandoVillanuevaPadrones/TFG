using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Player _playerScript;
    [SerializeField] private Weapon _weaponScript;

    [Header("Materials")]
    [SerializeField] private Material _velociMaterial;
    public static Material velociMaterial;

    public static int currentLevel = 1;

    private static int roomScore = 5;
    private static int enemyScore = 10;
    private static int bossScore = 20;

    private static int totalScore = 0;


    public static bool savedGame = false;



    private void Start()
    {
        velociMaterial = _velociMaterial;
        totalScore = PlayerPrefs.GetInt("Score");
    }
    public static void HideVelociraptos()
    {
        velociMaterial.SetFloat("DissolveProgressFloat", 1f);
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

}
