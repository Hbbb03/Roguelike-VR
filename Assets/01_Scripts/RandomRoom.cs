using System.Collections.Generic;
using UnityEngine;

public class RandomRoom : MonoBehaviour
{
    public GameObject roomPrefab; // Prefab del cuadrado
    public int numberOfRooms = 5; // Número total de salas a generar

    public Transform centerPoint; // Punto vacío en el centro de la sala

    private List<Vector3> directions;
    private HashSet<Vector3> occupiedPositions = new HashSet<Vector3>();

    private void Start()
    {
        // Inicializar las direcciones en función de la distancia entre salas
        directions = new List<Vector3>
        {
            new Vector3(40f, 0, 0), // Derecha
            new Vector3(-40f, 0, 0), // Izquierda
            new Vector3(0, 0, 40f), // Arriba
            new Vector3(0, 0, -40f)  // Abajo
        };

        GeneratePath();
    }

    void GeneratePath()
    {
        // Comenzar en el origen
        Vector3 currentPosition = Vector3.zero;
        occupiedPositions.Add(currentPosition);
        Instantiate(roomPrefab, currentPosition, Quaternion.identity);

        for (int i = 0; i < numberOfRooms - 1; i++)
        {
            Vector3 nextPosition;

            // Intentar encontrar una posición válida adyacente
            do
            {
                nextPosition = GetNextRoomPosition(currentPosition);
            }
            while (occupiedPositions.Contains(nextPosition)); // Repetir hasta encontrar una posición libre

            // Instanciar la sala en la nueva posición válida
            Instantiate(roomPrefab, nextPosition, Quaternion.identity);
            occupiedPositions.Add(nextPosition);

            // Actualizar la posición actual
            currentPosition = nextPosition;
        }
    }

    Vector3 GetNextRoomPosition(Vector3 currentPosition)
    {
        // Elegir una dirección aleatoria
        Vector3 randomDirection = directions[Random.Range(0, directions.Count)];

        // Calcular la nueva posición en base a la dirección
        Vector3 newPosition = currentPosition + randomDirection;

        return newPosition;
    }
}
