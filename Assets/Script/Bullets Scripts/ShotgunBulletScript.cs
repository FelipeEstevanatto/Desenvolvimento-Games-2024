using UnityEngine;

public class BulletShotgun : Bullet
{
    [SerializeField] private float minDamage; //damage given in short range

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Damagable"))
        {
            float distance = Vector2.Distance(startPos, collision.transform.position); // distance between target and firepoint
            damage = CalculateDamage(distance); //gets the bullet damage based on position

            EnemyDamage(collision);
        }
    }

    private float CalculateDamage(float distance)
    {
        // if it's far (maxRange/2), apply minimum damage
        if (distance >= maxRange/2) return minDamage;

        //otherwise, apply base damage
        return damage;
        //D� PARA USAR A FUN��O Mathf.Lerp pra ajustar o dano conforme a distancia, mas o dano por bala que colocamos na arma pode ficar inconsistente (pois o dano quase nunca ser� m�ximo)
    }
}
