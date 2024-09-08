using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 5f;  // Tiempo de vida del proyectil antes de destruirse
    public float bulletSpeed = 20f;  // Velocidad del proyectil
    private Vector3 direction;  // Direcci�n en la que el proyectil se mover�
    public bool playerBullet = false;
    public float damage = 1f;

    public float detectionRadius = 20f; // Radio en el que buscar� enemigos
    public float turnSpeed = 2f; // Velocidad de giro hacia el objetivo

    private Transform target;

    // Este m�todo ser� llamado desde el PlayerShooting al instanciar la bala
    public void SetDirection(Vector3 shootDirection)
    {
        direction = shootDirection.normalized;
    }

    void Start()
    {
        searchEnemy();
        // Destruir el proyectil despu�s de un tiempo para evitar que quede flotando
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Si no hay target, mover la bala recto
        if (target == null)
        {
            // Mover el proyectil en la direcci�n original
            transform.position += direction * bulletSpeed * Time.deltaTime;
            searchEnemy(); // Buscar un enemigo
        }
        else
        {
            // Si hay un objetivo, dirigir la bala hacia �l
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            // Rotar la bala hacia el objetivo
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, directionToTarget, turnSpeed * Time.deltaTime, 0f);
            transform.rotation = Quaternion.LookRotation(newDirection);

            // Mover la bala hacia adelante
            transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
        }
    }

    void searchEnemy()
    {
        // Buscar enemigos dentro del radio de detecci�n
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Enemy") || hit.CompareTag("Boss"))
            {
                target = hit.transform; // Fijar el primer objetivo encontrado
                break;
            }
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (playerBullet && collision.gameObject.CompareTag("Enemy"))
        {
            Enemy e = collision.gameObject.GetComponent<Enemy>();
            e?.TakeDamage(damage); // L�gica de da�o al enemigo
            Destroy(gameObject);
            Debug.Log("Enemigo da�ado");
        }
        else if (playerBullet && collision.gameObject.CompareTag("Boss"))
        {
            Boss b = collision.gameObject.GetComponent<Boss>();
            b?.TakeDamage(damage); // L�gica de da�o al jefe
            Destroy(gameObject);
            Debug.Log("Jefe da�ado");
        }
        else if (!playerBullet && collision.gameObject.CompareTag("Player"))
        {
            Player p = collision.gameObject.GetComponent<Player>();
            p?.TakeDamage(damage); // L�gica de da�o al jugador
            Destroy(gameObject);
            Debug.Log("Jugador da�ado");
        }
    }
}
