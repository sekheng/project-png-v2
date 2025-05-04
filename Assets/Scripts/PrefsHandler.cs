using UnityEngine;

public static class PrefsHandler
{
    private const string butterflyIlluminationStrengthText = "butterflyIlluminationStrength";
    private const string dayDurationText = "dayDuration";
    private const string nightDurationText = "nightDuration";
    private const string dayNightSettingsText = "DayNightSetting";

    public static int butterflyIlluminationStrength 
    {
        get
        {
            return PlayerPrefs.GetInt(butterflyIlluminationStrengthText, 1);
        }
        set
        {
            PlayerPrefs.SetInt(butterflyIlluminationStrengthText, value);
        }
    }

    public static int GetDayDuration(string scene)
    {
        return PlayerPrefs.GetInt(scene + dayDurationText, 1);
    }
    public static void SetDayDuration(string scene,int value)
    {
        PlayerPrefs.SetInt(scene + dayDurationText, value);
    }

    public static int GetNightDuration(string scene)
    {
        return PlayerPrefs.GetInt(scene+nightDurationText, 1);
    }

    public static void SetNightDuration(string scene,int value)
    {
        PlayerPrefs.SetInt(scene + nightDurationText, value);
    }

    public static bool GetDayNightSettings(string scene)
    {
        return PlayerPrefs.GetInt(scene+dayNightSettingsText, 0)==1;
    }

    public static void SetDayNightSettings(string scene, bool value)
    {
        PlayerPrefs.SetInt(scene + dayNightSettingsText, value?1:0);
    }
}