using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public float Health => health;
    public float MaxHealth => maxHealth;
    public bool IsCrouching => isCrouching;
    public bool IsLookingUp => isLookingUp;
    public bool IsDead => isDead;

    [Header("Player Settings")]
    [SerializeField] private float health = 100f;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float crouchSpeedMultiplier;
    [SerializeField] private float distanceToGround;
    [SerializeField] private float maxSlopeAngle = 45f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Collider2D normalCollider; 
    public PhysicsMaterial2D normalMaterial; // Material normal (menor fricção)
    public PhysicsMaterial2D slopeMaterial; // Material com maior fricção

    [Header("Dash Settings")]
    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;
    [SerializeField] private TrailRenderer tr;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sprite;
    private bool isGrounded;
    private bool isDead = false;
    private bool isCrouching;
    private bool isLookingUp;
    private bool canDash = true;
    private bool isDashing;
    private float maxHealth;
    public GameObject deathMenuUI;
    private bool isOnSlope;
    private float slopeAngle;

    [Header("Sound Settings")]
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip drumsOfWarClip;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        maxHealth = health;
    }

    void Update()
    {   
        if (health <= 0)
        {
            return;
        }

        HandleInput();
        HandleAnimation();
    }

    // This stays in check with the physics engine, things messing with the rigidbody should be here
    void FixedUpdate()
    {
        if (health <= 0)
        {
            return;
        }

        if (!isDashing)
        {
            HandleMovement();
        }
        CheckGrounded();
        CheckSlope();
    }

    private void HandleInput()
    {
        if (isDashing == false) {
            HandleJump();
            HandleCrouch();
        }

        if(Input.GetKey(KeyCode.LeftShift) && canDash) {
            StartCoroutine(Dash());
        }
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetBool("Jump", true);
            AudioManager.instance.PlaySFX(jumpClip);
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
            }
        }
        else if (isCrouching && CanStandUp())
        {
            isCrouching = false;
            speed /= crouchSpeedMultiplier;
            animator.SetBool("Crouched", false);
        }
    }

    private bool CanStandUp()
    {
        // E.g. a small raycast / circlecast / boxcast above the player to ensure no ceiling
        return !Physics2D.Raycast(transform.position, Vector2.up, 1f, groundLayer);
    }

    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(horizontalInput * speed, rb.linearVelocity.y);

        if (isOnSlope)
        {
            normalCollider.sharedMaterial = slopeMaterial;
        }
        else
        {
            normalCollider.sharedMaterial = normalMaterial;
        }

        // Flip the player sprite
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

    private void CheckSlope()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, distanceToGround, groundLayer);
        if (hit)
        {
            slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            isOnSlope = Mathf.Abs(slopeAngle) > 0 && Mathf.Abs(slopeAngle) < maxSlopeAngle;
        }
        else
        {
            isOnSlope = false;
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

        yield return new WaitForSeconds(dashingTime); // Espera o tempo do Dash

        tr.emitting = false; // Desativa o efeito de trail renderer
        rb.gravityScale = originalGravity; // Restaura a gravidade
        isDashing = false; // Termina o Dash

        yield return new WaitForSeconds(dashingCooldown); // Espera o cooldown
        canDash = true; // Permite que o Dash seja usado novamente
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead)
        {
            return;
        }

        health -= damageAmount;
        Debug.Log($"Dano recebido: {damageAmount}, health restante: {health}");

        if (health <= 0)
        {
            Die();
            health = 0;
        }
        else
        {
            StartCoroutine(TookDamageCoroutine());
        }
    }
    IEnumerator TookDamageCoroutine()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }

    protected void Die()
    {
        Debug.Log("Player morreu");

        AudioManager.instance.PauseMusic();
        AudioManager.instance.PlaySFX(drumsOfWarClip);

        // Activate game over screen on the canvas
        deathMenuUI.SetActive(true);

        animator.SetBool("Jump", false);
        animator.SetTrigger("Death");

        isDead = true;

        // Disable the horizontal player's movement
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        // Destroy(gameObject);
    }

    public void GiveHealth(float healthAmount)
    {
        health += healthAmount;
        health = Mathf.Clamp(health, 0, maxHealth);
    }

    public void GiveGrenades(int ammoAmount)
    {
        // Give ammo to the player
        GrenadeManager grenadeManager = GetComponent<GrenadeManager>();
        if (grenadeManager != null)
        {
            grenadeManager.AddGrenades(ammoAmount);
        }
    }
}