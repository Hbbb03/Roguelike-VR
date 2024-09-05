using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public List<GameObject> powerUpPrefabs; // Lista de prefabs de power-ups
    public Transform spawnPoint; // Punto desde donde se generan los power-ups
    private bool isOpened = false;

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && !isOpened)
        {
            OpenChest();
        }
    }

    void OpenChest()
    {
        isOpened = true;

        foreach (GameObject powerUpPrefab in powerUpPrefabs)
        {
            Vector3 randomOffset = Random.insideUnitSphere * 0.5f; // Ajusta el valor para controlar la dispersión
            randomOffset.y = 0; // Mantener los power-ups en el suelo
            Instantiate(powerUpPrefab, spawnPoint.position + randomOffset, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
