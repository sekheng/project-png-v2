using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MushroomsModeHandler : MonoBehaviour
{
    public Volume post;

    public VolumeProfile lightProfile;

    public Material lightSkybox;

    void Start()
    {
        if(PlayerPrefs.GetInt("IsMushroomModeDark", 1) == 0)
        {
            // Is Light Mode
            RenderSettings.fog = false;
            RenderSettings.skybox = lightSkybox;
            post.profile = lightProfile;
        }
    }
}