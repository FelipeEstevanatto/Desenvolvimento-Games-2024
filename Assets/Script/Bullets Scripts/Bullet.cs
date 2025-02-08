using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    [HideInInspector] public float damage; // damage that each bullet gives (defined in weapon inspector)
    [SerializeField] protected float maxRange = 5f;  // maximum range
    protected Vector2 startPos; // bullet's start position 
    [HideInInspector] public GameObject shooter;
    protected PlayerController playerController;

    private void Start()
    {
        startPos = transform.position; // gets the start position
        CheckAndRotateUp();
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

    protected void CheckAndRotateUp()
    {
        if (shooter != null && shooter.tag == "Player")
        {
            PlayerController playerController = shooter.GetComponent<PlayerController>();
            if (playerController != null && playerController.IsLookingUp == true)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            }
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
