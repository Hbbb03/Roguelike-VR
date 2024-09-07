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
    public float rotationSpeed = 10f;

    public float life = 5f;

    // Variables para disparo
    public Transform firePoint;  // Punto de origen del disparo
    public GameObject bulletPrefab;  // Prefab del proyectil
    public float bulletSpeed = 20f;  // Velocidad del proyectil
    public Rigidbody rb;

    private Camera cameraComponent;

    New_Input_System inputSystem;
    Vector2 dir = Vector2.zero;
    //Animaciones
    public Animator animator;

    public TextMeshProUGUI lives;
    public TextMeshProUGUI speedText;   
    public TextMeshProUGUI damageText;

    private void Awake()
    {
        inputSystem = new New_Input_System();
        inputSystem.Player.Movement.performed += ctx => dir = ctx.ReadValue<Vector2>();
        inputSystem.Player.Movement.canceled += ctx => dir =   Vector2.zero;

        inputSystem.Player.Shoot.performed += ctx => Shoot();
        
    }

    void OnEnable()
    {
        inputSystem.Enable();
    }

    void OnDisable()
    {
        inputSystem.Disable();
        
    }

     void Start()
    {

        // Calcular el offset inicial entre la cámara y el jugador
        cameraOffset = cameraTransform.position - transform.position;

        // Obtener el componente Camera del cameraTransform
        cameraComponent = cameraTransform.GetComponent<Camera>();

        if (cameraComponent == null)
        {
            Debug.LogError("El cameraTransform no tiene un componente Camera.");
        }

        
        UpdateStatsUI();
    }

    private void Update()
    {

        Movement();
        UpdateStatsUI();

        // Hacer que la cámara siga al jugador manteniendo el offset
        cameraTransform.position = transform.position + cameraOffset;
        //Attack();

        //// Detectar clic izquierdo para disparar
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Shoot();
        //    animator.Play("Shoot");
        //}
        //if (Input.GetKey(KeyCode.F))
        //{
        //    animator.Play("Dance");
        //}


    }

    void Attack()
    {
        // Hacer que la cámara siga al jugador manteniendo el offset
        cameraTransform.position = transform.position + cameraOffset;

        // Detectar clic izquierdo para disparar
        if (Input.GetMouseButtonUp(0)) 
        {
            Shoot();
            animator.Play("Shoot");
        }
        if (Input.GetKey(KeyCode.F))
        {
            animator.Play("Dance");
        }
    }
    void Movement()
    {
        // Variables de movimiento basadas en el joystick
        Vector3 movement = new Vector3(dir.x, 0, dir.y);

        // Convertir el movimiento a las coordenadas del mundo relativo a la cámara
        Vector3 moveDirection = cameraTransform.TransformDirection(movement);

        // Mantener el movimiento en el plano horizontal (Y=0)
        moveDirection.y = 0;

        // Normalizar el vector de movimiento para asegurar velocidad constante
        moveDirection = moveDirection.normalized;

        // Aplicar la velocidad al Rigidbody
        rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);

        // Si hay movimiento, rotar el jugador hacia la dirección del movimiento
        if (moveDirection != Vector3.zero)
        {
            // Calcular la rotación objetivo
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);

            // Interpolar suavemente hacia la rotación objetivo
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        // Actualizar parámetros de animación (si es necesario)
        animator.SetFloat("Velx", moveDirection.x);
        animator.SetFloat("Vely", moveDirection.z);
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
        // Dirección de disparo basada en la rotación actual del jugador
        Vector3 shootDirection = transform.forward;

        // Instanciar el proyectil en la posición del firePoint
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

        // Ejecutar la animación de disparo
        animator.Play("Shoot");
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

    void UpdateStatsUI()
    {
        speedText.text = "Velocidad: " + moveSpeed.ToString("F1");
        damageText.text = "Daño: " + bulletSpeed.ToString("F1");
    }
}
