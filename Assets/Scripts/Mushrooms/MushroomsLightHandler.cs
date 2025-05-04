using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MushroomsLightHandler : MonoBehaviour
{
    public Slider mushroomLightScroller;

    public Volume postVolume;

    float baseBloomIntensity;

    void Start()
    {
        if (postVolume.profile.TryGet(out Bloom bloom))
        {
            baseBloomIntensity = bloom.intensity.value;
        }

        mushroomLightScroller.value = (PlayerPrefs.GetFloat("mushroomLight", 0)*5)+5;

        SetLight();
        SetBloomIntensity();
    }

    public void OnMushroomSceneLightSettingsChanged()
    {
        PlayerPrefs.SetFloat("mushroomLight", (mushroomLightScroller.value - 5)/5.0f);
        SetLight();
        SetBloomIntensity();
    }

    void SetLight()
    {
        float intensity = PlayerPrefs.GetFloat("mushroomLight", 0);

        if (postVolume.profile.TryGet(out ColorAdjustments colorAdj))
        {
            float factor = Mathf.Pow(2, intensity);
            Color color = new Color(1 * factor, 1 * factor, 1 * factor);
            colorAdj.colorFilter.Override(color);
        }
    }

    void SetBloomIntensity()
    {
        float intensity = PlayerPrefs.GetFloat("mushroomLight", 0) * 10;

        if (postVolume.profile.TryGet(out Bloom bloom))
        {
            bloom.intensity.Override(baseBloomIntensity - intensity);
        }
    }
}