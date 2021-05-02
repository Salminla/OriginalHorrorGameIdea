using System.Collections.Generic;
using UnityEngine;

class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform spawnContainer;
        
    List<Transform> spawnPoints = new List<Transform>();
        
    private void GetSpawnPoints()
    {
        if (spawnContainer != null)
        {
            for (int i = 0; i < spawnContainer.childCount; i++)
            {
                spawnPoints.Add(spawnContainer.GetChild(i));
            }
        }
        else
            spawnPoints.Add(transform);

    }
    private Player SpawnPlayer(Vector3 pos)
    {
        GameObject temp = Instantiate(playerPrefab, pos + Vector3.up, Quaternion.identity);
        return temp.GetComponent<Player>();
    }
    private Monster SpawnMonster(Vector3 pos)
    {
        return Instantiate(monsterPrefab, pos + Vector3.up, Quaternion.identity).GetComponent<Monster>();
    }

    public void HandleSpawning()
    {
        GetSpawnPoints();
        Transform playerSpawn = spawnPoints[Random.Range(0, spawnPoints.Count)];
        Transform monsterSpawn = spawnPoints[Random.Range(0, spawnPoints.Count)];
        while (playerSpawn == monsterSpawn)
            playerSpawn = spawnPoints[Random.Range(0, spawnPoints.Count)];
        if (playerPrefab != null)
            SpawnPlayer(playerSpawn.position);
        if (monsterPrefab != null)
            SpawnMonster(monsterSpawn.position);
    }
}