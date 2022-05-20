using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI musicNumber;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private TextMeshProUGUI soundEffectNumber;
    [SerializeField] private Slider soundEffectSlider;

    private void Start()
    {
        musicSlider.value = GameManager.musicLevel;
        soundEffectSlider.value = GameManager.soundEffectLevel;

    }

    private void Update()
    {
        musicNumber.text = (int) (musicSlider.value * 100f) + "%";
        soundEffectNumber.text = (int) (soundEffectSlider.value * 100f) + "%";
    }
}
