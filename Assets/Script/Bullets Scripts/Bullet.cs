using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    [HideInInspector] public float damage; // damage that each bullet gives (defined in weapon inspector)
    [SerializeField] protected float maxRange = 5f;  // maximum range
    protected Vector2 startPos; // bullet's start position 
    [HideInInspector] public string shooterTag;

    private void Start()
    {
        startPos = transform.position; // gets the start position
    }

    private void Update()
    {
        // verify if the distance traveled is bigger than the maxRange
        float distanceTraveled = Vector2.Distance(startPos, transform.position);

        if (distanceTraveled >= maxRange)
        {
            Destroy(gameObject);  
        }
    }

    //gives damage to enemy
    protected virtual void EnemyDamage(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }

    protected virtual void playerDamage(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            player.TakeDamage(damage);
        }
    }
}
