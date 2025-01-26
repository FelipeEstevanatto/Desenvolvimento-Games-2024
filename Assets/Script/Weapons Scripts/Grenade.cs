using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private float explosionDelay = 2f; // time before the explosion occurs
    [SerializeField] private float explosionRadius = 3f; // radius of the explosion's damage
    [SerializeField] private float grenadeDamage = 50f; // damage caused by the explosion
    [SerializeField] private GameObject explosionEffect; // visual effect of the explosion

    private void Start()
    {
        // calls the Explode method after the delay specified by explosionDelay
        Invoke(nameof(Explode), explosionDelay);
    }

    private void Explode()
    {
        // instantiate the explosion effect at the grenade's position (Esboço para efeito)
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // detect all objects within the explosion radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D hit in colliders)
        {
            // if the object has the "Damagable" tag, apply damage
            if (hit.CompareTag("Damagable"))
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(grenadeDamage); // apply damage to the enemy
                }
            }
        }

        // destroy the grenade object after it explodes
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        // draw a red wire sphere in the editor to represent the explosion radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
