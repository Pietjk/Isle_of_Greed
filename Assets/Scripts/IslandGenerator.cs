using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> islandVariants;
    [SerializeField] private int amountOfIslands;
    [SerializeField] private float minHubDistance = 20, maxHubDistance = 100, minIslandDistance = 30;

    private List<GameObject> islands;

    private void Start()
    {
        islands = new List<GameObject>();

        for (int i = 0; i < amountOfIslands; i++)
        {
            Vector3 pos;
            pos = new Vector3(Random.Range(-maxHubDistance, maxHubDistance),transform.position.y, Random.Range(-maxHubDistance, maxHubDistance));
            int iteration = 0;

            while ((Vector3.Distance(transform.position,pos) < minHubDistance || Vector3.Distance(transform.position, pos) > maxHubDistance) || CheckIslandDistance(pos) == false)
            {
                pos = new Vector3(Random.Range(-maxHubDistance, maxHubDistance), transform.position.y, Random.Range(-maxHubDistance, maxHubDistance));
                iteration++;
                if (iteration > 50)
                {
                    Debug.LogError("Probabilities too low");
                    return;
                }
            }
            
            
            islands.Add(Instantiate(islandVariants[Random.Range(0, islandVariants.Count)], pos, Quaternion.identity));
        }
    }

    private bool CheckIslandDistance(Vector3 position)
    {
        for (int i = 0; i < islands.Count; i++)
        {
            if (Vector3.Distance(position,islands[i].transform.position) < minIslandDistance)
            {
                return false;
            }
        }
        return true;
    }
}
