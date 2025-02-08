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
    }

    protected override void Update()
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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MainCamera"))
        {
            rb.linearVelocity = new Vector2(-speed, 0);
            isActive = true;
        }
    }

    // private void OnTriggerExit2D(Collider2D other)
    // {
    //     // if (other.CompareTag("MainCamera"))
    //     // {
    //     //     Destroy(gameObject);
    //     // }
    // }

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

}
