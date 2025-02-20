using UnityEngine;
using System.Collections;

public abstract class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] protected float health = 100f;
    [SerializeField] protected float attackDistance;
    [SerializeField] protected int scoreValue = 10;

    protected bool facingRight = true;
    protected PlayerController player;
    protected float targetDistance;
    protected Rigidbody2D rb;
    protected SpriteRenderer sprite;

    protected bool isFlipping = false; 
    private float flipDelay = 0.1f;  

    private void Awake()
    {
        player = FindFirstObjectByType<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }


    protected virtual void Update()
    {
        if (player != null)
        {
            targetDistance = transform.position.x - player.transform.position.x;
        }
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

    protected virtual void Die()
    {
        Debug.Log("Enemy dead, score: " + scoreValue);
        ScoreManager.instance.AddScore(scoreValue);

        Destroy(gameObject);
    }

    private IEnumerator TookDamageCoroutine()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }
}
