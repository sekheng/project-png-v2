using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapeziumsManager : MonoBehaviour
{
    /*
     * Number of trapeziums
     */
    [SerializeField] private int rows;
    [SerializeField] private int columns;
    public int Rows
    {
        get { return rows; }
    }
    public int Columns
    {
        get { return columns; }
    }

    private Trapezium[,] trapeziums;    // trapezium[0,0] located at bottom right
    private Dictionary<TrapeziumPointId, TrapeziumPoint> trapeziumPoints;

    public Dictionary<TrapeziumPointId, TrapeziumPoint> GetTrapeziumPoints()
    {
        return trapeziumPoints;
    }

    /// <summary>
    /// Load trapeziums data either when WebcamHandler first initializes or when setting modes are changed
    /// </summary>
    public void LoadTrapeziumsData()
    {
        trapeziums = new Trapezium[rows, columns];
        trapeziumPoints = new Dictionary<TrapeziumPointId, TrapeziumPoint>();

        // get setting mode
        CalibrationSettings.CalibrationSetting settingMode = AppManager.instance.SettingMode;

        // can load trapezium point positions if saved rows and columns are equal
        //  and webcam resolution same as previously loaded
        bool pointsValid = TrapeziumsDataStore.TrapeziumDataIsValid(this);
        //Debug.Log("When loading, are pointsValid? " + pointsValid);

        // Update positions of points
        if (pointsValid)    // load trapezium point positions from PlayerPrefs
        {
            for (int i = 0; i <= rows; ++i)
            {
                for (int j = 0; j <= columns; ++j)
                {
                    // get id
                    TrapeziumPointId id = new TrapeziumPointId(i, j);

                    // read point from data store
                    string pointData = TrapeziumsDataStore.GetPointData(settingMode, i, j);
                    TrapeziumPoint point = TrapeziumPoint.Parse(pointData);
                    trapeziumPoints.Add(id, point);
                }
            }
        }
        else    // cannot load previous points, so manually set positions
        {
            int webcamWidth = WebcamHandler.instance.GetWidth();
            int webcamHeight = WebcamHandler.instance.GetHeight();

            // get length & height of each trapezium
            int segmentWidth = webcamWidth / columns;
            int segmentHeight = webcamHeight / rows;

            // set rows & columns and webcam width & height to PlayerPrefs
            TrapeziumsDataStore.SetTrapeziumConfiguration(settingMode, this);

            // create trapezium points & set their positions
            for (int i = 0; i <= rows; ++i)
            {
                for (int j = 0; j <= columns; ++j)
                {
                    // get id
                    TrapeziumPointId id = new TrapeziumPointId(i, j);

                    // create points
                    trapeziumPoints.Add(id, new TrapeziumPoint());

                    // get X pos (columns, width)
                    int x = (j == columns) ? webcamWidth : j * segmentWidth;    // right to left
                    // get Y pos (rows, height)
                    int y = (i == rows) ? webcamHeight : i * segmentHeight; // bottom to top

                    // set X and Y positions
                    trapeziumPoints[id].setPos(x, y);

                    // save positions to PlayerPrefs
                    TrapeziumPoint.SaveToPlayerPrefs(trapeziumPoints[id], id);
                }
            }

            // save
            PlayerPrefs.Save();
        }

        // create trapeziums
        for (int i = 0; i < rows; ++i)
        {
            for (int j = 0; j < columns; ++j)
            {
                // create trapezium
                trapeziums[i,j] = new Trapezium();

                // assign points
                trapeziums[i,j].SetTrapeziumPoints(
                    trapeziumPoints[new TrapeziumPointId(i+1, j+1)], // top left
                    trapeziumPoints[new TrapeziumPointId(i+1, j)], // top right
                    trapeziumPoints[new TrapeziumPointId(i, j+1)], // bot left
                    trapeziumPoints[new TrapeziumPointId(i, j)] // bot right
                    );

                trapeziumPoints[new TrapeziumPointId(i, j)].AddConnectedTrapezium(trapeziums[i, j]);
                trapeziumPoints[new TrapeziumPointId(i, j + 1)].AddConnectedTrapezium(trapeziums[i, j]);
                trapeziumPoints[new TrapeziumPointId(i+1, j)].AddConnectedTrapezium(trapeziums[i, j]);
                trapeziumPoints[new TrapeziumPointId(i+1, j+1)].AddConnectedTrapezium(trapeziums[i, j]);
            }
        }

        // Update trapeziums' normals
        for (int i = 0; i < rows; ++i)
        {
            for (int j = 0; j < columns; ++j)
            {
                trapeziums[i, j].UpdateTrapeziumNormals();
            }
        }
    }

    /// <summary>
    /// Check if point is outside all of the trapeziums
    /// </summary>
    /// <param name="posX">X coordinate of pixel</param>
    /// <param name="posY">Y coordinate of pixel</param>
    /// <returns>Whether point is outside all of the trapeziums</returns>
    public bool IsPointOutsideTrapeziums(int posX, int posY)
    {
        for (int i = 0; i < rows; ++i)
        {
            for (int j = 0; j < columns; ++j)
            {
                // check if point is not outside trapezium
                if (!trapeziums[i, j].IsPointOutsideTrapezium(posX, posY))
                    return false;   // point is inside a trapezium
            }
        }

        return true;    // point is outside all trapeziums
    }

    // convert point to screen
    public Point TrapeziumPointToScreenPoint(Point point, int targetWidth, int targetHeight)
    {
        for (int i = 0; i < rows; ++i)
        {
            for (int j = 0; j < columns; ++j)
            {
                // by right, Point should only be valid in 1 trapezium
                bool isPointValid;
                Vector2 normalizedPoint = trapeziums[i,j].IsPointWithinTrapeziumAndNormalize(point.x, point.y, out isPointValid);
                if (isPointValid)
                {
                    // calculate which segment point is in
                    int segmentWidth = targetWidth / columns;
                    int segmentHeight = targetHeight / rows;

                    // invert normalized x
                    normalizedPoint.x = 1f - normalizedPoint.x;

                    Point relativePt = new Point((int)(normalizedPoint.x * segmentWidth), (int)(normalizedPoint.y * segmentHeight));

                    Point offset = new Point(j * segmentWidth, i * segmentHeight);

                    return relativePt + offset;
                }
            }
        }

        return new Point(-1, -1);   // this should not occur!
    }

    /// <summary>
    /// Set trapezium points to their default positions.
    /// Normals will be updated by TrapeziumPoint update callback.
    /// </summary>
    public void ResetTrapeziumPointsToDefaults()
    {
        int webcamWidth = WebcamHandler.instance.GetWidth();
        int webcamHeight = WebcamHandler.instance.GetHeight();

        // get length & height of each trapezium
        int segmentWidth = webcamWidth / columns;
        int segmentHeight = webcamHeight / rows;

        // set positions for each trapezium point
        for (int i = 0; i <= rows; ++i)
        {
            for (int j = 0; j <= columns; ++j)
            {
                // get id
                TrapeziumPointId id = new TrapeziumPointId(i, j);

                // get X pos (columns, width)
                int x = (j == columns) ? webcamWidth : j * segmentWidth;    // right to left
                // get Y pos (rows, height)
                int y = (i == rows) ? webcamHeight : i * segmentHeight; // bottom to top

                // set X and Y positions
                trapeziumPoints[id].setPos(x, y);

                // save positions to PlayerPrefs
                TrapeziumPoint.SaveToPlayerPrefs(trapeziumPoints[id], id);
            }
        }

        /*
        // Update trapeziums' normals
        for (int i = 0; i < rows; ++i)
        {
            for (int j = 0; j < columns; ++j)
            {
                trapeziums[i, j].UpdateTrapeziumNormals();
            }
        }*/
    }
}
