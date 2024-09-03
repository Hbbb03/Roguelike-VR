using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    private Transform player;
    public string playerTag = "Player";
    public float shootingRange = 15f;
    public float meleeRange = 3f;
    public float retreatDistance = 7f;
    public float moveSpeed = 3.5f;
    public float rotationSpeed = 5f;
    public float attackCooldown = 2f;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float life = 10f;
    public float damage = 2f;

    private NavMeshAgent agent;
    private bool canAttack = true;
    private int meleeAttackCount = 0;

    public int minBullets = 10; // Número mínimo de balas antes de esperar
    public int maxBullets = 15; // Número máximo de balas antes de esperar
    public float shootCooldown = 2f; // Tiempo de espera antes de disparar nuevamente

    private int bulletsShot = 0;
    private float shootTimer = 0f; // Temporizador para controlar el tiempo de espera
    private bool isCoolingDown = false;

    public Animator animator;

    public float meleeAttackCooldown = 2f; // Tiempo entre ataques cuerpo a cuerpo
    private float meleeAttackTimer = 0f; // Temporizador para ataques cuerpo a cuerpo

    void Start()
    {
        player = GameObject.FindGameObjectWithTag(playerTag).transform;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        animator.SetFloat("Velx", distanceToPlayer);
        animator.SetFloat("Vely", distanceToPlayer);

        if (distanceToPlayer > meleeRange && distanceToPlayer <= shootingRange)
        {
            HandleShooting();
            animator.Play("Shoot");
        }
        else if (distanceToPlayer <= meleeRange)
        {
            MeleeAttack();
            animator.Play("Shoot");
        }
        else if (distanceToPlayer > shootingRange)
        {
            agent.SetDestination(player.position);
        }

        if (isCoolingDown)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootCooldown)
            {
                isCoolingDown = false;
                bulletsShot = 0; // Reiniciar el contador de balas
                shootTimer = 0f; // Reiniciar el temporizador
            }
        }

        // Manejo del temporizador para ataques cuerpo a cuerpo
        if (meleeAttackTimer > 0f)
        {
            meleeAttackTimer -= Time.deltaTime;
        }
    }

    void HandleShooting()
    {
        if (!isCoolingDown)
        {
            if (bulletsShot < Random.Range(minBullets, maxBullets))
            {
                Shoot();
                bulletsShot++;
            }
            else
            {
                isCoolingDown = true;
            }
        }
    }

    void Shoot()
    {
        // Instanciar la bala en el firePoint del jefe
        GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Calcular la dirección hacia el jugador
        Vector3 shootDirection = (player.position - firePoint.position).normalized;

        // Asignar la dirección a la bala
        Bullet bulletScript = bulletInstance.GetComponent<Bullet>();
        bulletScript.SetDirection(shootDirection);

        Debug.Log("Jefe disparando bala...");
    }

    void MeleeAttack()
    {
        if (canAttack && meleeAttackTimer <= 0f)
        {
            // Lógica de ataque cuerpo a cuerpo
            Debug.Log("Jefe atacando cuerpo a cuerpo...");
            Player p = player.GetComponent<Player>();

            if (p != null)
            {
                p.TakeDamage(damage);
                Debug.Log("Jefe ha causado " + damage + " de daño al jugador.");
                animator.Play("Shoot");
            }

            meleeAttackCount++;
            meleeAttackTimer = meleeAttackCooldown;

            if (meleeAttackCount >= 2)
            {
                Retreat();
                meleeAttackCount = 0;
            }

            canAttack = false;
            StartCoroutine(ResetAttackCooldown());
        }

    }

    void Retreat()
    {
        Vector3 retreatDirection = (transform.position - player.position).normalized;
        Vector3 retreatPosition = transform.position + retreatDirection * retreatDistance;

        agent.SetDestination(retreatPosition);
        Debug.Log("Jefe se retira...");
    }

    public void TakeDamage(float damage)
    {
        life -= damage;
        if (life <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Jefe derrotado");
        }
    }

    IEnumerator ResetAttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player p = collision.gameObject.GetComponent<Player>();
            p.TakeDamage(damage);
        }
    }
}
