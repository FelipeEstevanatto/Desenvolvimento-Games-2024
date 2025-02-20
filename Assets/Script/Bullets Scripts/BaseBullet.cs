using System.Collections;
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

        if (collision.CompareTag(shooter.tag)) return; //avoid TK

        if (shooter.tag == "Player" && collision.CompareTag("Enemy"))
        {
            EnemyDamage(collision);
            Destroy(gameObject);
        }
        else if (shooter.tag == "Enemy" && collision.CompareTag("Player"))
        {
            playerDamage(collision);
            Destroy(gameObject);
        }
    }
}