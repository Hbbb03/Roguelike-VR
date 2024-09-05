using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public HashSet<Vector3> roomPositions; // Lista de posiciones ocupadas
    public GameObject upDoor, downDoor, leftDoor, rightDoor; // Referencias a las puertas

    private void Start()
    {
        // Asegurarse de que roomPositions no sea nulo
        if (roomPositions != null)
        {
            Debug.Log("RoomPositions ha sido asignado correctamente para la sala: " + gameObject.name);
            CheckDoors(); // Revisar las puertas
        }
        else
        {
            Debug.LogError("roomPositions es nulo en " + gameObject.name);
        }

        // Debug para verificar si las puertas han sido asignadas correctamente
        if (upDoor == null) Debug.LogError("La puerta Up no está asignada en " + gameObject.name);
        if (downDoor == null) Debug.LogError("La puerta Down no está asignada en " + gameObject.name);
        if (leftDoor == null) Debug.LogError("La puerta Left no está asignada en " + gameObject.name);
        if (rightDoor == null) Debug.LogError("La puerta Right no está asignada en " + gameObject.name);
    }

    void CheckDoors()
    {
        Vector3 currentPosition = transform.position;

        Debug.Log("Revisando puertas para la sala en posición: " + currentPosition);

        // Revisar cada dirección (arriba, abajo, izquierda, derecha)
        if (!roomPositions.Contains(currentPosition + new Vector3(0, 0, 40))) // Arriba
        {
            Debug.Log("Activando puerta Up para " + gameObject.name);
            if (upDoor != null) upDoor.SetActive(true); // Activar puerta arriba
        }
        else
        {
            Debug.Log("Desactivando puerta Up para " + gameObject.name);
            if (upDoor != null) upDoor.SetActive(false); // Desactivar si hay una sala
        }

        if (!roomPositions.Contains(currentPosition + new Vector3(0, 0, -40))) // Abajo
        {
            Debug.Log("Activando puerta Down para " + gameObject.name);
            if (downDoor != null) downDoor.SetActive(true); // Activar puerta abajo
        }
        else
        {
            Debug.Log("Desactivando puerta Down para " + gameObject.name);
            if (downDoor != null) downDoor.SetActive(false); // Desactivar si hay una sala
        }

        if (!roomPositions.Contains(currentPosition + new Vector3(-40, 0, 0))) // Izquierda
        {
            Debug.Log("Activando puerta Left para " + gameObject.name);
            if (leftDoor != null) leftDoor.SetActive(true); // Activar puerta izquierda
        }
        else
        {
            Debug.Log("Desactivando puerta Left para " + gameObject.name);
            if (leftDoor != null) leftDoor.SetActive(false); // Desactivar si hay una sala
        }

        if (!roomPositions.Contains(currentPosition + new Vector3(40, 0, 0))) // Derecha
        {
            Debug.Log("Activando puerta Right para " + gameObject.name);
            if (rightDoor != null) rightDoor.SetActive(true); // Activar puerta derecha
        }
        else
        {
            Debug.Log("Desactivando puerta Right para " + gameObject.name);
            if (rightDoor != null) rightDoor.SetActive(false); // Desactivar si hay una sala
        }
    }
}
