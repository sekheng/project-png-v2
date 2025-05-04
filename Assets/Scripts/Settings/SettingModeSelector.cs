using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingModeSelector : MonoBehaviour
{
    [Header("For Calibration Setting A")]
    [SerializeField]
    private Toggle toggleSettingA;
    [SerializeField]
    private Text settingTextLabelA;
    [SerializeField]
    private InputField labelInputFieldA;
    [Header("For Calibration Setting B")]
    [SerializeField]
    private Toggle toggleSettingB;
    [SerializeField]
    private Text settingTextLabelB;
    [SerializeField]
    private InputField labelInputFieldB;

    [SerializeField]
    private bool readOnly;
    [SerializeField]
    private bool editLabelMode = false;

    void Awake()
    {
        // Enable/Disable InputFields based on editLabelMode
        labelInputFieldA.gameObject.SetActive(editLabelMode);
        labelInputFieldB.gameObject.SetActive(editLabelMode);
    }

    /// <summary>
    /// For function callback
    /// </summary>
    public void SaveCalibrationSettingLabels()
    {
        PlayerPrefs.SetString("CalibrationSettingA_Label", labelInputFieldA.text);
        PlayerPrefs.SetString("CalibrationSettingB_Label", labelInputFieldB.text);
    }

    private void OnEnable()
    {
        // Set text labels (read from PlayerPrefs)
        settingTextLabelA.text = PlayerPrefs.GetString("CalibrationSettingA_Label", "Calibration A");
        settingTextLabelB.text = PlayerPrefs.GetString("CalibrationSettingB_Label", "Calibration B");

        // Initialise text strings in InputFields
        labelInputFieldA.text = settingTextLabelA.text;
        labelInputFieldB.text = settingTextLabelB.text;
    }

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        while (!WebcamHandler.instance.IsTextureLoaded())
            yield return null;
        yield return null;  // wait 1 extra frame
        
        // fetch currently selected mode from AppManager (global data)
        CalibrationSettings.CalibrationSetting settingMode = AppManager.instance.SettingMode;
        // toggle to the active setting
        SyncWithActiveSetting();

        // if readOnly mode, prevent interaction for active setting
        if (readOnly) {
            // disable interactivity for all toggles
            toggleSettingA.interactable = false;
            toggleSettingB.interactable = false;

            // find the toggle corresponding to the current setting
            Toggle activeSetting;
            if (settingMode == CalibrationSettings.CalibrationSetting.SETTING_A)
                activeSetting = toggleSettingA;
            else
                activeSetting = toggleSettingB;

            // for the active setting, set it to a bright colour instead of greyed out when uninteractable
            var colors = activeSetting.colors;
            colors.disabledColor = activeSetting.colors.normalColor;
            activeSetting.colors = colors;
        }
        else {  // allow editing
            // disable interactivity for all toggles
            toggleSettingA.interactable = true;
            toggleSettingB.interactable = true;   
        }
    }

    private void SyncWithActiveSetting()
    {
        // fetch currently selected mode from AppManager (global data)
        CalibrationSettings.CalibrationSetting settingMode = AppManager.instance.SettingMode;
        // toggle the active setting
        toggleSettingA.isOn = (settingMode == CalibrationSettings.CalibrationSetting.SETTING_A);
        toggleSettingB.isOn = (settingMode == CalibrationSettings.CalibrationSetting.SETTING_B);
    }

    /// <summary>
    /// Function callback for OnValueChanged of the children Toggle objects in this Toggle group.
    /// For the Toggle object which was toggled to, set its corresponding setting mode in AppManager to load the corresponding calibration settings.
    /// </summary>
    /// <param name="settingToggled">The integer value of the enum of the Toggle object's corresponding Setting Mode</param>
    public void ToggleSetting(int settingToggled)
    {
        CalibrationSettings.CalibrationSetting settingMode = (CalibrationSettings.CalibrationSetting)settingToggled;
        // find the toggle object that was toggled
        Toggle toggledSetting;
        if (settingMode == CalibrationSettings.CalibrationSetting.SETTING_A)
            toggledSetting = toggleSettingA;
        else
            toggledSetting = toggleSettingB;

        if (toggledSetting.isOn)    // toggled to this setting
            AppManager.instance.SetSettingMode(settingMode);
    }

}
