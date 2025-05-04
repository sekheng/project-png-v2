using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
using System;

public class MenuManager : MonoBehaviour
{
    [Header("Settings")]
    public GameObject settingsPopup;
    public Slider illuminationSlider;

    [Header("Loading")]
    public GameObject loadingScreen;

    [Header("Togglers")]
    public GameObject toggleObject;
    public string[] scenesList;

    [Header("Mushroom Scene")]
    public Toggle mushroomModeToggle;

    private List<GameObject> togglers;

    static List<string> m_HistoryList = new List<string>();

    public Toggle fullScreenToggle, screenLockToggle;

    private void Awake()
    {
        Screen.fullScreen = fullScreenToggle.isOn = PlayerPrefs.GetInt("IsFullScreen", 0) == 1;
        var isLock = PlayerPrefs.GetInt("IsScreenLock", 0) == 1;

        screenLockToggle.isOn = isLock;
        SetResizable(isLock);
    }

    public void OnToggleFullscreen(bool val)
    {
        var isFullScreen = fullScreenToggle.isOn;
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt("IsFullScreen", isFullScreen?1:0);
    }

    public void OnToggleScreenResolution(bool val)
    {
        var isLock = screenLockToggle.isOn;
        PlayerPrefs.SetInt("IsScreenLock", isLock?1:0);
        SetResizable(isLock);
    }

#if UNITY_STANDALONE_WIN
    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    private const int GWL_STYLE = -16;
    private const int WS_SIZEBOX = 0x00040000; // Resizable border
    private const int WS_MAXIMIZEBOX = 0x00010000; // Maximize button

    public void SetResizable(bool resizable)
    {
        IntPtr windowHandle = GetActiveWindow();
        int style = GetWindowLong(windowHandle, GWL_STYLE);

        if (resizable)
        {
            style |= WS_SIZEBOX | WS_MAXIMIZEBOX;
        }
        else
        {
            style &= ~(WS_SIZEBOX | WS_MAXIMIZEBOX);
        }

        SetWindowLong(windowHandle, GWL_STYLE, style);
    }
#endif

    private void Start()
    {
        if (illuminationSlider) illuminationSlider.value = PrefsHandler.butterflyIlluminationStrength;
        
        if (toggleObject)
        {
            togglers = new List<GameObject>();
            toggleObject.GetComponent<MenuSceneDayNightToggleHandler>().Setup(scenesList[0]);
            togglers.Add(toggleObject);
            for (int i = 1; i < scenesList.Length; i++)
            {
                var obj = Instantiate(toggleObject, toggleObject.transform.parent) as GameObject;
                obj.GetComponent<MenuSceneDayNightToggleHandler>().Setup(scenesList[i]);
                togglers.Add(obj);
            }
        }

        mushroomModeToggle.isOn = PlayerPrefs.GetInt("IsMushroomModeDark", 1) == 1;
    }

    public void MushroomSceneToggleTap(bool val)
    {
        PlayerPrefs.SetInt("IsMushroomModeDark", mushroomModeToggle.isOn ? 1 : 0);
    }

    public void OpenSettings()
    {
        settingsPopup.SetActive(true);
    }

    public void LoadScene(string sceneName)
    {
        m_HistoryList.Add(SceneManager.GetActiveScene().name);
        loadingScreen.SetActive(true);
        PrefsHandler.butterflyIlluminationStrength = Mathf.RoundToInt(illuminationSlider.value);
        SaveDayNightToggleData();
        SceneManager.LoadScene(sceneName);
    }

    private void SaveDayNightToggleData()
    {
        foreach (var item in togglers)
        {
            item.GetComponent<MenuSceneDayNightToggleHandler>().SaveData();
        }
    }

    public void Back()
    {
        // go back to the previous scene
        if (m_HistoryList.Count > 0)
        {
            string sceneName = m_HistoryList[m_HistoryList.Count - 1];
            m_HistoryList.RemoveAt(m_HistoryList.Count - 1);
            SceneManager.LoadScene(sceneName);
        }
    }
}