﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateWebcam : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (DisableWebcam.DISABLE_WEBCAM_FLAG)
        {
            Destroy(gameObject);
            return;
        }
        WebcamHandler.instance.ActivateWebcam();
	}
}
