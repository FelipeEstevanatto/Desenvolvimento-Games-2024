using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy2 : Enemy
{
    private Transform posicaoJogador;
    public float velocidadeInimigo;
    public float distanciaMorte = 3f; // Distância mínima para considerar como colisão

    // O valor da health será configurável diretamente no Inspector da Unity para este inimigo

    protected void Start()
    {
        posicaoJogador = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected override void Update()
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
}