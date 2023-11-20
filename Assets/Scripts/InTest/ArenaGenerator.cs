using UnityEngine;
using System.Collections.Generic;

public class ArenaGenerator : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public GameObject wallPrefab;
    public GameObject floorPrefab;
    public float wallChance = 0.3f; // Probability of a wall spawning instead of a floor

    public int minDistanceBetweenEnemies = 3;
    public GameObject enemyPrefab;
    private List<Vector3> enemySpawnPoints = new List<Vector3>();
    private int maxEnemies = 10;

    void Start()
    {
        GenerateArena();
    }

    void GenerateArena()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x, 0, y);
                if (Random.value < wallChance)
                {
                    // Spawn a wall
                    Instantiate(wallPrefab, position, Quaternion.identity);
                }
                else
                {
                    // Spawn a floor
                    Instantiate(floorPrefab, position, Quaternion.identity);
                }
            }
        }
        EnsureConnectivity();
        DetermineEnemySpawns();
        PlaceEnemies();
        // Here you could implement additional rules, such as ensuring connectivity between paths
        // or placing special items or enemies.
    }

    //... (previous code)

    void EnsureConnectivity()
    {
        for (int i = 0; i < width; i++)
        {
            int openSpaceX = Random.Range(0, height);
            Vector3 position = new Vector3(i, 0, openSpaceX);
            Instantiate(floorPrefab, position, Quaternion.identity);
        }

        for (int j = 0; j < height; j++)
        {
            int openSpaceY = Random.Range(0, width);
            Vector3 position = new Vector3(openSpaceY, 0, j);
            Instantiate(floorPrefab, position, Quaternion.identity);
        }
    }


    void PlaceEnemies()
    {
        foreach (var point in enemySpawnPoints)
        {
            Instantiate(enemyPrefab, point, Quaternion.identity);
        }
    }

    void DetermineEnemySpawns()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 potentialSpawnPoint = new Vector3(x, 0, y);
                bool tooClose = false;

                foreach (var point in enemySpawnPoints)
                {
                    if (Vector3.Distance(potentialSpawnPoint, point) < minDistanceBetweenEnemies)
                    {
                        tooClose = true;
                        break;
                    }
                }

                if (!tooClose)
                {
                    enemySpawnPoints.Add(potentialSpawnPoint);
                }
            }
        }

        // Shuffle the list and then choose a subset for actual spawn points
        enemySpawnPoints = ShuffleList(enemySpawnPoints);
        enemySpawnPoints = enemySpawnPoints.GetRange(0, Mathf.Min(enemySpawnPoints.Count, maxEnemies));
    }

    List<Vector3> ShuffleList(List<Vector3> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Vector3 temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        return list;
    }

    // Add these at the end of GenerateArena method
    



}
