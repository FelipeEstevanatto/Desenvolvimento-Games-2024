using UnityEngine;

public class GunEnemy : Enemy
{
    [SerializeField] private float fireRate = 1f; 
    [SerializeField] private float walkDistance = 5f; 
    [SerializeField] private float speed = 2f; 
    [SerializeField] Animator anim;
    [SerializeField] Animator handsAnim;

    private float nextFireTime;
    private bool isAttacking;
    private bool isWalking;

    protected override void Update()
    {
        base.Update();

        float distanceToTarget = Mathf.Abs(targetDistance);

        // define enemy state
        isWalking = distanceToTarget < walkDistance && distanceToTarget > attackDistance;
        isAttacking = distanceToTarget <= attackDistance && Time.time >= nextFireTime;

        if(anim != null)
        {
            anim.SetBool("IsWalking", isWalking);
            handsAnim.SetBool("isRunning", isWalking);
            //anim.SetBool("isAttacking", isAttacking);
        }

        // Se estiver atacando, para de andar
        if (isAttacking)
        {
            Attack();
        }
    }

    private void FixedUpdate()
    {
        if (isWalking && !isAttacking)
        {
            Move();
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    private void Move()
    {
        float moveDirection = targetDistance < 0 ? 1 : -1;
        rb.linearVelocity = new Vector2(speed * moveDirection, rb.linearVelocity.y);
    }

    private void Attack()
    {
        weapon.Attack(transform.localScale.x);
        nextFireTime = Time.time + fireRate;
    }
}
