using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public string sceneName;
    public CameraCurve cameraCurve;
    public QRManager qrManager;
    public Text timerTxt;

    [Header("Gameplay Walk speed Settings")]
    public Slider walkSpeedSlider, cameraCurveSlider;

    public SchoolController school;

    private void Start()
    {
        float walkSpeed = PlayerPrefs.GetFloat($"{sceneName}:walkSpeed", 0);
        Time.timeScale = 1 + ((walkSpeed * walkSpeed) * 0.1f);
        if(walkSpeedSlider) walkSpeedSlider.value = walkSpeed;
        float camCurve = PlayerPrefs.GetFloat($"{sceneName}:CurveValue", 0);
        if (cameraCurve != null) cameraCurve.SetCameraCurve(camCurve);
        if(cameraCurveSlider) cameraCurveSlider.value = camCurve;
    }

    public void UpdateTime(string time)
    {
        timerTxt.text = time;
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }

    public void OpenMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void DeleteAllFiles()
    {
        AnimalsSpawner.Instance.DeleteAll();

        var files = Directory.GetFiles(Application.streamingAssetsPath+"/"+qrManager.folderName, "*.jpg");
        foreach (var file in files)
        {
            File.Delete(file);
        }

        qrManager.DeleteAllAnimals();
    }

    public void SpawnFish(GameObject fish)
    {
        //school.Spawn(fish, transform.position, fish.transform.rotation);
        school.gameObject.SetActive(true);
    }

    public void OnAnimalsSettingsClosed()
    {
        if (walkSpeedSlider)
        {
            PlayerPrefs.SetFloat($"{sceneName}:walkSpeed", walkSpeedSlider.value);
            Time.timeScale = 1 + ((walkSpeedSlider.value * walkSpeedSlider.value) * 0.1f);
            //AnimalsSpawner.Instance.SetAnimalsSpeed(Mathf.RoundToInt(walkSpeedSlider.value));
        }
        if (cameraCurveSlider)
        {
            // Update Camera Curve if it's assigned
            PlayerPrefs.SetFloat($"{sceneName}:CurveValue", cameraCurveSlider.value);
            if (cameraCurve != null) cameraCurve.SetCameraCurve(cameraCurveSlider.value);
        }
        OnToggleSettings();

        PlayerPrefs.Save();
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }

    bool isSettingsEnabled = false;

    public void OnToggleSettings()
    {
        isSettingsEnabled = !isSettingsEnabled;
        foreach (var item in FindObjectsOfType<AnimalInteractionHandler>())
        {
            item.PauseResumeTouch(isSettingsEnabled);
        }

        foreach (var item in FindObjectsOfType<BubbleHandler>())
        {
            item.OnPauseResumeTouch(isSettingsEnabled);
        }
    }
}