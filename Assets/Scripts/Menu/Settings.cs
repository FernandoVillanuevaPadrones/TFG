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

    private void Update()
    {
        musicNumber.text = (int) musicSlider.value + "%";
        soundEffectNumber.text = (int) soundEffectSlider.value + "%";
    }
}
