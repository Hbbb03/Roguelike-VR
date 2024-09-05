using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSpawner : MonoBehaviour
{
    public List<GameObject> chestPrefabs;  // Lista de prefabs de cofres
    public int numberOfChests = 3;         // N�mero de cofres a generar
    public float spawnInterval = 2f;       // Intervalo entre cada generaci�n
    public Transform spawnAreaMin;         // Transform del l�mite inferior del �rea de spawn
    public Transform spawnAreaMax;         // Transform del l�mite superior del �rea de spawn

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

            // Genera el cofre en la posici�n calculada
            Instantiate(randomChestPrefab, spawnPosition, Quaternion.identity);
            chestsSpawned++;

            // Espera el intervalo de spawn
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        // Calcula una posici�n aleatoria usando los Transforms para el �rea de spawn
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
