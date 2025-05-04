using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalsSpawner : MonoBehaviour
{
    public static AnimalsSpawner Instance;

    public string folderName;

    public GameObject[] animals;

    protected List<GameObject> spawnedAnimals;

    private void Awake()
    {
        spawnedAnimals = new List<GameObject>();
        Instance = this;
    }

    public bool SpawnAnimal(string name, Texture2D texture)
    {

        foreach (var animal in animals)
        {
            if(animal.name.Equals(name) && animal.transform.childCount==0)
            {
                Object animalObj = Resources.Load($"{folderName}/{name}");
                var an = Instantiate(animalObj, animal.transform) as GameObject;
                an.name = name;
                var setTexture = an.GetComponent<SetTexture>();
                if (setTexture != null)
                {
                    setTexture.SetTextureOnAnimal(texture);
                }
                else
                {
                    var setTexture2D = an.GetComponentInChildren<SetTexture2D>();
                    if (setTexture2D != null)
                    {
                        setTexture2D.SetTextureOnAnimal(texture);
                    }
                }
                spawnedAnimals.Add(an);
                return true;
            }
        }
        return false;
    }

    public void DeleteAll()
    {
        foreach (var animal in animals)
        {
            if (animal.transform.childCount > 0)
            {
                for (int i = 0; i < animal.transform.childCount; i++)
                {
                    Destroy(animal.transform.GetChild(i).gameObject);
                }
            }
        }
    }

    public virtual void SetAnimalsSpeed(int amount)
    {
        foreach (var animal in spawnedAnimals)
        {
            animal.GetComponent<AnimalController>().SetWalkSpeed(amount);
        }
    }
    public void SetAnimalsSpeed(float amount)
    {
        foreach (var animal in spawnedAnimals)
        {
            animal.GetComponentInChildren<BubbleHandler>().touchSpeed = amount;
        }
    }
}