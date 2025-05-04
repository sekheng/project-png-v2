using UnityEngine;

/*
 * Note: "Trapezium" is the name used for discussion, but it is more accurately a quadrilateral.
 */
public class Trapezium
{
    /*
     * Reference to 4 points
     */
    private TrapeziumPoint topLeft;
    private TrapeziumPoint topRight;
    private TrapeziumPoint botLeft;
    private TrapeziumPoint botRight;

    /*
     * Min and max x and y of trapezium (denoted as width and height)
     */
    private int minWidth;
    private int minHeight;
    private int maxWidth;
    private int maxHeight;

    // for normalization calculation
    private Vector3 n0, n1, n2, n3;

    public void SetTrapeziumPoints(TrapeziumPoint topLeft, TrapeziumPoint topRight, TrapeziumPoint botLeft, TrapeziumPoint botRight)
    {
        this.topLeft = topLeft;
        this.topRight = topRight;
        this.botLeft = botLeft;
        this.botRight = botRight;

        UpdateMinMaxWidthHeight();
    }

    /// <summary>
    /// Update minWidth, minHeight, maxWidth and maxHeight
    /// </summary>
    public void UpdateMinMaxWidthHeight()
    {
        /// 0 to max: right to left
        minWidth = Mathf.Min(topRight.x, botRight.x);
        maxWidth = Mathf.Max(topLeft.x, botLeft.x);

        // smallest (bottommost) Y value
        minHeight = Mathf.Min(botLeft.y, botRight.y);
        // largest (rightmost) Y value
        maxHeight = Mathf.Max(topLeft.y, topRight.y);

        //Debug.Log("minWidth: " + minWidth + " maxWidth: " + maxWidth + " minHeight: " + minHeight + " maxHeight: " + maxHeight);
    }

    /// <summary>
    /// Update calculated trapezium normals whenever one of its trapezium points move
    /// </summary>
    public void UpdateTrapeziumNormals()
    {
        // find normals - cross product with forward vector (0,0,1)
        /// N0: normal of P3->P0 (topLeft to botLeft)
        n0 = Vector3.Cross((topLeft.Pos - botLeft.Pos), Vector3.forward);
        /// N1: normal of P0->P1 (botLeft to botRight)
        n1 = Vector3.Cross((botLeft.Pos - botRight.Pos), Vector3.forward);
        /// N2: normal of P1->P2 (botRight to topRight)
        n2 = Vector3.Cross((botRight.Pos - topRight.Pos), Vector3.forward);
        /// N3: normal of P2->P3 (topRight to topLeft)
        n3 = Vector3.Cross((topRight.Pos - topLeft.Pos), Vector3.forward);

        //Debug.Log("topLeft.Pos: " + topLeft.Pos + " topRight.Pos: " + topRight.Pos + " botLeft.Pos: " + botLeft.Pos + " botRight.Pos: " + botRight.Pos);
    }

    /// <summary>
    /// Normalize point in the trapezium
    /// </summary>
    /// <param name="posX">Xth column pixel (width)</param>
    /// <param name="posY">Yth row pixel (height)</param>
    /// <returns>Point in irregular quadrilaterial as a Vector2, each X and Y ranging 0 to 1 each
    /// If point is not in quadrilateral, value will not be within range</returns>
    public Vector2 NormalizePoint(int posX, int posY)
    {
        // represent pixel position as Vector3
        Vector3 p = new Vector3(posX, posY, 0f);

        // find normalized position
        /// calculate dot products used in calculation
        float dotPP0_N0 = Vector3.Dot((p - botLeft.Pos), n0);
        float dotPP0_N1 = Vector3.Dot((p - botLeft.Pos), n1);
        float dotPP2_N2 = Vector3.Dot((p - topRight.Pos), n2);
        float dotPP3_N3 = Vector3.Dot((p - topLeft.Pos), n3);
        /// calculate normalized positions
        float u = dotPP0_N0 / (dotPP0_N0 + dotPP2_N2);
        float v = dotPP0_N1 / (dotPP0_N1 + dotPP3_N3);

        return new Vector2(u, v);
    }

    public bool IsPointOutsideTrapezium(int posX, int posY)
    {
        // check if point is outside bounding box of trapezium
        if (posX < minWidth || posX >= maxWidth || posY < minHeight || posY >= maxHeight)
            return true;

        // check if point is outside trapezium
        Vector2 normPt = NormalizePoint(posX, posY);
        return (normPt.x < 0f || normPt.x > 1f || normPt.y < 0f || normPt.y > 1f);
    }

    public Vector2 IsPointWithinTrapeziumAndNormalize(int posX, int posY, out bool IsPointValid)
    {
        Vector2 normPt = Vector2.zero;

        // check if point is outside bounding box of trapezium
        if (posX < minWidth || posX >= maxWidth || posY < minHeight || posY >= maxHeight)
        {
            IsPointValid = false;   // point is not in trapezium
        }
        else
        {
            // check if point is outside trapezium
            normPt = NormalizePoint(posX, posY);
            IsPointValid = !(normPt.x < 0f || normPt.x > 1f || normPt.y < 0f || normPt.y > 1f);
        }

        return normPt;
    }
}
