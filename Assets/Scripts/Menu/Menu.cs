using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button continueButton;
    [SerializeField]
    private Animator faderAnimator;
    private void Start()
    {
        faderAnimator.SetBool("FadeOut", true);
        if (SceneManager.GetActiveScene().name == "FirstScene" && PlayerPrefs.GetInt("SavedGame") == 0)
        {
            Debug.Log("Entra: " + PlayerPrefs.GetInt("SavedGame"));
            continueButton.interactable = false;
            continueButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.gray;           
        }
    }

    public void StartGame()
    {
        faderAnimator.SetBool("FadeIn", true);

        ResetAll();
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
        faderAnimator.SetBool("FadeIn", true);
        PlayerPrefs.SetInt("SavedGame", 1);
        SceneManager.LoadScene("FirstScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void PlayAgain()
    {
        faderAnimator.SetBool("FadeIn", true);
        ResetAll();
        SceneManager.LoadScene("Generator");
    }

    private void ResetAll()
    {
        PlayerPrefs.SetInt("SavedGame", 0);
        PlayerPrefs.SetInt("Level", 1);
        PlayerPrefs.SetInt("Score", 0);
        PlayerPrefs.SetInt("HasHeli", 0);
        PlayerPrefs.SetInt("ShowMapUpgrade", 0);
    }
}
