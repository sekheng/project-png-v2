using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagnameInteractionHandler : EffectManager
{
    [SerializeField, Tooltip("The tagname to interact with")]
    private string tagName = "Butterfly";
    protected override void HandlePoint(Vector2 point)
    {
        base.HandlePoint(point);

        RaycastHit hitInfo;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(point), out hitInfo))
        {
            if (hitInfo.collider.tag == tagName)
            {
                hitInfo.transform.GetComponent<FishEffect>().Interact();
            }
        }
    }
}
