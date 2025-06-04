using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Runtime.InteropServices;

public class AnimalInteractionHandler : MonoBehaviour
{

[DllImport("user32.dll")]
static extern bool SetCursorPos(int X, int Y);

public UnityEvent clickEvent;

    public SkinnedMeshRenderer[] renderers;
    public MeshRenderer[] mRenderers;

    private int cursorPosX, cursorPosY;


    bool pauseTouch;

    private void Awake()
    {
        // Set cursor position to top-middle of screen
        cursorPosX = (int)(Screen.width * 0.5f);
        cursorPosY = 0;
    }

    public void OnMouseEnter()
    {
        if (!DisableWebcam.DISABLE_WEBCAM_FLAG)
            return;
        if (pauseTouch) return;
        if (Interact())
        {
            SetCursorPos(cursorPosX, cursorPosY);//Call this to set the mouse position
            SetCursorPos(0, Screen.height);//Call this to set the mouse position to the bottom-left of screen
        }
    }

    public virtual bool Interact()
    {
        clickEvent.Invoke();
        return true;
    }

    public void SetParent(Transform ob)
    {
        ob.parent = transform.parent;
    }

    public virtual void SetTexture(Texture2D texture)
    {
        foreach (var r in renderers)
        {
            r.material.mainTexture=texture;
        }

        foreach (var r in mRenderers)
        {
            r.material.mainTexture = texture;
        }
    }

    public void PauseResumeTouch(bool pause)
    {
        pauseTouch = pause;
    }
}