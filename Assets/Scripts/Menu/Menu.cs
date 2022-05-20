using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button continueButton;

    private void Start()
    {

        if (SceneManager.GetActiveScene().name == "FirstScene" && PlayerPrefs.GetInt("SavedGame") == 0)
        {
            Debug.Log("Entra: " + PlayerPrefs.GetInt("SavedGame"));
            continueButton.interactable = false;
            continueButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.gray;           
        }
    }

    public void StartGame()
    {
        PlayerPrefs.SetInt("SavedGame", 0);
        PlayerPrefs.SetInt("Level", 1);
        PlayerPrefs.SetInt("Score", 0);
        Time.timeScale = 1f;

        SceneManager.LoadScene("Generator");
    }

    public void ContinueGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Generator");
    }

    public void MainMenu()
    {
        PlayerPrefs.SetInt("SavedGame", 1);
        SceneManager.LoadScene("FirstScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void PlayAgain()
    {
        PlayerPrefs.SetInt("SavedGame", 0);
        PlayerPrefs.SetInt("Level", 1);
        SceneManager.LoadScene("Generator");
    }
}
