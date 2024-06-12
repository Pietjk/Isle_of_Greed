using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzelGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectsToCreate;
    [SerializeField] private List<Transform> spawnPositions;
    List<bool> occupied;

    public float amountOfItems;


    private void Start()
    {
        amountOfItems = Random.Range(3, 11);

        occupied = new List<bool>();

        for (int i = 0; i < spawnPositions.Count; i++)
        {
            occupied.Add(false);
        }

        SpawnObject();
    }

    private void SpawnObject()
    {
        for (int i = 0; i < amountOfItems; i++)
        {
            int selectedSpawnPosition = Random.Range(0, spawnPositions.Count);

            while (occupied[selectedSpawnPosition])
            {
                selectedSpawnPosition = Random.Range(0, spawnPositions.Count);
            }

            Instantiate(objectsToCreate[Random.Range(0, objectsToCreate.Count)], spawnPositions[selectedSpawnPosition].position, Quaternion.identity);

            occupied[selectedSpawnPosition] = true;
        }
    }
}
