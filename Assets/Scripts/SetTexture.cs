using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTexture : MonoBehaviour
{
    public AnimalInteractionHandler[] animalBodies;

    public void SetTextureOnAnimal(Texture2D texture)
    {
        foreach (var animal in animalBodies)
        {
            animal.SetTexture(texture);
        }
    }
}