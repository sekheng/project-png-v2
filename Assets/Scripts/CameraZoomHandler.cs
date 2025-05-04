using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomHandler : MonoBehaviour
{
    public float zoomFactor, moveSpeed;

    private Camera cam;

    private float baseFOV;

    void Start()
    {
        cam = GetComponent<Camera>();
        baseFOV = cam.fieldOfView;
    }

    void LateUpdate()
    {
        var zoom = cam.fieldOfView;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            zoom -= zoomFactor;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            zoom += zoomFactor;
            if (zoom >= baseFOV)
            {
                zoom = baseFOV;
            }
        }

        cam.fieldOfView = zoom;

        var pos = transform.localPosition;

        if (Input.GetKey(KeyCode.A))
        {
            pos -= moveSpeed * transform.right;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            pos += moveSpeed * transform.right;
        }
        if (Input.GetKey(KeyCode.W))
        {
            pos += moveSpeed * transform.up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            pos -= moveSpeed * transform.up;
        }

        transform.localPosition = pos;
    }
}