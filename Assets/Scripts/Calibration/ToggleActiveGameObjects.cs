using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleActiveGameObjects : MonoBehaviour
{
    [SerializeField]
    private GameObject[] gameObjects;
    [SerializeField]
    [Min(-1)]
    [Tooltip("Set -1 for all objects to be disabled at Awake")]
    private int firstActiveIdx = 0;

    private GameObject lastToggledObject = null;

    private void Awake()
    {
        // disable all other objects except the first active one
        for (int i = 0; i < gameObjects.Length; ++i)
        {
            if (i != firstActiveIdx)
                gameObjects[i].SetActive(false);
            else
            {
                gameObjects[i].SetActive(true);
                lastToggledObject = gameObjects[i];
            }
        }
    }

    /// <summary>
    /// Function callback
    /// </summary>
    public void ToggleToObject(int idx)
    {
        if (idx >= gameObjects.Length)
        {
            Debug.LogWarning("Error in ToggleActiveGameObjects Component in " + gameObject.name + ": GameObjects list is smaller than provided index!");
            return;
        }

        // disable previous object (if any)
        if (lastToggledObject != null)
            lastToggledObject.SetActive(false);

        // set the new object to be active
        gameObjects[idx].SetActive(true);
        // set the new object as last toggled
        lastToggledObject = gameObjects[idx];
    }
}
