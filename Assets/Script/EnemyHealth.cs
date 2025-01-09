using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health = 100f;

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        Debug.Log("Dano recebido: " + damageAmount + ", health restante: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject); 
    }
}
