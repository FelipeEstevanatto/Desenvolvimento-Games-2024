using UnityEngine;
using System.Collections;

public class DropDrone : Enemy
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private Transform dropPoint;
    [SerializeField] private GameObject dropBombPrefab;
    [SerializeField] private float dropSpeedX = 20f;
    [SerializeField] private float dropTime = 2f;
    [SerializeField] private int dropTotal = 3;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Transform playerTransform;
    [SerializeField] public GameObject explosionEffect;

    [SerializeField] private float dropDelay = 0.1f;
    private Animator anim;
    private float dropCount;
    private bool isDroping;
    private bool isActive = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        dropCount = dropTime;
        if (dropBombPrefab == null)
        {
            Debug.LogError("Drop Bomb Prefab is not assigned in the Inspector");
        }
        if (audioSource == null)
        {
            Debug.LogError("AudioSource is not assigned in the Inspector");
        }
        if (playerTransform == null)
        {
            Debug.LogError("Player Transform is not assigned in the Inspector");
        }
    }

    protected override void Update()
    {
        if (!player.IsDead)
        {
            base.Update();
            anim.SetBool("isDroping", isDroping);
            if (isActive)
            {
                isDroping = true;
                dropCount -= Time.deltaTime;
                if (dropCount <= 0)
                {
                    dropCount = dropTime;
                    StartCoroutine(DropBomb());
                }
                else
                {
                    isDroping = false;
                }
            }

            // Calculate the distance between the player and the plane
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            float screenHeight = Camera.main.orthographicSize * 2;
            float screenWidth = screenHeight * Camera.main.aspect;
            float maxDistance = screenWidth * 2;

            // Destroy the game object if the distance is greater than twice the screen width
            if (isActive && distance > maxDistance)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MainCamera"))
        {
            rb.linearVelocity = new Vector2(-speed, 0);
            isActive = true;
            audioSource.Play();
        }
    }

    private IEnumerator DropBomb()
    {
        for(int i=0; i < dropTotal; i++)
        {
            GameObject gravityBomb = Instantiate(dropBombPrefab, dropPoint.position, Quaternion.identity);
            SetThrowerTag(gravityBomb);
            Rigidbody2D dropGrenadeRB = gravityBomb.GetComponent<Rigidbody2D>();
            dropGrenadeRB.linearVelocity = new Vector2(dropSpeedX, 0);
            yield return new WaitForSeconds(dropDelay);
            // Destroy(gameObject);
        }
    }

    private void SetThrowerTag(GameObject gravityBomb)
    {
        if (gravityBomb != null)
        {
            GravityBomb bombController = gravityBomb.GetComponent<GravityBomb>();
            bombController.throwerTag = transform.root.tag;
        }
    }

    protected override void Die()
    {
        base.Die();
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        // Play your explosion sound
        AudioManager.instance.PlaySFX(AudioManager.instance.bombClip);
    }
}
