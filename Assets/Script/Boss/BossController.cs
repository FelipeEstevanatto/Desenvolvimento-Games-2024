using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    [Header("Boss Settings")]
    [SerializeField] private float maxHealth = 900f;
    [SerializeField] private int scoreValue = 100;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float minWaitTime = 5f;
    [SerializeField] private float maxWaitTime = 12f;
    [SerializeField] private float patrolDistance = 2f;
    [SerializeField] private float launchAngle = 30f;
    [SerializeField] private float fireRate = 1.2f;
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private GameObject explodeAnt;
    [SerializeField] private float collisionDamage = 15f;

    [Header("Boss Components")]
    [SerializeField] private BossBattle bossBattle;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Transform[] patrolPoints;

    private float currentHealth;
    private int currentPoint = 0;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private bool isWaiting = false;
    private bool facingRight = true;
    private Transform firePoint;
    private Transform antSpawnPoint;
    private float nextFireTime = 0f;
    private PlayerController player;
    private Animator anim;


    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        player = FindAnyObjectByType<PlayerController>();
        anim = GetComponent<Animator>();

        foreach (Transform pPoint in patrolPoints)
        {
            pPoint.SetParent(null);
        }
        firePoint = transform.Find("FirePoint");
        antSpawnPoint = transform.Find("AntSpawnPoint");
    }

    private void Update()
    {
        Patrol();

        if (isWaiting) // Verifica se já passou o tempo para atirar
        {
            if (Time.time >= nextFireTime)
            {
                if (currentHealth >= maxHealth / 2)
                {
                    Fire();
                }
                else 
                { 
                    // Definir uma probabilidade de 70% para spawnar bombas e 30% para spawnar formigas
                    float spawnChance = Random.value;  // Gera um valor entre 0 e 1

                    if (spawnChance <= 0.6f)
                    {
                        // Spawn a bomba
                        Fire();
                    }
                    else
                    {
                        // Spawn a formiga
                        if (antSpawnPoint != null)
                        {
                            SpawnAnts();
                        }
                    }
                }
                nextFireTime = Time.time + fireRate; // Set fire rate correctly
            }
        }
    }



    private void Patrol()
    {
        // Verifica se o inimigo está perto do ponto atual
        if (Vector2.Distance(transform.position, patrolPoints[currentPoint].position) < patrolDistance)
        {
            if (!isWaiting)
            {
                StartCoroutine(WaitAtPatrolPoint());
            }
            else
            {
                // Aqui fazemos o flip, mas só se o boss estiver esperando
                CheckFlip();
            }
        }
        else
        {
            // Checa o flip e move em direção ao ponto
            MoveTowardsTarget();
        }
    }


    private void CheckFlip()
    {
        // Verifica a direção para o próximo ponto de patrulha
        if ((currentPoint == 0 && facingRight) || (currentPoint == 1 && !facingRight))
        {
            Flip();
        }
    }

    private IEnumerator WaitAtPatrolPoint()
    {
        isWaiting = true;  // Inicia a espera
        anim.SetBool("isMoving", false);
        yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));  // Aguarda por um tempo aleatório
        currentPoint = (currentPoint == 0) ? 1 : 0;  // Alterna para o próximo ponto
        isWaiting = false;  // Termina a espera
    }

    private void Flip()
    {
        // Inverte a direção do sprite
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        if (healthBar != null)
        {
            UpdateHealthBar();
        }
        Debug.Log($"Dano recebido: {damageAmount}, health restante: {currentHealth}");

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            bossBattle.EndBattle();
        }
        else
        {
            StartCoroutine(TookDamageCoroutine());
        }
    }

    private void UpdateHealthBar()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    private IEnumerator TookDamageCoroutine()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }

    private void MoveTowardsTarget()
    {
        anim.SetBool("isMoving", true);
        float targetDistance = transform.position.x - patrolPoints[currentPoint].position.x;
        float moveDirection = targetDistance < 0 ? 1 : -1;
        rb.linearVelocity = new Vector2(speed * moveDirection, rb.linearVelocity.y);
    }

    private void Fire()
    {
        float direction = facingRight ? -1f : 1f;
        anim.SetBool("isFiring", true);
        GameObject bombInstance = Instantiate(bombPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D grenadeRb = bombInstance.GetComponent<Rigidbody2D>();

        float randomLaunchForce = Random.Range(10f, 30f);
        Vector2 launchVelocity = CalculateLaunchVelocity(launchAngle, direction, randomLaunchForce);
        SetThrower(bombInstance);

        grenadeRb.linearVelocity = launchVelocity;
        anim.SetBool("isFiring", false);
    }

    private Vector2 CalculateLaunchVelocity(float angle, float direction, float force)
    {
        float angleRad = angle * Mathf.Deg2Rad;
        float x = Mathf.Cos(angleRad) * force * direction;
        float y = Mathf.Sin(angleRad) * force;
        return new Vector2(x, y);
    }

    private void SetThrower(GameObject bomb)
    {
        if (bomb != null)
        {
            Grenade grenadeController = bomb.GetComponent<Grenade>();
            grenadeController.thrower = gameObject;
        }
    }

    private void SpawnAnts()
    {
        anim.SetBool("isSpawning", true);
        GameObject ant = Instantiate(explodeAnt, antSpawnPoint.position, Quaternion.identity);
        Rigidbody2D antRb = ant.GetComponent<Rigidbody2D>();

        Vector2 force = new Vector2(0f, 20f); 
        antRb.AddForce(force, ForceMode2D.Impulse);
        anim.SetBool("isSpawning", false);
    }
}
