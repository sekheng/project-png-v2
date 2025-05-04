using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZooAnimalsEnabler : MonoBehaviour
{
    public static ZooAnimalsEnabler Instance;

    public GameObject animals;

    private void Awake()
    {
        Instance = this;
    }

    public void EnableAnimals()
    {
        animals.SetActive(true);
    }
}