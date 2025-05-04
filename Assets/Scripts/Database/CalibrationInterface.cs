using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CalibrationInterface : MonoBehaviour {

    private CalibrationSettings calibrationSettings;    // current settings

    [Header("Calibration Setting Handlers")]
    [SerializeField]
    private Slider thresholdSlider;
    [SerializeField]
    private Slider minBlobsSlider;
    [SerializeField]
    private Slider maxBlobsSlider;

    [SerializeField]
    private InputField redBlobLifetimeField;
    [SerializeField]
    private InputField crossesLifetimeField;
    [SerializeField]
    private InputField skipValueField;

    [SerializeField]
    private Toggle showCrossesToggle;

    [SerializeField]
    private Dropdown horizontalFlipDropdown;
    [SerializeField]
    private Dropdown verticalFlipDropdown;

    [Header("Other GUI Elements")]
    [SerializeField]
    private Text debugText;
    [SerializeField]
    private Text errorLogText;

    // Use this for initialization
    void Start () {

        //calibrationSettings = UserHandler.instance.GetCalibrationSettings();

        // Set calibration setting values
        LoadCalibrationSettings(AppManager.instance.SettingMode);

        // update debug text
        StartCoroutine(UpdateDebugText());
    }

    private IEnumerator UpdateDebugText()
    {
        while (WebcamHandler.instance.GetWidth() == 0 || WebcamHandler.instance.GetHeight() == 0)
            yield return new WaitForSeconds(0.1f);

        debugText.text = "Width: " + WebcamHandler.instance.GetWidth() + "\nHeight: " + WebcamHandler.instance.GetHeight();
    }

    public void SaveSettings()
    {
        string showCrossesValue = showCrossesToggle.isOn ? "1" : "0";

        bool errorCaught = false;

        // Update Calibration Settings Table
        try
        {
            User.Utility.UpdateCalibrationSettings(AppManager.instance.SettingMode, thresholdSlider.value.ToString(), minBlobsSlider.value.ToString(), maxBlobsSlider.value.ToString(),
                redBlobLifetimeField.text, crossesLifetimeField.text, skipValueField.text, showCrossesValue,
                horizontalFlipDropdown.value.ToString(), verticalFlipDropdown.value.ToString());
        }
        catch (Exception e)
        {
            errorLogText.text = e.Message;
            errorCaught = true;
        }

        if (!errorCaught)
        {
            Debug.Log("Calibration Settings Saved");
            errorLogText.text = "Calibration Settings Saved Successfully";
            UserHandler.instance.UpdateCalibrationSettings();
        }
    }

    // Doing this resets settings if not changed; else change will be fetched from the database
    private void OnDestroy()
    {
        CalibrationData.instance.GetCalibrationDataFromDatabase();
    }

    public void LoadDefaultSettings()
    {
        LoadCalibrationSettings(CalibrationSettings.CalibrationSetting.SETTING_DEFAULT);
    }

    public void LoadCalibrationSettings(CalibrationSettings.CalibrationSetting setting)
    {
        // Fetch latest copy from database
        calibrationSettings = UserHandler.instance.GetCalibrationSettings();

        // Set calibration setting values to the UI
        thresholdSlider.value = int.Parse(calibrationSettings.GetColorThreshold(setting));
        minBlobsSlider.value = int.Parse(calibrationSettings.GetMinBlobSize(setting));
        maxBlobsSlider.value = int.Parse(calibrationSettings.GetMaxBlobSize(setting));

        redBlobLifetimeField.text = calibrationSettings.GetRedBlobLifetime(setting);
        crossesLifetimeField.text = calibrationSettings.GetCrossesLifetime(setting);
        skipValueField.text = calibrationSettings.GetSkipValue(setting);

        int showCrosses = int.Parse(calibrationSettings.GetShowCrosses(setting));
        if (showCrosses == 1)
            showCrossesToggle.isOn = true;
        else
            showCrossesToggle.isOn = false;

        horizontalFlipDropdown.value = int.Parse(calibrationSettings.GetHorizontalFlip(setting));   // 0: Normal, 1: Flip
        verticalFlipDropdown.value = int.Parse(calibrationSettings.GetVerticalFlip(setting));   // 0: Normal, 1: Flip

    }
}
