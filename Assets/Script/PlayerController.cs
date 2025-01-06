using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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
    {
        HandleInput();
        HandleCrouch();
        HandleAnimation();
    }

    void FixedUpdate()
    {
        HandleMovement();
        CheckGrounded();
    }

    private void HandleInput()
    {
        if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetBool("Jump", true);
        }

        //if (Input.GetButtonDown("Fire1"))
        //{
        //    animator.SetTrigger("Shoot");
        //}
    }

    private void HandleCrouch() //Corrigir animação e fazer verificação para saber se o Player pode levantar ou não
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
        else if (isCrouching)
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
}
