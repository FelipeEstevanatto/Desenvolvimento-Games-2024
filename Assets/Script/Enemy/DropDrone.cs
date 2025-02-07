using UnityEngine;
using System.Collections;

public class DropDrone : Enemy
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private Transform dropPoint;
    [SerializeField] private GameObject dropGrenadePrefab;
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

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("MainCamera"))
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator DropBomb()
    {
        for(int i=0; i < dropTotal; i++)
        {
            GameObject dropGrenade = Instantiate(dropGrenadePrefab, dropPoint.position, Quaternion.identity);
            SetThrowerTag(dropGrenade);
            Rigidbody2D dropGrenadeRB = dropGrenade.GetComponent<Rigidbody2D>();
            dropGrenadeRB.linearVelocity = new Vector2(dropSpeedX, 0);
            yield return new WaitForSeconds(dropDelay);
        }
    }

    private void SetThrowerTag(GameObject grenade)
    {
        if (grenade != null)
        {
            Grenade grenadeController = grenade.GetComponent<Grenade>();
            grenadeController.throwerTag = transform.root.tag;
        }
    }

}
