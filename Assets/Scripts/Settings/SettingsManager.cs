using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour {

    [SerializeField]
    private Dropdown screenResDropdown;

    private Resolution[] resolutions;

    // Use this for initialization
    void Start () {

        // Remove all options
        screenResDropdown.ClearOptions();

        // Create List of options
        List<string> resOptions = new List<string>();

        // Get current screen res
        int width = Screen.currentResolution.width;
        int height = Screen.currentResolution.height;
        int currResIdx = 0;

        // Get all possible screen resolutions
        resolutions = Screen.resolutions;
        for (int i = 0; i < resolutions.Length; ++i)
        {
            Resolution res = resolutions[i];

            // get res option
            string resOption = res.width + " x " + res.height;
            resOptions.Add(resOption);

            // find the current resolution
            if (res.width == width && res.height == height)
                currResIdx = i;
        }

        // Add options
        screenResDropdown.AddOptions(resOptions);

        screenResDropdown.value = currResIdx;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SaveSettings()
    {
        // Save Screen Resolution Settings
        SaveScreenResSettings();
    }

    private void SaveScreenResSettings()
    {
        Resolution selectedRes = resolutions[screenResDropdown.value];

        // Set resolution
        AppManager.instance.SetScreenResolution(selectedRes.width, selectedRes.height);
    }

}
