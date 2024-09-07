using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{
    public EnemyType enemyType;

    public string playerTag = "Player";
    private Transform player;
    public float attackRange = 2f;
    public float detectionRange = 10f;
    public float retreatDistance = 5f;
    public float moveSpeed = 3.5f;
    public float rotationSpeed = 5f;
    public Transform firePoint;
    public GameObject bulletPrefab;
    private NavMeshAgent agent;
    private bool canAttack = true;
    private float attackTimer = 0f;
    public float attackCooldown = 3f;
    public float life = 3f;
    public float damage = 1f;

    public float kamikazeDetectionRange = 5f;
    public float kamikazeMoveSpeed = 6f;

    public Animator animator;


    public float meleeDetectionRange = 15f; // Rango de detección más amplio para el enemigo Melee
    public float meleeAttackRange = 1.5f; // Rango de ataque del enemigo Melee
    public float meleeAttackCooldown = 2f; // Tiempo entre cada golpe del enemigo Melee
    private float meleeAttackTimer = 0f;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag(playerTag).transform;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
    }

    void Update()
    {
        switch (enemyType)
        {
            case EnemyType.Normal:
                Normal();
                break;
            case EnemyType.Kamikaze:
                Kamikaze();
                break;
            case EnemyType.Melee:
                Melee();
                break;
        }
    }

    void Normal()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            if (distanceToPlayer <= retreatDistance)
            {
                CircleAndAttackPlayer();
            }
        }
       
    }

    void Kamikaze()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= kamikazeDetectionRange)
        {
            ChaseAndExplode();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (!agent.hasPath)
        {
            Vector3 randomDirection = Random.insideUnitSphere * detectionRange;
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, detectionRange, 1);
            Vector3 finalPosition = hit.position;

            agent.SetDestination(finalPosition);
        }
    }

    void ChaseAndExplode()
    {
        agent.speed = kamikazeMoveSpeed;
        agent.SetDestination(player.position);

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            Explode();
        }
    }

    void Explode()
    {

        Debug.Log("¡Kamikaze explota!");
        Destroy(gameObject);
    }

    void Shoot()
    {
        GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Calcular la dirección hacia el jugador
        Vector3 shootDirection = (player.position - firePoint.position).normalized;

        // Asignar la dirección a la bala
        Bullet bulletScript = bulletInstance.GetComponent<Bullet>();
        bulletScript.SetDirection(shootDirection);

        Debug.Log("Disparando bala...");
    }

    void CircleAndAttackPlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        Vector3 perpendicularDirection = Vector3.Cross(directionToPlayer, Vector3.up).normalized;
        Vector3 circlingPosition = player.position + perpendicularDirection * retreatDistance;

        agent.SetDestination(circlingPosition);

        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown)
        {
            Debug.Log("Apuntando bala...");
            Shoot();
            attackTimer = 0f;
        }
    }

    void Melee()
    {
        animator.SetBool("Walk", true);
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Si el jugador está dentro del rango de detección
        if (distanceToPlayer <= meleeDetectionRange)
        {
            // Moverse hacia el jugador
            agent.SetDestination(player.position);
            animator.SetFloat("Velx", distanceToPlayer);
            animator.SetFloat("Vely", distanceToPlayer);


            // Cuando el enemigo esté dentro del rango de ataque
            if (distanceToPlayer <= meleeAttackRange)
            {
                agent.isStopped = true;
                animator.SetBool("Walk", false);

                // Temporizador para ataques
                meleeAttackTimer += Time.deltaTime;

                if (meleeAttackTimer >= meleeAttackCooldown)
                {
                    AttackPlayer();
                    meleeAttackTimer = 0f;  // Reiniciar el temporizador
                }
            }
            else
            {
                animator.SetBool("Walk", true);
                agent.isStopped = false;
            }
        }
     
    }

    void AttackPlayer()
    {
        // Si el jugador está en rango de ataque
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= meleeAttackRange)
        {
            // Obtener el componente Player del jugador
            Player p = player.GetComponent<Player>();

            if (p != null)
            {
                // Aplicar daño al jugador
                animator.SetTrigger("Attack");
                p.TakeDamage(damage);
                Debug.Log("El enemigo Melee ha atacado al jugador, causando " + damage + " de daño.");

           
            }
        }
    }

    public void TakeDamage(float damage)
    {
        life -= damage;
        if (life <= 0)
        {
            //animator.SetTrigger("Die");
            Destroy(gameObject);
        }
    }

    

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player p = collision.gameObject.GetComponent<Player>();
            p.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    public enum EnemyType
    {
        Normal,
        Kamikaze,
        Melee
    }

}
