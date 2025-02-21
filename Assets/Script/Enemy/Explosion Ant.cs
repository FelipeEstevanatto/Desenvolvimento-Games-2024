using UnityEngine;
using System.Collections;

public class ExplosionAnt : Enemy
{
    [Header("Explosion Ant Settings")]
    [SerializeField] private float chaseSpeed = 2f;
    [SerializeField] private float stopDistance = 3f; // Distância em que o inimigo para
    [SerializeField] private float startChaseDistance = 10f; // Distância para começar a perseguição
    [SerializeField] private float explosionRadius = 5f; // Raio da explosão
    [SerializeField] private float explosionDamage = 20f; // Dano da explosão
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject explosionEffect;
    private bool isChasing = false;

    protected override void Update()
    {
        base.Update();

        if (player != null)
        {
            if (targetDistance < startChaseDistance && !isChasing)
            {
                isChasing = true;
                anim.SetBool("isRunning", true);
            }
            else if (targetDistance <= stopDistance && isChasing)
            {
                isChasing = false;
                anim.SetBool("isRunning", false);
                StartCoroutine(ExplosionAlert());
                StartCoroutine(ExplodeAfterDelay());
            }

            if (isChasing)
            {
                ChasePlayer();
            }
        }
    }


    private void ChasePlayer()
    {
        int direction = (targetDistance >= 0) ? -1 : 1; 
        rb.linearVelocity = new Vector2(direction * chaseSpeed, rb.linearVelocity.y); 

        CheckFlip();
    }

    private IEnumerator ExplodeAfterDelay()
    {
        //rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(1.5f);

        Explode();
    }

    private void Explode()
    {
        if (Vector2.Distance(transform.position, player.transform.position) <= explosionRadius)
        {
            player.TakeDamage(explosionDamage); 
        }

        anim.SetBool("isDead", true);
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    private IEnumerator ExplosionAlert()
    {
        for (int i = 0; i <= 8; i++)
        {
            sprite.color = Color.black;
            yield return new WaitForSeconds(0.1f);
            sprite.color = Color.white;
        }
    }
    
}
