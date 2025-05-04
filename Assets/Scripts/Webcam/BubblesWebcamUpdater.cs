using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblesWebcamUpdater : EffectManager
{
    protected override void HandlePoint(Vector2 point)
    {
        base.HandlePoint(point);
        RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(point)); 
        if (hit.collider != null && hit.collider.tag == "Bubble")
        {
            hit.transform.GetComponent<TouchHandler>().Interact();
        }
    }
}
