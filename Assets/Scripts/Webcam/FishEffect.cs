using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishEffect : MonoBehaviour
{


    
    //public void OnMouseOver()
    //{
        //GetComponent<AnimalInteractionHandler>().OnMouseEnter();
    //}

    public virtual bool Interact()
    {
        GetComponent<AnimalInteractionHandler>().Interact();
        return true;
    }
}
