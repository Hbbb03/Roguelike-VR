using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 100f;
    public Transform cameraTransform; // Cámara asignada en el Inspector
    private Vector3 cameraOffset; // Offset de la cámara respecto al jugador
    public float maxLife = 5f;

    public float life = 5f;

    // Variables para disparo
    public Transform firePoint;  // Punto de origen del disparo
    public GameObject bulletPrefab;  // Prefab del proyectil
    public float bulletSpeed = 20f;  // Velocidad del proyectil

    private Camera cameraComponent;

    //Animaciones
    public Animator animator;

    public TextMeshProUGUI lives;

    private void Start()
    {
        // Calcular el offset inicial entre la cámara y el jugador
        cameraOffset = cameraTransform.position - transform.position;

        // Obtener el componente Camera del cameraTransform
        cameraComponent = cameraTransform.GetComponent<Camera>();

        if (cameraComponent == null)
        {
            Debug.LogError("El cameraTransform no tiene un componente Camera.");
        }
    }

    private void Update()
    {
        // Movimiento del jugador
        float moveForward = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        float moveSideways = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        
        transform.Translate(moveSideways, 0, moveForward);

        // Controlar la rotación del jugador con el mouse

        // Hacer que la cámara siga al jugador manteniendo el offset
        cameraTransform.position = transform.position + cameraOffset;

        // Detectar clic izquierdo para disparar
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
            animator.Play("Shoot");
        }
        if (Input.GetKey(KeyCode.F))
        {
            animator.Play("Dance");
        }

        animator.SetFloat("Velx", moveSideways);
        animator.SetFloat("Vely", moveForward);

    }

    void fillLives()
    {

        string myLives = "";

        for (int i = 0; i < life; i++)
        {
            myLives += '♥';
        }

        lives.text = myLives;

    }

    void Shoot()
    {
        // Obtener la posición del mouse en la pantalla
        Ray ray = cameraComponent.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Hacer un raycast desde la cámara hacia la posición del mouse
        if (Physics.Raycast(ray, out hit))
        {
            // Dirección desde el firepoint hacia el punto impactado por el raycast
            Vector3 shootDirection = (hit.point - firePoint.position).normalized;

            // Instanciar el proyectil
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            // Darle velocidad al proyectil en la dirección calculada
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Asegurarse de que el Rigidbody es cinemático esté desactivado
                rb.isKinematic = false;

                // Aplicar la velocidad en la dirección correcta
                rb.velocity = shootDirection * bulletSpeed;
            }
            else
            {
                Debug.LogError("El proyectil no tiene un componente Rigidbody.");
            }
        }
    }

    public void TakeDamage(float damage)
    {
        life -= damage;
        fillLives();
        if (life <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

}
