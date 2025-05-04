using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableAnimals : MonoBehaviour
{
    public void EnableZooAnimals()
    {
        ZooAnimalsEnabler.Instance.EnableAnimals();
    }
}