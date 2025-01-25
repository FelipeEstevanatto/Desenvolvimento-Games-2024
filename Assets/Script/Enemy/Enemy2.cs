using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy2 : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    private Transform posicaoJogador;
    public float velocidadeInimigo;
    public float distanciaMorte = 3f; // Distância mínima para considerar como colisão

    // Start é chamado uma vez antes da primeira execução de Update
    void Start()
    {
        posicaoJogador = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update é chamado uma vez por frame
    void Update()
    {
        SeguirJogador();
        VerificarColisaoJogador();
    }

    private void SeguirJogador()
    {
        if (posicaoJogador != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, posicaoJogador.position, velocidadeInimigo * Time.deltaTime);
        }
    }

    private void VerificarColisaoJogador()
    {
        if (posicaoJogador != null && Vector2.Distance(transform.position, posicaoJogador.position) < distanciaMorte)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

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