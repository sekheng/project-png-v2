using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadBGMFromStream : MonoBehaviour
{
    public AudioSource audioSource;

    public string bgmName;

    IEnumerator Start()
    {
        string url = Application.streamingAssetsPath + "/BGM/" + bgmName+".mp3";
        WWW www = new WWW(url);
        yield return www;

        audioSource.clip = www.GetAudioClip(false,false);
        audioSource.Play();
    }
}