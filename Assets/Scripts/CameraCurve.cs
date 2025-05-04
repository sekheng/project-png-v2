using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCurve : MonoBehaviour
{
    public Animator anim;

    private void Awake()
    {
        anim.Play("CameraCurvePath", 0, 0);
    }

    public void SetCameraCurve(float value)
    {
        anim.Play("CameraCurvePath", 0, (value-1)/10);
    }
}