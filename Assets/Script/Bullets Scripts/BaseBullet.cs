using UnityEngine;

public class BaseBullet : Bullet
{
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
            EnemyDamage(collision);
        }
    }
}