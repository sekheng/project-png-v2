using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class TouchHandler : MonoBehaviour
{
    Tweener tween;

    public Vector2 xVarient;

    public float yVal, floatingTime;

    bool hasTouched;

    public BubbleHandler parentObj;
    [DllImport("user32.dll")]
    static extern bool SetCursorPos(int X, int Y);
    private int cursorPosX, cursorPosY;

    void OnEnable()
    {
        transform.position = transform.parent.position;
        tween = transform.DOMove(new Vector3(transform.parent.position.x+Random.Range(xVarient.x, xVarient.y), yVal, transform.parent.position.z), floatingTime)
            .OnComplete(()=> { tween = null; gameObject.SetActive(false); hasTouched = false; });
    }
    private void Start()
    {
        // Set cursor position to top-middle of screen
        cursorPosX = (int)(Screen.width * 2);
        cursorPosY = (int)(Screen.height * 2);
    }

    private void OnDisable()
    {
        transform.localScale = Vector3.one * -1;

        if (tween != null)
        {
            tween.Kill();
            tween = null;
        }
    }

    public void OnMouseEnter()
    {
        if (DisableWebcam.DISABLE_WEBCAM_FLAG)
            return;
        if (Interact())
        {
            SetCursorPos(cursorPosX, cursorPosY);//Call this to set the mouse position
        }
    }

    public virtual bool Interact()
    {
        if (!GetComponentInParent<BubbleHandler>().pauseTouch && !hasTouched && tween != null)
        {
            hasTouched = true;
            transform.DOShakeScale(0.25f, 1f, 20);
            GetComponent<AudioSource>().Play();
            tween.timeScale = parentObj.touchSpeed;
            return true;
        }
        return false;
    }
}