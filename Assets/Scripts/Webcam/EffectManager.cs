using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour {

    protected int screenWidth;
    protected int screenHeight;

    protected float delayBeforeProcessing = 3.0f;
    [SerializeField, Tooltip("To update the webcam input or not")]
    protected bool webcamInputFlag = true;

    // Use this for initialization
    protected void Awake() {

        screenWidth = Screen.width;
        screenHeight = Screen.height;

        // re-activate webcam
        if(WebcamHandler.instance)
            WebcamHandler.instance.ActivateWebcam();
    }
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(delayBeforeProcessing);
        delayBeforeProcessing = -1;
    }
    /// <summary>
    ///  To be called by child classes to go through the list of crosses.
    ///  Input to be mapped to screen size if useScreenSize is true
    /// </summary>
    /// <param name="useScreenSize"> Input to be mapped to screen size if true, or something else if false (e.g. an Image) </param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    protected virtual void ProcessInput(bool useScreenSize, int width = 0, int height = 0)
    {
        if (delayBeforeProcessing > 0)
        {
            return;
        }
        //if (mouseMode)  // use mouse only
        //{
        //    HandlePoint(Input.mousePosition);
        //}
        //else
        //HandlePoint(Input.mousePosition);   // Handle mouse position

        if (WebcamHandler.instance.IsWebcamActive())
        {
            foreach (Point point in WebcamInputReceiver.instance.GetCrossesList())
            {
                Point relativePt = WebcamInputReceiver.instance.CalculateRelativePoint(point, useScreenSize, width, height);
                
                HandlePoint(new Vector2(relativePt.x, relativePt.y));
            }
        }
        else
        {
            Debug.Log("Webcam is Inactive!");
        }
    }

    /// <summary>
    ///  To be overwritten by child classes, to use the converted point for an effect interaction
    /// </summary>
    /// <param name="point"></param>
    protected virtual void HandlePoint(Vector2 point)
    {

    }

    public void SetHandleWebcamInput(bool _setInput)
    {
        webcamInputFlag = _setInput;
    }

    protected void Update()
    {
        if (webcamInputFlag)
            ProcessInput(true);
    }
}
