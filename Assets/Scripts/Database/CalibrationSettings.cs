using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CalibrationSettings {

    public DataTable calibrationTable;  // Reference to Settings table from database

    public enum CalibrationSetting
    {
        SETTING_DEFAULT = 0,    // default setting configuration
        SETTING_A = 1,  // configuration for Setting A
        SETTING_B = 2,  // configuration for Setting B
    }
    
    // name of each column
    private string color_threshold;
    private string min_blob_size;
    private string max_blob_size;
    private string red_blob_lifetime;
    private string cross_lifetime;
    private string skip_value;
    private string show_crosses;
    private string horizontal_flip;
    private string vertical_flip;

    public CalibrationSettings(DataTable table, string[] columns)
    {
        calibrationTable = table;

        color_threshold = columns[0];
        min_blob_size = columns[1];
        max_blob_size = columns[2];
        red_blob_lifetime = columns[3];
        cross_lifetime = columns[4];
        skip_value = columns[5];
        show_crosses = columns[6];
        horizontal_flip = columns[7];
        vertical_flip = columns[8];
    }

    // For Current Settings
    public string GetColorThreshold(CalibrationSetting setting)
    {
        int idx = (int)setting;
        return calibrationTable[idx][color_threshold].ToString();
    }

    public string GetMinBlobSize(CalibrationSetting setting)
    {
        int idx = (int)setting;
        return calibrationTable[idx][min_blob_size].ToString();
    }

    public string GetMaxBlobSize(CalibrationSetting setting)
    {
        int idx = (int)setting;
        return calibrationTable[idx][max_blob_size].ToString();
    }

    public string GetRedBlobLifetime(CalibrationSetting setting)
    {
        int idx = (int)setting;
        return calibrationTable[idx][red_blob_lifetime].ToString();
    }

    public string GetCrossesLifetime(CalibrationSetting setting)
    {
        int idx = (int)setting;
        return calibrationTable[idx][cross_lifetime].ToString();
    }

    public string GetSkipValue(CalibrationSetting setting)
    {
        int idx = (int)setting;
        return calibrationTable[idx][skip_value].ToString();
    }

    public string GetShowCrosses(CalibrationSetting setting)
    {
        int idx = (int)setting;
        return calibrationTable[idx][show_crosses].ToString();
    }

    public string GetHorizontalFlip(CalibrationSetting setting)
    {
        int idx = (int)setting;
        return calibrationTable[idx][horizontal_flip].ToString();
    }

    public string GetVerticalFlip(CalibrationSetting setting)
    {
        int idx = (int)setting;
        return calibrationTable[idx][vertical_flip].ToString();
    }

}
