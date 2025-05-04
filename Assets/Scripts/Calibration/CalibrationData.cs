using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Stores data from the CalibrationSettings struct in UserHandler.
// Acts as intermediate step between user modification and saving to database 
public class CalibrationData : MonoBehaviour {

    // Saved into Database
    public int minimumBlobSize = 8;     // range from 8 to 15
    public int maxBlobSize = 100;  // range from 10 to 100

    public int redBlobLifetime = 3;
    public int crossLifetime = 2;

    public int skipValue = 0;
    public bool showCrosses;

    public float colorThreshold = 25;   // minimum change in pixel colour
    public float colorThresholdSqr;

    public bool horizontalFlip = false;
    public bool verticalFlip = false;

    // App Local
    public float distThreshold = 3;    // for sizing of blobs

    public GameObject thresholdCrossPrefab;

    public static CalibrationData instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        // update calibration settings from database
        GetCalibrationDataFromDatabase();
    }
    
    /// <summary>
    ///  Update values with data from the database.
    ///  Can be used to retrieve current settings or to undo changes
    /// </summary>
    public void GetCalibrationDataFromDatabase()
    {
        // use setting in AppManager
        CalibrationSettings.CalibrationSetting settingMode = AppManager.instance.SettingMode;

        minimumBlobSize = int.Parse(UserHandler.instance.GetCalibrationSettings().GetMinBlobSize(settingMode));
        maxBlobSize = int.Parse(UserHandler.instance.GetCalibrationSettings().GetMaxBlobSize(settingMode));

        redBlobLifetime = int.Parse(UserHandler.instance.GetCalibrationSettings().GetRedBlobLifetime(settingMode));
        crossLifetime = int.Parse(UserHandler.instance.GetCalibrationSettings().GetCrossesLifetime(settingMode));

        skipValue = int.Parse(UserHandler.instance.GetCalibrationSettings().GetSkipValue(settingMode));
        int showCrossesInt = int.Parse(UserHandler.instance.GetCalibrationSettings().GetShowCrosses(settingMode));
        if (showCrossesInt == 0)
            showCrosses = false;
        else
            showCrosses = true;

        colorThreshold = int.Parse(UserHandler.instance.GetCalibrationSettings().GetColorThreshold(settingMode));
        colorThresholdSqr = colorThreshold * colorThreshold;

        int horzFlipInt = int.Parse(UserHandler.instance.GetCalibrationSettings().GetHorizontalFlip(settingMode));
        if (horzFlipInt == 0)   // false
            horizontalFlip = false;
        else
            horizontalFlip = true;

        int vertFlipInt = int.Parse(UserHandler.instance.GetCalibrationSettings().GetVerticalFlip(settingMode));
        if (vertFlipInt == 0)   // false
            verticalFlip = false;
        else
            verticalFlip = true;
    }
    
    /// <summary>
    ///  Set Calibration Data to default from database (does not overwrite currently saved settings)
    /// </summary>
    /*public void GetDefaultCalibrationDataFromDatabase()
    {
        minimumBlobSize = int.Parse(UserHandler.instance.GetCalibrationSettings().GetDefaultMinBlobSize());
        maxBlobSize = int.Parse(UserHandler.instance.GetCalibrationSettings().GetDefaultMaxBlobSize());

        redBlobLifetime = int.Parse(UserHandler.instance.GetCalibrationSettings().GetDefaultRedBlobLifetime());
        crossLifetime = int.Parse(UserHandler.instance.GetCalibrationSettings().GetDefaultCrossesLifetime());

        skipValue = int.Parse(UserHandler.instance.GetCalibrationSettings().GetDefaultSkipValue());
        showCrosses = bool.Parse(UserHandler.instance.GetCalibrationSettings().GetDefaultShowCrosses());

        colorThreshold = int.Parse(UserHandler.instance.GetCalibrationSettings().GetDefaultColorThreshold());

        int horzFlipInt = int.Parse(UserHandler.instance.GetCalibrationSettings().GetDefaultHorizontalFlip());
        if (horzFlipInt == 0)
            horizontalFlip = false;
        else
            horizontalFlip = true;

        int vertFlipInt = int.Parse(UserHandler.instance.GetCalibrationSettings().GetDefaultVerticalFlip());
        if (vertFlipInt == 0)
            verticalFlip = false;
        else
            verticalFlip = true;
    }*/
}