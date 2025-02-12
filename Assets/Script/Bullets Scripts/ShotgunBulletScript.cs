using UnityEngine;

public class BulletShotgun : Bullet
{
    [SerializeField] private float minDamage; //damage given in short range

    private void OnTriggerEnter2D(Collider2D collision)
    {
        float distance = Vector2.Distance(startPos, collision.transform.position); // distance between target and firepoint
        damage = CalculateDamage(distance); //gets the bullet damage based on positionz
        if (this.shooter.tag == "Player" && collision.CompareTag("Enemy"))
        {
            EnemyDamage(collision);
            Destroy(gameObject);
        }
        else if (this.shooter.tag == "Enemy" && collision.CompareTag("Player"))
        {
            playerDamage(collision);
            Destroy(gameObject);
        }
        //destroy bullet if it hits a wall
        if (collision.CompareTag("Ground"))
        {
            //Adicionar efeito de colisão da bullet na parede(Quando tivermos)
            Destroy(gameObject);
        }
    }

    private float CalculateDamage(float distance)
    {
        // if it's far (maxRange/2), apply minimum damage
        if (distance >= maxRange/2) return minDamage;

        //otherwise, apply base damage
        return damage;
        //DÁ PARA USAR A FUNÇÃO Mathf.Lerp pra ajustar o dano conforme a distancia, mas o dano por bala que colocamos na arma pode ficar inconsistente (pois o dano quase nunca será máximo)
    }
}
