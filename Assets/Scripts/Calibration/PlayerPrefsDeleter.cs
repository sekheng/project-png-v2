using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Add this script's callback if you want to reset PlayerPrefs related to Calibration.
/// </summary>
public class PlayerPrefsDeleter : MonoBehaviour
{
    // Start is called before the first frame update
    public void DeletePrefsAndExit()
    {
        // Delete PlayerPrefs
        PlayerPrefs.DeleteKey("SettingMode");   // last selected setting mode
        PlayerPrefs.DeleteKey("A_TrapeziumPoints_Rows");    // A rows
        PlayerPrefs.DeleteKey("B_TrapeziumPoints_Rows");    // B rows
        PlayerPrefs.DeleteKey("A_TrapeziumPoints_Columns"); // A columns
        PlayerPrefs.DeleteKey("B_TrapeziumPoints_Columns"); // B columns
        PlayerPrefs.DeleteKey("A_WebcamTexture_Width"); // A width
        PlayerPrefs.DeleteKey("B_WebcamTexture_Width"); // B width
        PlayerPrefs.DeleteKey("A_WebcamTexture_Height");    // A height
        PlayerPrefs.DeleteKey("B_WebcamTexture_Height");    // B height

        // Force Close Application
        Application.Quit();
    }
}
