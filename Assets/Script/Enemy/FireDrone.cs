using UnityEngine;
using System.Collections;

public class EnemyDrone : Enemy
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float waitTime = 3f;

    [Header("Shooting Settings")]
    [SerializeField] private float shootingCooldown = 1f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;

    private float waitCount;
    private Animator anim;
    private bool isChasing = false;
    private bool isShooting = false;
    private float patrolDirection = 1f;
    private Vector2 startPosition;

    private void Start()
    {
        anim = GetComponent<Animator>();
        startPosition = transform.position;
        waitCount = waitTime;
        StartCoroutine(Patrol());
    }

    protected override void Update()
    {
        base.Update();
        DetectPlayer();

        if (isChasing && !isShooting)
        {
            ChasePlayer();
        }
    }

    private void DetectPlayer()
    {
        if (Vector2.Distance(transform.position, target.position) <= detectionRadius)
        {
            isChasing = true;
        }
    }

    private void ChasePlayer()
    {
        if (target == null) return;

        Vector2 direction = (target.position - transform.position).normalized;
        Vector2 move = direction * speed; //follows the player in X and Y axis
        rb.linearVelocity = move;

        CheckFlip();

        // Calculate the angle and constrain the rotation to the Z-axis
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        // If the angle crosses 90 or -90, flip the sprite’s Y
        // flips the artwork, not the actual transform direction
        sprite.flipY = angle > 90 || angle < -90;

        if (Vector2.Distance(transform.position, target.position) <= attackDistance && !isShooting)
        {
            StartCoroutine(Shoot());
        }
    }

    private IEnumerator Patrol()
    {
        while (!isChasing)
        {
            waitCount -= Time.deltaTime;
            if(waitCount <= 0)
            {
                waitCount = waitTime;
                anim.SetBool("isMoving", true);
                rb.linearVelocity = new Vector2(patrolDirection * speed, 0); 

                if (patrolDirection > 0 && transform.localScale.x < 0)
                {
                    Vector3 scale = transform.localScale;
                    scale.x *= -1;
                    transform.localScale = scale;
                }
                else if (patrolDirection < 0 && transform.localScale.x > 0)
                {
                    Vector3 scale = transform.localScale;
                    scale.x *= -1;
                    transform.localScale = scale;
                }

                yield return new WaitForSeconds(2f);

                patrolDirection *= -1;
            }
        }
    }

    private IEnumerator Shoot()
    {
        isShooting = true;
        anim.SetBool("isFiring", isShooting);
        rb.linearVelocity = Vector2.zero; //stop and shoot

        Vector2 direction = (target.position - firePoint.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        SetShooter(bullet);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        if (bulletRb != null)
        {
            bulletRb.linearVelocity = direction * 10f; 
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        yield return new WaitForSeconds(shootingCooldown);
        isShooting = false;
    }

    private void SetShooter(GameObject bullet)
    {
        Bullet bulletController = bullet.GetComponent<Bullet>();
        if (bulletController != null)
        {
            bulletController.shooter = transform.root.gameObject;
        }
    }
}
