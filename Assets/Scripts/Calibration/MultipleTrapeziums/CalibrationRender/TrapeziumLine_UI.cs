using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapeziumLine_UI : MonoBehaviour
{
    /*
     * Line endpoint
     */
    private Transform point0;
    private Transform point1;

    public void AssignPoints(Transform point0, Transform point1)
    {
        this.point0 = point0;
        this.point1 = point1;

        // draw line correctly
        UpdateLine();
    }

    private static float lengthScale = 0.555f;  // default for 1080p screen reso
    public static float LengthScale
    {
        set { lengthScale = value; }
    }

    public void UpdateLine()
    {
        transform.position = point0.position;
        Vector3 dir = point1.position - point0.position;
        transform.localScale = new Vector3(lengthScale * dir.magnitude, transform.localScale.y, 1f);
        float angle = Mathf.Atan2(dir.y, dir.x);
        transform.eulerAngles = new Vector3(0f, 0f, Mathf.Rad2Deg * angle);
    }
}
