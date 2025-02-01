using UnityEngine;

public class GunEnemy : Enemy
{
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float walkDistance = 5f;
    [SerializeField] private float speed = 5f;
    [SerializeField] Animator anim;
    [SerializeField] Animator handsAnim;

    private float nextFireTime;
    private bool isAttacking;
    private bool isRunning;
    private Vector3 weaponHolderOriginalPos;
    private Vector3 weaponHolderTargetPos;
    private float weaponMoveSpeed = 5f; 

    protected override void Start()
    {
        base.Start();
        weaponHolderOriginalPos = weaponHolder.localPosition; 
        weaponHolderTargetPos = weaponHolderOriginalPos; 
    }

    protected override void Update()
    {
        base.Update();

        float distanceToTarget = Mathf.Abs(targetDistance);

        // Define o estado do inimigo
        isRunning = distanceToTarget < walkDistance && distanceToTarget > attackDistance;
        isAttacking = distanceToTarget <= attackDistance && Time.time >= nextFireTime;

        if (anim != null)
        {
            anim.SetBool("isRunning", isRunning);
            handsAnim.SetBool("isRunning", isRunning);
        }

        if (isAttacking)
        {
            Attack();
        }
    }

    private void FixedUpdate()
    {
        if (isRunning && !isAttacking)
        {
            Move();
            weaponHolderTargetPos = new Vector3(-0.05f, weaponHolderOriginalPos.y, weaponHolderOriginalPos.z);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            weaponHolderTargetPos = weaponHolderOriginalPos;
        }

        weaponHolder.localPosition = Vector3.Lerp(weaponHolder.localPosition, weaponHolderTargetPos, Time.fixedDeltaTime * weaponMoveSpeed); //weapon moves smoothly 
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
