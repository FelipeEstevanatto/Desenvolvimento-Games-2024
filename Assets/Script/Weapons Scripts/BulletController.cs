using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float bulletDestroyTime = 1f;
    [HideInInspector] public float damage; //o valor de damage está contido nos scripts das armas
    void Start()
    {
        Destroy(gameObject, bulletDestroyTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //destroy bullet if it hits a wall
        if (collision.CompareTag("Ground"))
        {
            //Adicionar efeito de colisão da bullet na parede(Quando tivermos)
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Damagable"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            Destroy(gameObject); 
        }
    }
}