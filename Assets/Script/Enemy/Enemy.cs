using UnityEngine;
using System.Collections;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float health = 100f;
    [SerializeField] protected WeaponPickup weaponPickup;
    [SerializeField] protected Weapon weaponPrefab;
    [SerializeField] protected Transform weaponHolder;
    [SerializeField] protected float attackDistance;

    protected Weapon weapon;
    protected bool facingRight = true;
    protected Transform target;
    protected float targetDistance;
    protected Rigidbody2D rb;
    protected SpriteRenderer sprite;

    protected bool isFlipping = false; 
    private float flipDelay = 0.1f;  

    private void Awake()
    {
        target = FindFirstObjectByType<PlayerController>().transform;
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        if (weaponHolder != null && weaponPrefab != null)
        {
            weapon = Instantiate(weaponPrefab, weaponHolder.position, Quaternion.identity, weaponHolder);
            weapon.transform.localScale /= Mathf.Abs(transform.localScale.x);
        }

        
        CheckFlip();
    }

    protected virtual void Update()
    {
        targetDistance = transform.position.x - target.position.x;
    }

    protected void CheckFlip()
    {
        if ((targetDistance < 0 && !facingRight) || (targetDistance > 0 && facingRight))
        {
            if (!isFlipping)
            {
                StartCoroutine(Flip());
            }
        }
    }
    protected IEnumerator Flip()
    {
        isFlipping = true;
        yield return new WaitForSeconds(flipDelay); 

        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        isFlipping = false; 
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        Debug.Log($"Dano recebido: {damageAmount}, health restante: {health}");

        if (health <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(TookDamageCoroutine());
        }
    }

    protected void Die()
    {
        if (weaponPickup != null)
        {
            Instantiate(weaponPickup, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    IEnumerator TookDamageCoroutine()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }
}
