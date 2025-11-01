using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;

    public GameObject[] alienPrefabs;

    public float startDelay = 2.0f;
    public float spawnInterval = 5.0f;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnAlien), startDelay, spawnInterval);
    }

    void SpawnAlien()
    {
        if (alienPrefabs.Length == 0 || spawnPoints.Length == 0)
        {
            return;
        }

        int alienIndex = Random.Range(0, alienPrefabs.Length);

        int spawnIndex = Random.Range(0, spawnPoints.Length);

        GameObject newAlien = Instantiate(
            alienPrefabs[alienIndex],
            spawnPoints[spawnIndex].position,
            Quaternion.identity
        );
    }
}
