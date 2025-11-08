using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AlienEntry
{
    public GameObject prefab;
    [Range(0f, 1f)] public float probability = 1f;
}

public class AlienSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public Transform[] spawnPoints;

    [Header("Alien Settings")]
    public AlienEntry[] alienEntries;

    [Header("Timing")]
    public float startDelay = 2.0f;
    public float spawnInterval = 5.0f;
    [Tooltip("Интервал между первыми 5 пришельцами (медленный старт)")]
    public float initialSlowSpawnInterval = 8.0f;

    [Header("Wave Settings")]
    public int aliensMax;
    public int aliensSpawned;

    [Header("UI")]
    public Slider progressBar;

    private Coroutine spawnRoutine;

    private void Start()
    {
        if (progressBar != null)
        {
            progressBar.maxValue = aliensMax;
        }

        spawnRoutine = StartCoroutine(SpawnAliensRoutine());
    }

    private void Update()
    {
        if (progressBar != null)
        {
            progressBar.value = aliensSpawned;
        }
    }

    private IEnumerator SpawnAliensRoutine()
    {
        yield return new WaitForSeconds(startDelay);

        while (aliensSpawned < aliensMax)
        {
            SpawnAlien();

            // Для первых 5 пришельцев — более медленный интервал
            if (aliensSpawned <= 5)
                yield return new WaitForSeconds(initialSlowSpawnInterval);
            else
                yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnAlien()
    {
        if (aliensSpawned >= aliensMax) return;
        if (alienEntries.Length == 0 || spawnPoints.Length == 0) return;

        GameObject alienPrefab = GetRandomAlien();
        if (alienPrefab == null) return;

        int spawnIndex = Random.Range(0, spawnPoints.Length);
        var newAlien = Instantiate(alienPrefab, spawnPoints[spawnIndex].position, Quaternion.identity);

        aliensSpawned++;

        // Помечаем последнего
        if (aliensSpawned >= aliensMax && newAlien.TryGetComponent<Alien>(out var alien))
        {
            alien.LastAlien = true;
        }
    }

    private GameObject GetRandomAlien()
    {
        float totalWeight = 0f;
        foreach (var entry in alienEntries)
        {
            totalWeight += Mathf.Max(entry.probability, 0f);
        }

        if (totalWeight <= 0f) return null;

        float randomValue = Random.value * totalWeight;
        float cumulative = 0f;

        foreach (var entry in alienEntries)
        {
            cumulative += entry.probability;
            if (randomValue <= cumulative)
            {
                return entry.prefab;
            }
        }

        // fallback (если что-то пошло не так)
        return alienEntries[alienEntries.Length - 1].prefab;
    }
}
