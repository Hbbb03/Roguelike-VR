using System.Collections.Generic;
using UnityEngine;

public class RandomRoom : MonoBehaviour
{
    public GameObject roomPrefab; // Prefab de la sala
    public int numberOfRooms = 5; // Número total de salas a generar

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
            bool validPositionFound = false;

            // Barajar direcciones para asegurar la aleatoriedad
            List<Vector3> shuffledDirections = ShuffleDirections(directions);

            // Intentar encontrar una posición válida adyacente
            foreach (Vector3 direction in shuffledDirections)
            {
                nextPosition = currentPosition + direction;

                if (!occupiedPositions.Contains(nextPosition) && IsPositionValid(nextPosition))
                {
                    // Instanciar la sala en la nueva posición válida
                    Instantiate(roomPrefab, nextPosition, Quaternion.identity);
                    occupiedPositions.Add(nextPosition);

                    // Actualizar la posición actual
                    currentPosition = nextPosition;
                    validPositionFound = true;
                    break;
                }
            }

            if (!validPositionFound)
            {
                Debug.LogWarning("No se encontró una posición válida para la siguiente sala.");
                break;
            }
        }
    }

    List<Vector3> ShuffleDirections(List<Vector3> directions)
    {
        // Copiar la lista original
        List<Vector3> shuffled = new List<Vector3>(directions);

        // Barajar la lista utilizando el algoritmo Fisher-Yates
        for (int i = shuffled.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Vector3 temp = shuffled[i];
            shuffled[i] = shuffled[j];
            shuffled[j] = temp;
        }

        return shuffled;
    }

    bool IsPositionValid(Vector3 position)
    {
        // Comprobar las posiciones adyacentes
        int adjacentCount = 0;

        foreach (Vector3 direction in directions)
        {
            if (occupiedPositions.Contains(position + direction))
            {
                adjacentCount++;
            }
        }

        // Si la posición tiene más de una sala adyacente, no es válida
        return adjacentCount <= 1;
    }
}
