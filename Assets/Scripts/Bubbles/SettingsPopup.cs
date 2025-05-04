using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPopup : MonoBehaviour
{
    public InputField speedInput;

    void Start()
    {
        speedInput.text = PlayerPrefs.GetFloat("BubblesTouchSpeed", 4).ToString();
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat("BubblesTouchSpeed", float.Parse(speedInput.text));

        AnimalsSpawner.Instance.SetAnimalsSpeed(float.Parse(speedInput.text));
    }
}