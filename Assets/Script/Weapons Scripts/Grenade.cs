using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private float explosionDelay = 2f; // time before the explosion occurs
    [SerializeField] private float explosionRadius = 3f; // radius of the explosion's damage
    [SerializeField] private float grenadeDamage = 50f; // damage caused by the explosion
    [SerializeField] private AudioClip explosionClip; // sound effect of the explosion
    [SerializeField] private GameObject explosionEffect; // visual effect of the explosion
    [HideInInspector] public GameObject thrower;
    private void Start()
    {
        // calls the Explode method after the delay specified by explosionDelay
        Invoke(nameof(Explode), explosionDelay);
    }

    private void Explode()
    {
        // instantiate the explosion effect at the grenade's position (Esbo�o para efeito)
        if (explosionEffect != null)
        {
            // Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
            Instantiate(explosionEffect, transform.position, randomRotation);
        }

        AudioManager.instance.PlaySFX(explosionClip);

        // detect all objects within the explosion radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D hit in colliders)
        {
            if (thrower.tag == "Player")
            {
                if (hit.CompareTag("Enemy"))
                {
                    Enemy enemy = hit.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(grenadeDamage);
                    }
                }
                else //allow grenade TK
                {
                    if (hit.CompareTag("Player"))
                    {
                        PlayerController player = hit.GetComponent<PlayerController>();
                        if (player != null)
                        {
                            player.TakeDamage(grenadeDamage);
                        }
                    }
                }
            }
            else if (thrower.tag == "Enemy" && hit.CompareTag("Player"))
            {
                PlayerController player = hit.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.TakeDamage(grenadeDamage);
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
