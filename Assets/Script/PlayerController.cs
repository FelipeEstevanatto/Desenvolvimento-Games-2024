using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int Health => health;

    [SerializeField] private int health;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float crouchSpeedMultiplier;
    [SerializeField] private float distanceToGround;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Collider2D normalCollider; 
    [SerializeField] private Collider2D crouchCollider; 

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private bool isCrouching;
    private bool isLookingUp;
    private bool canDash = true;
    private bool isDashing;
    [SerializeField]private float dashingPower = 24f;
    [SerializeField]private float dasgingTime = 0.2f;
    [SerializeField]private float dashingCooldown = 1f;
    [SerializeField] private TrailRenderer tr; 

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        normalCollider.enabled = true;
        crouchCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {   if ( isDashing == false) {
        HandleInput();
        HandleCrouch();
    }
        HandleAnimation();
        if(Input.GetKey(KeyCode.LeftShift) && canDash) {
            StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        if (!isDashing)
    {
        HandleMovement();
    }
        CheckGrounded();
    }

    private void HandleInput()
    {
        if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching)
        {
            health--;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetBool("Jump", true);
        }

        isLookingUp = Input.GetKey(KeyCode.W);
        animator.SetBool("LookingUp", isLookingUp);
    }

    private void HandleCrouch() //Corrigir anima��o e fazer verifica��o para saber se o Player pode levantar ou n�o
    {
        if (Input.GetKey(KeyCode.LeftControl) && isGrounded)
        {
            if (!isCrouching)
            {
                isCrouching = true;
                speed *= crouchSpeedMultiplier;
                animator.SetBool("Crouched", true);

                normalCollider.enabled = false;
                crouchCollider.enabled = true;
            }
        }
        else if (isCrouching && !Physics2D.Raycast(transform.position, Vector2.up, 1f, groundLayer))
        {
            isCrouching = false;
            speed /= crouchSpeedMultiplier;
            animator.SetBool("Crouched", false);

            normalCollider.enabled = true;
            crouchCollider.enabled = false;
        }
    }

    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(horizontalInput * speed, rb.linearVelocity.y);

        if (horizontalInput != 0)
        {
            Vector3 newScale = transform.localScale;
            newScale.x = Mathf.Sign(horizontalInput) * Mathf.Abs(newScale.x);
            transform.localScale = newScale;
        }
    }

    private void HandleAnimation()
    {
        animator.SetFloat("Velocity", Mathf.Abs(rb.linearVelocity.x));

        if (isGrounded)
        {
            animator.SetBool("Jump", false);
        }
    }

    private void CheckGrounded()
    {
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, distanceToGround, groundLayer);

        if (isGrounded != wasGrounded)
        {
            animator.SetBool("Grounded", isGrounded);
        }
    }

    private IEnumerator Dash()
{
    canDash = false; // Desabilita a capacidade de dar Dash novamente
    isDashing = true; // Marca que está dashing

    float originalGravity = rb.gravityScale; // Salva a gravidade original
    rb.gravityScale = 0f; // Desabilita gravidade durante o Dash
    rb.linearVelocity = new Vector2(transform.localScale.x * dashingPower, 0f); // Aplica a força de Dash na direção correta
    tr.emitting = true; // Ativa o efeito de trail renderer

    yield return new WaitForSeconds(dasgingTime); // Espera o tempo do Dash

    tr.emitting = false; // Desativa o efeito de trail renderer
    rb.gravityScale = originalGravity; // Restaura a gravidade
    isDashing = false; // Termina o Dash

    yield return new WaitForSeconds(dashingCooldown); // Espera o cooldown
    canDash = true; // Permite que o Dash seja usado novamente
}

}