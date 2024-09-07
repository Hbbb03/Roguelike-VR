using System.Collections.Generic;
using UnityEngine;

public class RandomRoom : MonoBehaviour
{
    public GameObject roomPrefab; // Prefab de la sala
    public GameObject bossPrefab; // Prefab del jefe
    public int numberOfRooms = 5; // Número total de salas a generar
    public GameObject spawnerPrefab;

    private List<Vector3> directions;
    private HashSet<Vector3> occupiedPositions = new HashSet<Vector3>();
    private List<RoomData> generatedRooms = new List<RoomData>();

    private void Start()
    {
        directions = new List<Vector3>
        {
            new Vector3(40f, 0, 0), // Derecha
            new Vector3(-40f, 0, 0), // Izquierda
            new Vector3(0, 0, 40f), // Arriba
            new Vector3(0, 0, -40f)  // Abajo
        };

        GeneratePath();
        PlaceBossInLastRoom();
    }

    void GeneratePath()
    {
        Vector3 currentPosition = Vector3.zero;
        occupiedPositions.Add(currentPosition);
        GameObject firstRoom = Instantiate(roomPrefab, currentPosition, Quaternion.identity);

        Instantiate(spawnerPrefab, firstRoom.transform.position, Quaternion.identity, firstRoom.transform);
        DoorManager doorManager = firstRoom.GetComponent<DoorManager>();
        if (doorManager != null)
        {
            doorManager.roomPositions = occupiedPositions;
        }

        generatedRooms.Add(new RoomData(currentPosition, firstRoom));

        for (int i = 0; i < numberOfRooms - 1; i++)
        {
            Vector3 nextPosition;
            bool validPositionFound = false;

            List<Vector3> shuffledDirections = ShuffleDirections(directions);

            foreach (Vector3 direction in shuffledDirections)
            {
                nextPosition = currentPosition + direction;

                if (!occupiedPositions.Contains(nextPosition) && IsPositionValid(nextPosition))
                {
                    GameObject newRoom = Instantiate(roomPrefab, nextPosition, Quaternion.identity);
                    occupiedPositions.Add(nextPosition);

                    Instantiate(spawnerPrefab, newRoom.transform.position, Quaternion.identity, newRoom.transform);

                    DoorManager newDoorManager = newRoom.GetComponent<DoorManager>();
                    if (newDoorManager != null)
                    {
                        newDoorManager.roomPositions = occupiedPositions;
                    }

                    currentPosition = nextPosition;
                    validPositionFound = true;
                    generatedRooms.Add(new RoomData(nextPosition, newRoom));
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

    void PlaceBossInLastRoom()
    {
        if (generatedRooms.Count > 0)
        {
            RoomData lastRoom = generatedRooms[generatedRooms.Count - 1];
            Instantiate(bossPrefab, lastRoom.position, Quaternion.identity);
            Debug.Log("Jefe generado en la última sala.");
        }
    }

    class RoomData
    {
        public Vector3 position;
        public GameObject roomObject;

        public RoomData(Vector3 pos, GameObject obj)
        {
            position = pos;
            roomObject = obj;
        }
    }

    List<Vector3> ShuffleDirections(List<Vector3> directions)
    {
        List<Vector3> shuffled = new List<Vector3>(directions);
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
        int adjacentCount = 0;
        foreach (Vector3 direction in directions)
        {
            if (occupiedPositions.Contains(position + direction))
            {
                adjacentCount++;
            }
        }
        return adjacentCount <= 1;
    }

    public List<Vector3> GetGeneratedRoomPositions()
    {
        List<Vector3> roomPositions = new List<Vector3>();
        foreach (RoomData room in generatedRooms)
        {
            roomPositions.Add(room.position);
        }
        return roomPositions;
    }

    public List<GameObject> GetGeneratedRoomObjects()
    {
        List<GameObject> roomObjects = new List<GameObject>();
        foreach (RoomData room in generatedRooms)
        {
            roomObjects.Add(room.roomObject);
        }
        return roomObjects;
    }
}
