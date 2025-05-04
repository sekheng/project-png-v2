using UnityEngine;

public class AppManager : MonoBehaviour {

    public static AppManager instance = null;

    [SerializeField] private int targetWidth;
    [SerializeField] private int targetHeight;

    // reference to Trapeziums Manager
    private TrapeziumsManager trapeziumsManager = null;

    /*
     * Application specific parameters
     */
    private CalibrationSettings.CalibrationSetting settingMode = CalibrationSettings.CalibrationSetting.SETTING_A;  // default
    public CalibrationSettings.CalibrationSetting SettingMode {
        get { return settingMode; }
    }

    /*
     * Screen
     */
    public bool fullscreen = false;

    void Awake() {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
            return;
        }
        
        // Set default as fullscreen
        fullscreen = true;
        LoadFullscreen();

        // load setting mode from PlayerPrefs
        int savedSettingMode = PlayerPrefs.GetInt("SettingMode", (int)CalibrationSettings.CalibrationSetting.SETTING_A);
        settingMode = (CalibrationSettings.CalibrationSetting)savedSettingMode;
    }

    /// <summary>
    /// Check if there is a saved resolution. Else, set to target width and height.
    /// </summary>
    /// <returns>Whether to set to target width and height or not</returns>
    private bool SetToTarget()
    {
        int savedW = PlayerPrefs.GetInt("ScreenWidth", -1);
        int savedH = PlayerPrefs.GetInt("ScreenHeight", -1);
        return (savedW == -1 || savedH == -1);  // set to target width and height if no previous resolution was saved
    }

    /// <summary>
    ///  Get actual screen width. Default is Screen.currentResolution.width
    /// </summary>
    public int GetScreenWidth()
    {
        //return PlayerPrefs.GetInt("ScreenWidth", Screen.currentResolution.width);
        return Screen.width;
    }

    /// <summary>
    ///  Get actual screen height. Default is Screen.currentResolution.height
    /// </summary>
    public int GetScreenHeight()
    {
        //return PlayerPrefs.GetInt("ScreenHeight", Screen.currentResolution.height);
        return Screen.height;
    }

    /// <summary>
    ///  Set new screen resolution to PlayerPrefs
    /// </summary>
    public void SetScreenResolution(int width, int height)
    {
        PlayerPrefs.SetInt("ScreenWidth", width);
        PlayerPrefs.SetInt("ScreenHeight", height);

        if (fullscreen)
            SetFullscreen(width, height);
        else
            SetWindowed(width, height);
    }
	
	// Update is called once per frame
	void Update () {

        // Toggle fullscren
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // toggle fullscreen
            fullscreen = !fullscreen;
            // update Screen to toggled fullscreen mode
            Screen.fullScreen = fullscreen;

            // set to current width and height, or target width and height if no previous resolution was saved
            if (fullscreen)
                LoadFullscreen();
            else
                LoadWindowed();
        }
	}

    private void LoadFullscreen()
    {
        if (SetToTarget())
            SetFullscreen(targetWidth, targetHeight);
        else
            SetFullscreen(GetScreenWidth(), GetScreenHeight());
    }

    private void LoadWindowed()
    {
        if (SetToTarget())
            SetWindowed(targetWidth, targetHeight);
        else
            SetWindowed(GetScreenWidth(), GetScreenHeight());
    }

    private void SetFullscreen(int width, int height)
    {
        Screen.SetResolution(width, height, true);
    }

    private void SetWindowed(int width, int height)
    {
        Screen.SetResolution(width, height, false);
    }

    public void SetSettingMode(CalibrationSettings.CalibrationSetting newSetting)
    {
        if (newSetting != settingMode)  // setting mode has changed
        {
            // update settingMode variable
            settingMode = newSetting;

            // load new calibration setting variables to WebcamHandler
            CalibrationData.instance.GetCalibrationDataFromDatabase();
            // load trapezium setting variables to TrapeziumsManager
            if (trapeziumsManager == null) trapeziumsManager = FindObjectOfType<TrapeziumsManager>();
            if (trapeziumsManager != null) trapeziumsManager.LoadTrapeziumsData();

            // update Trapezium UI, if any
            Trapeziums_UI trapeziumsUI = FindObjectOfType<Trapeziums_UI>();
            if (trapeziumsUI != null) trapeziumsUI.RefreshTrapeziumsUI();
            // update CalibrationInterface UI, if any
            CalibrationInterface calibrationInterface = FindObjectOfType<CalibrationInterface>();
            if (calibrationInterface != null)
            {
                CalibrationData.instance.GetCalibrationDataFromDatabase();
                calibrationInterface.LoadCalibrationSettings(settingMode);
            }
            // BUG: isn't fetching latest version


            // save to PlayerPrefs
            PlayerPrefs.SetInt("SettingMode", (int)settingMode);
            PlayerPrefs.Save(); // supposedly not really needed; automatically called on application quit
        }
    }
}
