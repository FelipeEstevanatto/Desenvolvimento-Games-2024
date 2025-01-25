using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    [SerializeField] private float speed;
    public bool ground = true;
    public Transform groundCheck;
    public LayerMask groundLayer;

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

    void Update() {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        ground = Physics2D.Linecast(groundCheck.position, transform.position, groundLayer);
        Debug.Log(ground);

        if(ground == false) {
            speed = speed * -1;
        }
    }
}
