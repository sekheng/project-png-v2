using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIValueText : MonoBehaviour {
    
    [SerializeField]
    [Tooltip("Slider to retrieve value from")]
    private Slider slider;

    [SerializeField]
    [Tooltip("Text to update")]
    private Text text;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateValue()
    {
        text.text = (slider.value).ToString();
    }

}
