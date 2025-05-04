using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEffect : EffectManager
{
    [SerializeField] private GameObject calibrationCrossPrefab;
    [SerializeField] private Transform crossesParent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput(true);
    }

    protected override void HandlePoint(Vector2 point)
    {
        GameObject cross = Instantiate(calibrationCrossPrefab);

        cross.transform.SetParent(crossesParent);

        cross.transform.position = new Vector3(point.x, point.y, 0f);
    }
}
