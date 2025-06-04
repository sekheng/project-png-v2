using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableWebcam : MonoBehaviour
{
    [SerializeField, Tooltip("To disable the webcam entirely")]
    private bool disableWebcamForever = false;

    public static bool DISABLE_WEBCAM_FLAG { get; private set; }

    void Awake()
    {
        DISABLE_WEBCAM_FLAG = disableWebcamForever;
    }
}
