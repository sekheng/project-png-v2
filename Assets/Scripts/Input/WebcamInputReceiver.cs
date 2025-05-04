using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebcamInputReceiver : MonoBehaviour {

    public static WebcamInputReceiver instance = null;

    // user input data - is data for the current frame
    private List<BlobData> redBlobsList;
    private List<Point> crossesList;
    private int crossRadius = 0;    // how many screen pixels each cross affects; affected by screen resolution and calibration size

    // calibration resolution
    private int inputWidth;
    private int inputHeight;

    // scale factor (calculated) -- Not needed anymore
    //private float scaleFactorWidth;
    //private float scaleFactorHeight;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
            return;
        }

        redBlobsList = new List<BlobData>();
        crossesList = new List<Point>();
    }
    
    public void AddRedBlob(BlobData redBlob)
    {
        redBlobsList.Add(redBlob);
    }

    public void AddCross(Point point)
    {
        crossesList.Add(point);
    }

    /// <summary>
    ///  Clear list of crosses & red blobs. Called at start of each updated webcam frame
    /// </summary>
    public void ClearInputList()
    {
        redBlobsList.Clear();
        crossesList.Clear();
    }

    public List<Point> GetCrossesList()
    {
        return crossesList;
    }

    public void SetInputWidth(int newWidth)
    {
        inputWidth = newWidth;
    }

    public void SetInputHeight(int newHeight)
    {
        inputHeight = newHeight;
    }

    /// <summary>
    ///  Calculate the point position relative to the screen, or the otherwise specified rectangle
    /// </summary>
    /// <returns>The relative point to the screen or specified rectangle</returns>
    public Point CalculateRelativePoint(Point point, bool useScreenSize, int width = 0, int height = 0)
    {
        // invert x-coordinate of point as camera image is usually inverted from screen
        //point.x = inputWidth - (point.x - WebcamHandler.instance.minWidth); //WebcamHandler.instance.maxWidth - point.x + WebcamHandler.instance.minWidth;
        //point.y = point.y - WebcamHandler.instance.minHeight;

        int targetWidth = width;
        int targetHeight = height;
        if (useScreenSize)
        {
            targetWidth = AppManager.instance.GetScreenWidth();
            targetHeight = AppManager.instance.GetScreenHeight();
        }

        // for single trapezium: get normalized point first
        //Vector2 normalizedPoint = WebcamHandler.instance.NormalizePoint(point.x, point.y);
        //Point relativePt = new Point((int)(normalizedPoint.x * targetWidth), (int)(normalizedPoint.y * targetHeight));
        //return relativePt;

        Point relativePt = WebcamHandler.instance.TrapeziumsManager.TrapeziumPointToScreenPoint(point, targetWidth, targetHeight);
        // invert x-coordinate of point as camera image is usually inverted from screen
        /// invert after calculating relative point, as original point needs to be within the trapezium
        relativePt.x = targetWidth - relativePt.x;

        return relativePt;
    }

}
