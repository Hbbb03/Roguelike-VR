using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    public List<GameObject> enemyPrefabs;
    public int maxEnemies = 10;
    public float spawnInterval = 2f;
    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public GameObject bossPrefab;
    private int enemiesSpawned = 0;
    private int enemiesKilled = 0;
    private bool bossSpawned = false;
        
    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (enemiesSpawned < maxEnemies || !bossSpawned)
        {
            if (enemiesSpawned < maxEnemies)
            {
                Vector3 spawnPosition = GetRandomSpawnPosition();

                if (NavMesh.SamplePosition(spawnPosition, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
                {
                    GameObject randomEnemyPrefab = GetRandomEnemyPrefab();
                    Instantiate(randomEnemyPrefab, hit.position, Quaternion.identity);
                    enemiesSpawned++;
                }
                else
                {
                    Debug.LogWarning("No se encontró una posición válida en el NavMesh para generar el enemigo.");
                }
            }

            if (!bossSpawned && enemiesKilled >= 20)
            {
                SpawnBoss();
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }   

    Vector3 GetRandomSpawnPosition()
    {
        float randomX = Random.Range(spawnPoint1.position.x, spawnPoint2.position.x);
        float randomZ = Random.Range(spawnPoint1.position.z, spawnPoint2.position.z);
        return new Vector3(randomX, spawnPoint1.position.y, randomZ);
    }

    GameObject GetRandomEnemyPrefab()
    {
        int randomIndex = Random.Range(0, enemyPrefabs.Count);
        return enemyPrefabs[randomIndex];
    }

    void SpawnBoss()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();

        if (NavMesh.SamplePosition(spawnPosition, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
        {
            Instantiate(bossPrefab, hit.position, Quaternion.identity);
            bossSpawned = true;
            Debug.Log("Jefe generado!");
        }
        else
        {
            Debug.LogWarning("No se encontró una posición válida en el NavMesh para generar el jefe.");
        }
    }

    public void EnemyKilled()
    {
        enemiesKilled++;
    }
}
