using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Can abstract away the PlayerPrefs implementation of a data store to more easily use other data saving methods interchangably.
// Could be a MonoBehaviour class for more flexibility or non-static functionality, but I do not see a point for that now.
public static class TrapeziumsDataStore
{
    /// <summary>
    /// Set necessary initial configurations (no. rows, no. columns, webcam width & height) to data store.
    /// </summary>
    /// <param name="settingMode">Setting mode to set initial configuration</param>
    /// <param name="trapeziumsManager">TrapeziumsManager object containing trapeziums configuration</param>
    public static void SetTrapeziumConfiguration(CalibrationSettings.CalibrationSetting settingMode, TrapeziumsManager trapeziumsManager)
    {
        char settingModeChar = (settingMode == CalibrationSettings.CalibrationSetting.SETTING_A) ? 'A' : 'B';

        // set rows & columns and webcam width & height to PlayerPrefs
        PlayerPrefs.SetInt(string.Format("{0}_TrapeziumPoints_Rows", settingModeChar), trapeziumsManager.Rows);
        PlayerPrefs.SetInt(string.Format("{0}_TrapeziumPoints_Columns", settingModeChar), trapeziumsManager.Columns);
        PlayerPrefs.SetInt(string.Format("{0}_WebcamTexture_Width", settingModeChar), WebcamHandler.instance.GetWidth());
        PlayerPrefs.SetInt(string.Format("{0}_WebcamTexture_Height", settingModeChar), WebcamHandler.instance.GetHeight());
    }

    /// <summary>
    /// Get the point data for a trapezium point at coordinate (row, col) for a setting mode.
    /// </summary>
    /// <param name="settingMode">Setting mode</param>
    /// <param name="row">Trapezium point's row position</param>
    /// <param name="col">Trapezium point's column position</param>
    /// <returns>The point data (string)</returns>
    public static string GetPointData(CalibrationSettings.CalibrationSetting settingMode, int row, int col)
    {
        char settingModeChar = (settingMode == CalibrationSettings.CalibrationSetting.SETTING_A) ? 'A' : 'B';

        string key = string.Format("{0}_TrapeziumPoint_{1}{2}", settingModeChar, row, col);
        string pointData = PlayerPrefs.GetString(key, "");

        return pointData;
    }

    /// <summary>
    /// Set the point data for a trapezium point at coordinate (row, col) for a setting mode.
    /// </summary>
    /// <param name="settingMode">Setting mode</param>
    /// <param name="row">Trapezium point's row position</param>
    /// <param name="col">Trapezium point's column position</param>
    public static void SetPointData(CalibrationSettings.CalibrationSetting settingMode, int row, int col, string data)
    {
        char settingModeChar = (settingMode == CalibrationSettings.CalibrationSetting.SETTING_A) ? 'A' : 'B';

        string key = string.Format("{0}_TrapeziumPoint_{1}{2}", settingModeChar, row, col);
        PlayerPrefs.SetString(key, data);
    }

    /// <summary>
    /// Checks whether there are trapezium and data points saved in the data store before, and whether the data is valid.
    /// (Can load trapezium point positions if the saved rows and columns are equal, and webcam resolution same as previously loaded.)
    /// </summary>
    /// <param name="trapeziumsManager">TrapeziumsManager object containing trapeziums configuration</param>
    /// <returns>Whether saved trapezium data is valid</returns>
    public static bool TrapeziumDataIsValid(TrapeziumsManager trapeziumsManager)
    {
        char settingMode = (AppManager.instance.SettingMode == CalibrationSettings.CalibrationSetting.SETTING_A) ? 'A' : 'B';

        return PlayerPrefs.GetInt(string.Format("{0}_TrapeziumPoints_Rows", settingMode), -1) == trapeziumsManager.Rows &&
            PlayerPrefs.GetInt(string.Format("{0}_TrapeziumPoints_Columns", settingMode), -1) == trapeziumsManager.Columns &&
            PlayerPrefs.GetInt(string.Format("{0}_WebcamTexture_Width", settingMode), -1) == WebcamHandler.instance.GetWidth() &&
            PlayerPrefs.GetInt(string.Format("{0}_WebcamTexture_Height", settingMode), -1) == WebcamHandler.instance.GetHeight();
    }
}
