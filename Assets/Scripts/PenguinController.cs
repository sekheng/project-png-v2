using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinController : AnimalPathFollow
{
    public GameObject bubbles;

    private bool isOnLand = true, isTouch;

    private void Start()
    {
        waypoints = WaypointsHolder.Instance.GetWaypoints(gameObject.name+"_Land");
    }

    protected override void ReachedWaypoint(Transform waypoint)
    {
        var wp = waypoint.GetComponent<Waypoint>();
        if (wp != null)
        {
            if (waypoint.name.Contains("BubblesOn"))
            {
                bubbles.SetActive(true);
            }
            else if (waypoint.name.Contains("BubblesOff"))
            {
                bubbles.SetActive(false);
            }
            
            if (isTouch)
            {
                isTouch = !wp.isTouchEnd;
                if (!isTouch)
                {
                    if (isOnLand)
                    {
                        waypoints = WaypointsHolder.Instance.GetWaypoints(gameObject.name + "_Land");
                    }
                    else
                    {
                        waypoints = WaypointsHolder.Instance.GetWaypoints(gameObject.name + "_Water");
                    }
                    currentWayPoint = 0;
                }
            }
        }
        base.ReachedWaypoint(waypoint);
    }

    public void OnPenguinTouched()
    {
        if (!isTouch)
        {
            isTouch = true;
            isOnLand = !isOnLand;

            if (isOnLand)
            {
                GetComponent<Animator>().Play("FastSwim");
                waypoints = WaypointsHolder.Instance.GetWaypoints(gameObject.name+"_W2L");
            }
            else
            {
                waypoints = WaypointsHolder.Instance.GetWaypoints(gameObject.name+"_L2W");
            }

            currentWayPoint = 0;

            OnTouch.Invoke();
        }
    }
}