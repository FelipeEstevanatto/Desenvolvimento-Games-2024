using UnityEngine;

public class GravityBomb : MonoBehaviour
{
    [SerializeField] private float explosionRadius = 3f;   // radius of the explosion damage
    [SerializeField] private float grenadeDamage = 50f;    // damage caused by the explosion
    [SerializeField] private GameObject explosionEffect;    // visual effect of the explosion
    [HideInInspector] public string throwerTag;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the collided object has the tag "Ground", we explode
        if (collision.gameObject.CompareTag("Ground"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        // Instantiate the explosion effect at the grenade's position, optionally with random rotation
        if (explosionEffect != null)
        {
            Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
            Instantiate(explosionEffect, transform.position, randomRotation);
        }

        // Play your explosion sound
        AudioManager.instance.PlaySFX(AudioManager.instance.bombClip);

        // Detect all objects within the explosion radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D hit in colliders)
        {
            if (throwerTag == "Player")
            {
                if (hit.CompareTag("Enemy"))
                {
                    Enemy enemy = hit.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(grenadeDamage);
                    }
                }
                else if (hit.CompareTag("Player"))
                {
                    PlayerController player = hit.GetComponent<PlayerController>();
                    if (player != null)
                    {
                        player.TakeDamage(grenadeDamage);
                    }
                }
            }
            else if (throwerTag == "Enemy" && hit.CompareTag("Player"))
            {
                PlayerController player = hit.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.TakeDamage(grenadeDamage);
                }
            }
        }

        // Destroy this grenade object after it explodes
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Debug.Log("OnDrawGizmos called");
        // Draw a red wire sphere in the editor to represent the explosion radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
