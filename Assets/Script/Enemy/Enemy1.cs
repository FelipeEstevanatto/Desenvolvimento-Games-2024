using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Enemy1 : Enemy
{
    [SerializeField] private float speed;
    public bool ground = true;
    public Transform groundCheck;
    public LayerMask groundLayer;

    protected override void Die()
    {
        Destroy(gameObject);
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        ground = Physics2D.Linecast(groundCheck.position, transform.position, groundLayer);
        //Debug.Log(ground);

        if (ground == false)
        {
            speed = speed * -1;
        }
    }
}
