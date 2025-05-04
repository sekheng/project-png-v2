using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource source;
    public AudioClip buttonClick,dayClip, nightClip;

    private void Start()
    {
        if (Instance != null) 
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound(string clip)
    {
        switch (clip)
        {
            case "Click":
                {
                    source.PlayOneShot(buttonClick);
                    break;
                }
            case "Day":
                {
                    source.PlayOneShot(dayClip);
                    break;
                }
            case "Night":
                {
                    source.PlayOneShot(nightClip);
                    break;
                }
            default:
                break;
        }
    }
}