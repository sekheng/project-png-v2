using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebcamTextureImage : MonoBehaviour {

    public static WebcamTextureImage instance = null;

    public RawImage rawImage;

    public Transform thresholdCrosses;

    //private bool horizontalFlip = false;
    //private bool verticalFlip = false;

    private Vector3 defaultPosition;

    private float scaleFactorX = 0f; // how much different this image's size is from the camera texture (e.g. 160x120)
    private float scaleFactorY = 0f; // how much different this image's size is from the camera texture (e.g. 160x120)
    public float GetScaleFactorX() { return scaleFactorX; }
    public float GetScaleFactorY() { return scaleFactorY; }

    // half-width and half-height (easier to work with)
    private float halfWidth = 0f;
    private float halfHeight = 0f;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    // Use this for initialization
    private IEnumerator Start () {

        //rawImage.uvRect = new Rect(0f, 0f, 1f / webcamTexture.width, 1f / webcamTexture.height);

        // set default position
        defaultPosition = transform.position;

        // wait for webcam to load
        while (!WebcamHandler.instance.IsTextureLoaded())
        {
            yield return 0;
        }

        // set webcam texture onto raw image
        rawImage.texture = WebcamHandler.instance.GetWebcamTexture();

        // calculate scale factor
        scaleFactorX = rawImage.GetComponent<RectTransform>().sizeDelta.x / WebcamHandler.instance.GetWidth();
        scaleFactorY = rawImage.GetComponent<RectTransform>().sizeDelta.y / WebcamHandler.instance.GetHeight();

        // calculate half-width and half-height
        halfWidth = 0.5f * WebcamHandler.instance.GetWidth();
        halfHeight = 0.5f * WebcamHandler.instance.GetHeight();
    }
    
    // Update is called once per frame
    void LateUpdate () {

        // Update webcam texture
        if (WebcamHandler.instance.DidUpdateThisFrame())
        {
            // Set webcam texture image
            rawImage.texture = WebcamHandler.instance.GetWebcamTextureRender();

            // Render this frame's crosses
            if (thresholdCrosses != null && CalibrationData.instance.showCrosses)
            {
                RenderCrosses();
            }
        }

        // always reset the webcam image's position
        transform.position = defaultPosition;
    }

    private void RenderCrosses()
    {
        foreach (Point pixel in WebcamInputReceiver.instance.GetCrossesList())
        {
            GameObject crossRender = Instantiate(CalibrationData.instance.thresholdCrossPrefab, Vector3.zero, Quaternion.identity);
            if (thresholdCrosses)
                crossRender.transform.SetParent(thresholdCrosses);
            Vector2 position = new Vector2(pixel.x, pixel.y);

            //float xPos = -position.x * (640f / WebcamHandler.instance.GetWidth()) * 0.8f + 640 * 0.4f;
            float xPos = -position.x * scaleFactorX + 0.5f * WebcamHandler.instance.GetWidth() * scaleFactorX;
            //if (horizontalFlip)
            //    xPos *= -1f;
            //float yPos = position.y * (480f / WebcamHandler.instance.GetHeight()) * 0.8f - 0.2f - 480f * 0.4f;
            float yPos = position.y * scaleFactorY - 0.5f * WebcamHandler.instance.GetHeight() * scaleFactorY;
            //if (verticalFlip)
            //    yPos *= -1f;

            crossRender.transform.localPosition = new Vector3(xPos, yPos, 0f);

            // set lifespan
            crossRender.GetComponent<CrossLifespan>().lifespan = CalibrationData.instance.crossLifetime;

            //if (!CalibrationCorners.instance.IsWithinMarkers(crossRender.transform.position.x, crossRender.transform.position.y))
            //    Destroy(crossRender);
            //-position.y * (480f / height) * 0.8f - 0.2f, 0f);
        }
    }

    //public void SetHorizontalFlip(Dropdown dropdown)
    //{
    //    if (dropdown.value == 1)
    //    {
    //        horizontalFlip = true;
    //        transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
    //    }
    //    else
    //    {
    //        horizontalFlip = false;
    //        transform.localScale = new Vector3(-1f, transform.localScale.y, transform.localScale.z);
    //    }
    //}
    //
    //public void SetVerticalFlip(Dropdown dropdown)
    //{
    //    if (dropdown.value == 1)
    //    {
    //        verticalFlip = true;
    //        transform.localScale = new Vector3(transform.localScale.x, -1f, transform.localScale.z);
    //    }
    //    else
    //    {
    //        verticalFlip = false;
    //        transform.localScale = new Vector3(transform.localScale.x, 1f, transform.localScale.z);
    //    }
    //}

}
