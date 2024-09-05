using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSpawner : MonoBehaviour
{
    public List<GameObject> chestPrefabs;  // Lista de prefabs de cofres
    public int numberOfChests = 3;         // Número de cofres a generar
    public float spawnInterval = 2f;       // Intervalo entre cada generación
    public Transform spawnAreaMin;         // Transform del límite inferior del área de spawn
    public Transform spawnAreaMax;         // Transform del límite superior del área de spawn

    private int chestsSpawned = 0;

    void Start()
    {
        StartCoroutine(SpawnChests());
    }

    IEnumerator SpawnChests()
    {
        while (chestsSpawned < numberOfChests)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();

            // Selecciona un cofre aleatorio de la lista
            GameObject randomChestPrefab = GetRandomChestPrefab();

            // Genera el cofre en la posición calculada
            Instantiate(randomChestPrefab, spawnPosition, Quaternion.identity);
            chestsSpawned++;

            // Espera el intervalo de spawn
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        // Calcula una posición aleatoria usando los Transforms para el área de spawn
        float randomX = Random.Range(spawnAreaMin.position.x, spawnAreaMax.position.x);
        float randomY = Random.Range(spawnAreaMin.position.y, spawnAreaMax.position.y);
        float randomZ = Random.Range(spawnAreaMin.position.z, spawnAreaMax.position.z);

        return new Vector3(randomX, randomY, randomZ);
    }

    GameObject GetRandomChestPrefab()
    {
        int randomIndex = Random.Range(0, chestPrefabs.Count);
        return chestPrefabs[randomIndex];
    }

}
