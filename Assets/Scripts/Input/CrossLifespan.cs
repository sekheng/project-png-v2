using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Lifespan by number of frames
public class CrossLifespan : MonoBehaviour {

    private Point crossReference;
    public int lifespan = 1;

    [SerializeField]
    [Tooltip("Buffer time before starting to fade out")]
    private float bufferTime = 0f;
    [SerializeField]
    private float fadeSpeed = 1f;

	// Use this for initialization
	void Start () {
        lifespan = CalibrationData.instance.crossLifetime;

        // for fade out
        StartCoroutine(FadeOut());
	}
	
	// Update is called once per frame
	void Update () {

        // update by lifespan
        //UpdateLifespan();
    }

    private void UpdateLifespan()
    {
        --lifespan;

        if (lifespan <= 0)
            Destroy(gameObject);
    }

    private IEnumerator FadeOut()
    {
        // buffer time before starting fade
        float timer = 0f;
        while (timer < bufferTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        // get image component
        Image image = GetComponent<Image>();
        Color color = image.color;
        
        // fade out
        while (color.a > 0f)
        {
            color.a -= fadeSpeed * Time.deltaTime;
            if (color.a > 0f)
            {
                image.color = color;
                yield return null;
            }
        }

        // end of lifespan; destroy cross
        Destroy(gameObject);
    }

}
