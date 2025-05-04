using System.Collections.Generic;
using UnityEngine;

public struct Point
{
    public int x;
    public int y;

    public Point(int x_, int y_)
    {
        x = x_;
        y = y_;
    }

    public static Point operator+(Point a, Point b)
    {
        return new Point(a.x + b.x, a.y + b.y);
    }
}

public class BlobData {

    public static List<BlobData> blobs = new List<BlobData>();
    // red blobs are to be kept for slightly longer and produce crosses
    public static List<BlobData> redBlobs = new List<BlobData>();

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    public float GetMinX() { return minX; }
    public float GetMinY() { return minY; }
    public float GetMaxX() { return maxX; }
    public float GetMaxY() { return maxY; }

    //private int timer = 3;
    public int lifetime;

    //private bool setToRemove = false;

    /// <summary>
    ///  Update blob
    /// </summary>
    /// <returns> True if blob reached end of lifespan, false if not </returns>
    public bool UpdateLifetime()
    {
        //--timer;
        //if (timer <= 0)
        //    return true;

        //lifetime -= Time.deltaTime;

        //if (setToRemove)
        //    return true;

        if (isFirstFrame)
            isFirstFrame = false;

        --lifetime;
        if (lifetime <= 0)
            return true;

        return false;
    }

    //public void SetToRemove()
    //{
    //    setToRemove = true;
    //}

    private List<Point> pixels;   // list of pixels that make up this blob
    private List<Point> crossedPixels;  // list of pixels that have crosses rendered already

    private bool isFirstFrame = true;   // blob is at the frame it was initialised
    public bool IsFirstFrame() { return isFirstFrame; }

    public void ShiftPixel(int idx)
    {
        Point pixel = pixels[idx];
        crossedPixels.Add(pixel);
        pixels.RemoveAt(idx);
    }
    private int overallPixelCount = 0;
    public int GetOverallPixelCount() { return overallPixelCount; }

    public BlobData(int X, int Y)
    {
        pixels = new List<Point>();
        crossedPixels = new List<Point>();

        minX = X;
        maxX = X;
        minY = Y;
        maxY = Y;

        Add(X, Y);

        lifetime = CalibrationData.instance.redBlobLifetime;
    }

    public bool IsNear(int X, int Y)
    {
        //if (X > minX - 1 && X < maxX + 1
        //    && Y > minY - 1 && Y < maxY + 1)    // very near the blob if it's huge
        //    return true;

        /// Check distance of point to center of blob
        //float centerX = (minX + maxX) * 0.5f;
        //float centerY = (minY + maxY) * 0.5f;
        //
        //float distSqr = DistSqr(centerX, X, centerY, Y);    // get dist between the point and the blob
        //if (distSqr < CalibrationData.instance.distThreshold * CalibrationData.instance.distThreshold)
        //    return true;

        /// check for distance to near pixels
        //foreach (Point p in pixels)
        //{
        //    float distSqr = DistSqr(p.x, X, p.y, Y);
        //    if (distSqr < CalibrationData.instance.distThreshold * CalibrationData.instance.distThreshold)
        //        return true;
        //}

        /// find point's distance to edge
        float cx = Mathf.Max(Mathf.Min(X, maxX), minX);
        float cy = Mathf.Max(Mathf.Min(Y, maxY), minY);
        float distSqr = Utility.DistSqr(cx, X, cy, Y);
        if (distSqr < CalibrationData.instance.distThreshold * CalibrationData.instance.distThreshold)
            return true;

        return false;
    }

    public bool IsNear(int X, int Y, ref float distSqr)
    {
        //if (X > minX - 1 && X < maxX + 1
        //    && Y > minY - 1 && Y < maxY + 1)    // very near the blob if it's huge
        //    return true;

        float centerX = (minX + maxX) * 0.5f;
        float centerY = (minY + maxY) * 0.5f;

        distSqr = Utility.DistSqr(centerX, X, centerY, Y);  // get dist between the point and the blob
        if (distSqr < CalibrationData.instance.distThreshold * CalibrationData.instance.distThreshold)
            return true;

        return false;
    }

    public void Add(int X, int Y)
    {
        Point point = new Point(X, Y);
        pixels.Add(point);

        // change min and max values
        minX = Mathf.Min(X, minX);
        maxX = Mathf.Max(X, maxX);
        minY = Mathf.Min(Y, minY);
        maxY = Mathf.Max(Y, maxY);

        ++overallPixelCount;
    }

    public Point GetPixel(int idx)
    {
        return pixels[idx];
    }

    public int Size()  // size of this blob
    {
        return pixels.Count;
    }
	
    public Vector2 GetCenterPoint()
    {
        /// average position of all pixels - to avoid outliers
        /// but don't get every single pixel; too costly
        float thisX = 0f, thisY = 0f;
        //foreach (Point pixel in pixels)
        //{
        //    minX += pixel.x;
        //    minY += pixel.y;
        //}
        //minX /= pixels.Count;
        //minY /= pixels.Count;

        int count = 0;
        for (int i = 0; i < pixels.Count; i += 2, ++count)
        {
            Point pixel = pixels[i];
            thisX += pixel.x;
            thisY += pixel.y;
        }
        thisX /= count;
        thisY /= count;

        return new Vector2(thisX, thisY);

        //return new Vector2((minX + maxX) * 0.5f, (minY + maxY) * 0.5f);
    }

    public static int GetIndexOfLargestBlob()
    {
        int recordSize = 0;
        int blobIdx = 0;
        for (int i = 0; i < blobs.Count; ++i)
        {
            BlobData blob = blobs[i];
            if (blob.Size() > recordSize)
            {
                recordSize = blob.Size();
                blobIdx = i;
            }
        }

        return blobIdx;
    }

    /// <summary>
    ///  Acceptable red blob size is 80% of the largest size for this frame
    /// </summary>
    public static int CalculateAcceptableRedBlobSize()
    {
        int largestBlobIdx = GetIndexOfLargestBlob();   // get largest blob

        return (int)(0.8f * blobs[largestBlobIdx].Size());
    }

}
