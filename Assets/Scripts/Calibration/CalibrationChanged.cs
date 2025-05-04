using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalibrationChanged : MonoBehaviour {

    // for sliders with input for testing
    public InputField inputField;

    private void Start()
    {
        //GetComponent<Slider>().value = CalibrationData.instance.distThreshold;
    }

    public void OnThresholdValueChanged(Slider slider)
    {
        CalibrationData.instance.colorThreshold = slider.value;
        CalibrationData.instance.colorThresholdSqr = CalibrationData.instance.colorThreshold * CalibrationData.instance.colorThreshold;

        // update input field (temp!)
        //inputField.text = slider.value.ToString();
    }

    public void ThresholdValueChangeFromInput()
    {
        //CalibrationData.instance.colorThreshold = float.Parse(inputField.text);
        //GetComponent<Slider>().value = CalibrationData.instance.colorThreshold;
    }

    public void OnToggleShowCrosses(Toggle toggle)
    {
        CalibrationData.instance.showCrosses = toggle.isOn;
    }

    public void OnMinBlobsValueChanged(Slider slider)
    {
        CalibrationData.instance.minimumBlobSize = (int)slider.value;
    }

    public void OnMaxBlobsValueChanged(Slider slider)
    {
        CalibrationData.instance.maxBlobSize = (int)slider.value;
    }

    public void OnRedBlobsDelayValueChanged(InputField inputField)
    {
        CalibrationData.instance.redBlobLifetime = int.Parse(inputField.text);
    }

    public void OnCrossesDelayValueChanged(InputField inputField)
    {
        CalibrationData.instance.crossLifetime = int.Parse(inputField.text);
    }

    public void OnSkipValueChanged(InputField inputField)
    {
        CalibrationData.instance.skipValue = int.Parse(inputField.text);
    }

    public void SetHorizontalFlip(Dropdown dropdown)
    {
        if (dropdown.value == 1)
            CalibrationData.instance.horizontalFlip = true;
        else
            CalibrationData.instance.horizontalFlip = false;
    }

    public void SetVerticalFlip(Dropdown dropdown)
    {
        if (dropdown.value == 1)
            CalibrationData.instance.verticalFlip = true;
        else
            CalibrationData.instance.verticalFlip = false;
    }

}