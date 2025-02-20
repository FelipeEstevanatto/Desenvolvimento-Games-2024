using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using System.Collections;

public class GunEnemy : Enemy
{
    [Header("Weapon Components")]
    [SerializeField] protected WeaponPickup weaponPickup;
    [SerializeField] protected Weapon weaponPrefab;
    [SerializeField] protected Transform weaponHolder;
    [SerializeField] private float fireRate = 1f;

    [Header("Enemy Components")]
    [SerializeField] private Animator anim;
    [SerializeField] private float chaseDistance = 5f;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform obstacleCheck;
    [SerializeField] private float obstacleCheckDistance = 2f;

    [Header("Patrol Components")]
    [SerializeField] private Transform[] patrolPoints;
    private int currentPoint = 0;
    [SerializeField] private float waitAtPoint = 5f;
    private float waitCounter;


    protected Weapon weapon;
    private float nextFireTime;
    private bool isAttacking;
    private bool isChasing = false;
    private bool isRunning = true;
    private bool isGrounded;
    private bool isPatrolling = true;
    private bool isWaiting = false;

    protected void Start()
    {
        if (weaponHolder != null && weaponPrefab != null)
        {
            weapon = Instantiate(weaponPrefab, weaponHolder.position, Quaternion.identity, weaponHolder);
            weapon.transform.localScale /= Mathf.Abs(transform.localScale.x);
            //if(weapon is Gun gun)
            //{
            //    fireRate = gun.fireRate;
            //}
        }

        waitCounter = waitAtPoint;

        foreach(Transform pPoint in patrolPoints)
        {
            pPoint.SetParent(null);
        }
    }
    protected override void Update()
    {
        if (!player.IsDead)
        {
            base.Update();

            float distanceToTarget = Mathf.Abs(targetDistance);
            isChasing = distanceToTarget < chaseDistance && distanceToTarget > attackDistance;
            isAttacking = distanceToTarget <= attackDistance && Time.time >= nextFireTime;
            isRunning = rb.linearVelocity.x != 0 ? true : false;

            isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

            if (distanceToTarget < chaseDistance)
            {
                isPatrolling = false; 
                CheckFlip();
            }
            else if (!isPatrolling)
            {
                isPatrolling = true; 
            }

            if (anim != null)
            {
                anim.SetBool("isRunning", isRunning);
            }

            if (isAttacking)
            {
                Attack();
            }
        }
    }

    private void FixedUpdate()
    {
        if (isPatrolling)
        {
            Patrol();
            if (!isWaiting)
            {
                CheckAndJump();
            }
        }
        else if (isChasing && !isAttacking)
        {
            MoveTowardsTarget();
            CheckAndJump();
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

    }

    private void Patrol()
    {
        Transform targetPoint = patrolPoints[currentPoint];
        float direction = targetPoint.position.x - transform.position.x;

        if (Mathf.Abs(direction) < .2f)
        {
            isWaiting = true;
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

            waitCounter -= Time.fixedDeltaTime;

            if (waitCounter <= 0)
            {
                waitCounter = waitAtPoint;
                currentPoint = (currentPoint + 1) % patrolPoints.Length;

                isWaiting = false;
            }
        } 
        else
        {
            isWaiting = false;
            rb.linearVelocity = new Vector2(Mathf.Sign(direction) * speed, rb.linearVelocity.y);

            if (!isFlipping && ((direction > 0 && !facingRight) || (direction < 0 && facingRight)))
            {
                StartCoroutine(Flip());
            }

        }

    }

    private void MoveTowardsTarget()
    {
        float moveDirection = targetDistance < 0 ? 1 : -1; //enemy goes towards player (oposite player direction)
        rb.linearVelocity = new Vector2(speed * moveDirection, rb.linearVelocity.y);
    }

    private void Attack()
    {
        weapon.Attack(transform.localScale.x);
        nextFireTime = Time.time + fireRate;
    }

    private void CheckAndJump()
    {
        RaycastHit2D obstacleHit = Physics2D.Raycast(obstacleCheck.position, Vector2.right * transform.localScale.x, obstacleCheckDistance, groundLayer);
        bool isObstacleAhead = obstacleHit.collider != null;

        if (isObstacleAhead && isGrounded)
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    protected override void Die()
    {
        StartCoroutine(DeathBehaviour());
        ScoreManager.instance.AddScore(scoreValue);
    }

    private IEnumerator DeathBehaviour()
    {
        rb.linearVelocity = Vector2.zero;
        anim.SetBool("isDead", true);
        yield return new WaitForSeconds(0.5f); // death animation time
        Destroy(gameObject);
        if (weaponPickup != null)
        {
            Instantiate(weaponPickup, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }
    }

    private void OnDrawGizmos()
    {
        if (obstacleCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawRay(obstacleCheck.position, Vector2.right * transform.localScale.x * obstacleCheckDistance);
        }

        if (patrolPoints.Length > 0)
        {
            Gizmos.color = Color.blue;
            foreach (var point in patrolPoints)
            {
                Gizmos.DrawSphere(point.position, 0.2f);
            }
        }
    }
}
