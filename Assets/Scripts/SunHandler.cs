using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunHandler : MonoBehaviour
{
    public GameObject dayOnly, dayNight;

    public bool isDay=true;

    public static SunHandler Instance;

    private void Start()
    {
        Instance = this;

        if (PrefsHandler.GetDayNightSettings(AnimalsSpawner.Instance.folderName))
        {
            dayOnly.SetActive(false);
            dayNight.SetActive(true);

            iTween.ValueTo(gameObject, iTween.Hash("from", new Vector3(90,0,0), "to", new Vector3(180, 0, 0),
            "time", PrefsHandler.GetDayDuration(AnimalsSpawner.Instance.folderName) * 30, "onupdate", "RotateObject", "oncomplete", "onDayEnd"));
        }
        else
        {
            dayOnly.SetActive(true);
            dayNight.SetActive(false);
        }
    }

    private void onDayEnd()
    {
        isDay = false;
        iTween.ValueTo(gameObject, iTween.Hash("from", new Vector3(180, 0, 0), "to", new Vector3(359, 0, 0),
            "time", PrefsHandler.GetNightDuration(AnimalsSpawner.Instance.folderName) * 60, "onupdate", "RotateObject", "oncomplete", "OnNightEnd"));
    }

    private void OnNightEnd()
    {
        isDay = true;
        iTween.ValueTo(gameObject, iTween.Hash("from", Vector3.zero, "to", new Vector3(180, 0, 0),
            "time", PrefsHandler.GetDayDuration(AnimalsSpawner.Instance.folderName) * 60, "onupdate", "RotateObject", "oncomplete", "onDayEnd"));
    }

    public void RotateObject(Vector3 rotation)
    {
        rotation.y = rotation.z = 0;
        dayNight.transform.localEulerAngles = rotation;
    }
}