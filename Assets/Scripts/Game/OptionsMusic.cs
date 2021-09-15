using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMusic : MonoBehaviour
{
    public string nameOption;
    public Text percent;
    public Slider slider;

    public void SetSetting(float volume)
    {
        slider.value = volume;
        volume *= 100;
        if (!string.IsNullOrEmpty(nameOption))
        {
            percent.text = nameOption + ": " + volume.ToString("N0") + "%";
        }


    }
}