using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PasswordForCalib : MonoBehaviour
{

    public Button Calibration;
    public InputField PassField, newPassField;
    public Button newPassButton;

    private void Start()
    {
        //PlayerPrefs.SetString("SavedCalibPass", "albert");
        if (!PlayerPrefs.HasKey("SavedCalibPass"))
        {
            PlayerPrefs.SetString("SavedCalibPass", "albert");
        }
    }

    public void SetCalibrationStatus()
    {
        if(PassField.text == PlayerPrefs.GetString("SavedCalibPass"))
        {
            Calibration.interactable = true;
        }
        else
        {
            Calibration.interactable = false;
        }
    }
    public void SetNewPassword()
    {
        if(PassField.text == PlayerPrefs.GetString("SavedCalibPass"))
        {
            PlayerPrefs.SetString("SavedCalibPass", newPassField.text);
        }
    }
}
