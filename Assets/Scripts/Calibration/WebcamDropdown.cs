using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WebcamDropdown : MonoBehaviour
{
    public const string CONST_WEBCAMNAME = "webcam_name";
    [Tooltip("Dropdown for webcam name"), SerializeField]
    TMP_Dropdown webcamDropdown;

    private void Start()
    {
        string webcamName = PlayerPrefs.GetString(CONST_WEBCAMNAME);
        if (!webcamDropdown)
        {
            webcamDropdown = GetComponent<TMP_Dropdown>();
        }
        // populate dropdown with available webcams
        int index = 0;
        bool foundName = false;
        List<string> options = new List<string>();
        foreach (WebCamDevice device in WebCamTexture.devices)
        {
            options.Add(device.name);
            if (device.name.Equals(webcamName))
            {
                foundName = true;
            }
            else if (!foundName)
            {
                index++;
            }
        }
        webcamDropdown.AddOptions(options);
        if (foundName)
        {
            webcamDropdown.value = index;
        }
    }

    public void SetWebcam()
    {
        PlayerPrefs.SetString(CONST_WEBCAMNAME, webcamDropdown.options[webcamDropdown.value].text);
        // set the selected webcam
        WebcamHandler.instance.ChooseWebcam(webcamDropdown.options[webcamDropdown.value].text);
    }
}
