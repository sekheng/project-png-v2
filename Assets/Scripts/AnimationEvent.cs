using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : MonoBehaviour
{
    public ButterflyInteractionHandler butterfly;

    public UnityEvent animationEvent;

    public void RestartWalk()
    {
        if(butterfly!=null)
            butterfly.OnButterflyOutComplete();

        animationEvent.Invoke();
    }
}