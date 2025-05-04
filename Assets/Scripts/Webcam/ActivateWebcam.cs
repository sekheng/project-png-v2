using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateWebcam : MonoBehaviour {

	// Use this for initialization
	void Start () {

        WebcamHandler.instance.ActivateWebcam();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
