using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float health = 100f;

    protected abstract void Die();

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        Debug.Log("Dano recebido: " + damageAmount + ", health restante: " + health);

        if (health <= 0)
        {
            Die();
        }
    }
}