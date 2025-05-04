using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointsHolder : MonoBehaviour
{
    [SerializeField]
    private WaypointsData[] waypointsData;

    public static WaypointsHolder Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDisable()
    {
        Instance = null;
    }

    public Transform[] GetWaypoints(string name)
    {
        foreach (var item in waypointsData)
        {
            if(name.Contains(item.animalName))
            {
                return item.waypoints;
            }
        }
        return null;
    }
}

[System.Serializable]
public class WaypointsData
{
    public string animalName;
    public Transform[] waypoints;
}