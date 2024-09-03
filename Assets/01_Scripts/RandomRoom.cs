using System.Collections.Generic;
using UnityEngine;

public class RandomRoom : MonoBehaviour
{
    public GameObject roomPrefab; // Prefab del cuadrado
    public int numberOfRooms = 5; // N�mero total de salas a generar

    public Transform centerPoint; // Punto vac�o en el centro de la sala

    private List<Vector3> directions;
    private HashSet<Vector3> occupiedPositions = new HashSet<Vector3>();

    private void Start()
    {
        // Inicializar las direcciones en funci�n de la distancia entre salas
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

            // Intentar encontrar una posici�n v�lida adyacente
            do
            {
                nextPosition = GetNextRoomPosition(currentPosition);
            }
            while (occupiedPositions.Contains(nextPosition)); // Repetir hasta encontrar una posici�n libre

            // Instanciar la sala en la nueva posici�n v�lida
            Instantiate(roomPrefab, nextPosition, Quaternion.identity);
            occupiedPositions.Add(nextPosition);

            // Actualizar la posici�n actual
            currentPosition = nextPosition;
        }
    }

    Vector3 GetNextRoomPosition(Vector3 currentPosition)
    {
        // Elegir una direcci�n aleatoria
        Vector3 randomDirection = directions[Random.Range(0, directions.Count)];

        // Calcular la nueva posici�n en base a la direcci�n
        Vector3 newPosition = currentPosition + randomDirection;

        return newPosition;
    }
}
