using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trapeziums_UI : MonoBehaviour
{
    /*
     * Size of raw image in Calibration scene
     */
    [SerializeField] private int renderWidth = 640;
    [SerializeField] private int renderHeight = 480;
    public int RenderWidth { get { return renderWidth; } }
    public int RenderHeight { get { return renderHeight; } }

    // scale webcam position by following
    public float RenderWidthScale { get; private set; }
    public float RenderHeightScale { get; private set; }

    [SerializeField]
    private CanvasScaler canvasScaler;

    /*
     * Prefabs
     */
    [SerializeField] private GameObject trapeziumPointPrefab;
    [SerializeField] private GameObject trapeziumLinePrefab;


    [SerializeField] private Transform trapeziumParent;
    [SerializeField] private Transform linesParent;

    // reference to trapezium points
    private Dictionary<TrapeziumPointId, TrapeziumPoint_UI> trapeziumPoints_ui;

    /// <summary>
    /// Clamp a Trapezium Point UI object within the webcam window and its adjacent points,
    ///  to prevent trapeziums from inverting.
    /// </summary>
    /// <param name="id">Trapezium Point to clamp</param>
    /// <returns>Local position of the Trapezium Point after clamping</returns>
    public Vector3 ClampTrapeziumPoint(TrapeziumPointId id)
    {
        float minX, maxX, minY, maxY;   // min/max clamp positions
        float offset = 5f;  // minimum gap between trapezium points

        // get reference to TrapeziumsManager
        TrapeziumsManager trapeziumsManager = WebcamHandler.instance.TrapeziumsManager;

        // Check row position (id.i)
        if (id.i == 0)  // bottommost side
            minY = 0f;  // no other points below
        else
            minY = trapeziumPoints_ui[new TrapeziumPointId(id.i - 1, id.j)].transform.localPosition.y + offset;
        if (id.i == trapeziumsManager.Rows) // topmost side
            maxY = renderHeight;    // no other points above
        else
            maxY = trapeziumPoints_ui[new TrapeziumPointId(id.i + 1, id.j)].transform.localPosition.y - offset;

        // Check column position (id.j)
        if (id.j == 0)  // rightmost side
            maxX = renderWidth; // no other points to the right
        else
            maxX = trapeziumPoints_ui[new TrapeziumPointId(id.i, id.j - 1)].transform.localPosition.x - offset;

        if (id.j == trapeziumsManager.Columns)  // leftmost side
            minX = 0f;  // no other points to the left
        else
            minX = trapeziumPoints_ui[new TrapeziumPointId(id.i, id.j + 1)].transform.localPosition.x + offset;

        Debug.Log("ID: " + id + ", MinX: " + minX + ", MaxX: " + maxX);

        // get position of this point
        Vector3 thisPos = trapeziumPoints_ui[id].transform.localPosition;
        Vector3 clampedPos = new Vector3(Mathf.Clamp(thisPos.x, minX, maxX),
            Mathf.Clamp(thisPos.y, minY, maxY),
            thisPos.z);

        return clampedPos;
    }

    private bool initDone = false;
    
    // Start is called before the first frame update
    private IEnumerator Start()
    {
        if (!WebcamHandler.instance.IsTextureLoaded())
            yield return null;

        yield return null; // wait 1 more frame

        RenderWidthScale = (float)renderWidth / WebcamHandler.instance.GetWidth();
        RenderHeightScale = (float)renderHeight / WebcamHandler.instance.GetHeight();

        InitTrapeziumRender();
        initDone = true;

        if (AppManager.instance != null)
            TrapeziumLine_UI.LengthScale = canvasScaler.referenceResolution.y / AppManager.instance.GetScreenHeight();
    }

    public void RefreshTrapeziumsUI()
    {
        if (!initDone)
            return;

        RenderWidthScale = (float)renderWidth / WebcamHandler.instance.GetWidth();
        RenderHeightScale = (float)renderHeight / WebcamHandler.instance.GetHeight();

        Dictionary<TrapeziumPointId, TrapeziumPoint> trapeziumPoints = WebcamHandler.instance.TrapeziumsManager.GetTrapeziumPoints();
        // update position
        foreach (var key in trapeziumPoints_ui.Keys)
        {
            // get updated point model data
            TrapeziumPoint point = trapeziumPoints[key];

            // update trapezium point UI data
            TrapeziumPoint_UI point_ui = trapeziumPoints_ui[key];
            // assign point model data
            point_ui.SetTrapeziumPointModel(point);

            // set updated position of point
            point_ui.transform.localPosition = new Vector3(
                (WebcamHandler.instance.GetWidth() - point.Pos.x) * RenderWidthScale,
                point.Pos.y * RenderHeightScale,
                0f
            );

            point_ui.UpdateTrapeziums();
        }
    }

    private void InitTrapeziumRender()
    {
        // get reference to TrapeziumsManager
        TrapeziumsManager trapeziumsManager = WebcamHandler.instance.TrapeziumsManager;

        // render a point for each trapezium point
        Dictionary<TrapeziumPointId, TrapeziumPoint> trapeziumPoints = trapeziumsManager.GetTrapeziumPoints();
        trapeziumPoints_ui = new Dictionary<TrapeziumPointId, TrapeziumPoint_UI>();
        // instantiate points
        foreach (var key in trapeziumPoints.Keys)
        {
            // get point model data
            TrapeziumPoint point = trapeziumPoints[key];

            // instantiate point
            GameObject pointGO = Instantiate(trapeziumPointPrefab);
            // set parent of point
            pointGO.transform.SetParent(trapeziumParent);
            // set position of point
            pointGO.transform.localPosition = new Vector3(
                (WebcamHandler.instance.GetWidth() - point.Pos.x) * RenderWidthScale,
                point.Pos.y * RenderHeightScale,
                0f
            );

            // assign point model data
            TrapeziumPoint_UI point_ui = pointGO.GetComponent<TrapeziumPoint_UI>();
            point_ui.SetTrapeziumPointModel(point);
            // assign reference to this
            point_ui.SetTrapeziumsUI(this);
            // assign id
            point_ui.SetId(key);

            // add to dictionary
            trapeziumPoints_ui.Add(key, point_ui);
        }

        // render lines to connect some trapezium points
        for (int i = 1; i < WebcamHandler.instance.TrapeziumsManager.Rows; ++i)
        {
            for (int j = 1; j < WebcamHandler.instance.TrapeziumsManager.Columns; ++j)
            {
                // draw horizontal line to point on the left
                AddTrapeziumLine(trapeziumPoints_ui[new TrapeziumPointId(i, j)], trapeziumPoints_ui[new TrapeziumPointId(i, j-1)]);

                // draw vertical line to point on top
                AddTrapeziumLine(trapeziumPoints_ui[new TrapeziumPointId(i, j)], trapeziumPoints_ui[new TrapeziumPointId(i-1, j)]);

                // for last point, also draw point to the right
                if (j == WebcamHandler.instance.TrapeziumsManager.Columns - 1)
                {
                    AddTrapeziumLine(trapeziumPoints_ui[new TrapeziumPointId(i, j)], trapeziumPoints_ui[new TrapeziumPointId(i, j+1)]);
                }

                // for last row, also draw line downwards
                if (i == WebcamHandler.instance.TrapeziumsManager.Rows - 1)
                {
                    AddTrapeziumLine(trapeziumPoints_ui[new TrapeziumPointId(i, j)], trapeziumPoints_ui[new TrapeziumPointId(i+1, j)]);
                }
            }
        }

        // connect outer lines of trapezium
        for (int i = 0; i <= WebcamHandler.instance.TrapeziumsManager.Rows; ++i)
        {
            for (int j = 0; j <= WebcamHandler.instance.TrapeziumsManager.Columns; ++j)
            {
                // draw horizontal lines
                if ((i == 0 || i == WebcamHandler.instance.TrapeziumsManager.Rows) && j < WebcamHandler.instance.TrapeziumsManager.Columns)
                {
                    AddTrapeziumLine(trapeziumPoints_ui[new TrapeziumPointId(i, j)], trapeziumPoints_ui[new TrapeziumPointId(i, j+1)]);
                }

                // draw vertical lines
                if (i > 0 && (j == 0 || j == WebcamHandler.instance.TrapeziumsManager.Columns))
                {
                    AddTrapeziumLine(trapeziumPoints_ui[new TrapeziumPointId(i, j)], trapeziumPoints_ui[new TrapeziumPointId(i-1, j)]);
                }
            }
        }
    }

    private void AddTrapeziumLine(TrapeziumPoint_UI point0, TrapeziumPoint_UI point1)
    {
        // Instantiate Line
        GameObject lineGO = Instantiate(trapeziumLinePrefab);
        lineGO.transform.SetParent(linesParent);
        // set line's end points
        TrapeziumLine_UI line = lineGO.GetComponent<TrapeziumLine_UI>();
        line.AssignPoints(point0.transform, point1.transform);
        // assign line to points
        point0.AddTrapeziumLine(line);
        point1.AddTrapeziumLine(line);
    }

    /// <summary>
    /// Called by "Save" button in CalibrationScene.
    /// </summary>
    public void SaveTrapeziumPointsToPlayerPrefs()
    {
        Dictionary<TrapeziumPointId, TrapeziumPoint> trapeziumPoints = WebcamHandler.instance.TrapeziumsManager.GetTrapeziumPoints();
        foreach (var key in trapeziumPoints.Keys)
        {
            TrapeziumPoint.SaveToPlayerPrefs(trapeziumPoints[key], key);
        }
    }

    /// <summary>
    /// Called by "Reset Trapezium to Default" button in CalibrationScene.
    /// </summary>
    public void ResetTrapeziumPointsToDefault()
    {
        // reset positions
        WebcamHandler.instance.TrapeziumsManager.ResetTrapeziumPointsToDefaults();

        // update position
        foreach (var key in trapeziumPoints_ui.Keys)
        {
            TrapeziumPoint_UI point_ui = trapeziumPoints_ui[key];
            TrapeziumPoint point = point_ui.GetTrapeziumPointModel();

            // set position of point
            point_ui.transform.localPosition = new Vector3(
                (WebcamHandler.instance.GetWidth() - point.Pos.x) * RenderWidthScale,
                point.Pos.y * RenderHeightScale,
                0f
            );

            point_ui.UpdateTrapeziums();
        }
    }
}
