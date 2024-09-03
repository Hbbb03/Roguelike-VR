using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 5f;  // Tiempo de vida del proyectil antes de destruirse
    public float bulletSpeed = 20f;  // Velocidad del proyectil
    private Vector3 direction;  // Dirección en la que el proyectil se moverá
    public bool playerBullet = false;
     public float damage = 1f;
    // Este método será llamado desde el PlayerShooting al instanciar la bala
    public void SetDirection(Vector3 shootDirection)
    {
        direction = shootDirection.normalized;
    }

    void Start()
    {
        // Destruir el proyectil después de un tiempo para evitar que quede flotando
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Mover el proyectil en la dirección especificada
        transform.position += direction * bulletSpeed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (playerBullet && collision.gameObject.CompareTag("Enemy"))
        {
            Enemy e = collision.gameObject.GetComponent<Enemy>();
            e.TakeDamage(damage);
            Destroy(gameObject);
            Debug.Log("enemigo dañado");
        }
        else if(playerBullet && collision.gameObject.CompareTag("Boss"))
        {
            Boss b = collision.gameObject.GetComponent<Boss>();
            b.TakeDamage(damage);
            Destroy(gameObject);
            Debug.Log("jefe dañado");
        }
        else if (!playerBullet && collision.gameObject.CompareTag("Player"))
        {
            Player p = collision.gameObject.GetComponent<Player>();
            p.TakeDamage(damage);
            Destroy(gameObject);
            Debug.Log("jugador dañado");
        }
    }
}
