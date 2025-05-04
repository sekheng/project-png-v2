using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSceneDayNightToggleHandler : MonoBehaviour
{
    public Text titleTxt;

    public Toggle dayToggle, dayNightToggle;

    public Slider daySlider, nightSlider;

    private string title;

    public void Setup(string title)
    {
        this.title = title;
        gameObject.name = this.title + " toggle";

        titleTxt.text = this.title;
        SetData();
    }

    private void SetData()
    {
        dayToggle.isOn = !PrefsHandler.GetDayNightSettings(title);
        dayNightToggle.isOn = PrefsHandler.GetDayNightSettings(title);

        daySlider.value = PrefsHandler.GetDayDuration(title);
        nightSlider.value = PrefsHandler.GetNightDuration(title);
    }

    public void ToggleClicked(bool isDayNight)
    {
        PrefsHandler.SetDayNightSettings(title,isDayNight);
    }

    public void SaveData()
    {
        PrefsHandler.SetDayDuration(title, Mathf.RoundToInt(daySlider.value));
        PrefsHandler.SetNightDuration(title, Mathf.RoundToInt(nightSlider.value));
    }
}