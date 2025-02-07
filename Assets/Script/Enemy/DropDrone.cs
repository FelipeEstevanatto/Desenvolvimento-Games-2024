using UnityEngine;

public class DropDrone : Enemy
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private Transform dropPoint;
    [SerializeField] private GameObject dropGrenadePrefab;
    [SerializeField] private float dropDistance = 20f;
    [SerializeField] private float dropSpeedX = 20f;
    private bool isActive = false;
    private bool hasDropped = false; 

    protected override void Update()
    {
        base.Update();
        if (isActive && !hasDropped) 
        {
            if (Mathf.Abs(targetDistance) <= dropDistance)
            {
                Debug.Log("Entrou");
                GameObject dropGrenade = Instantiate(dropGrenadePrefab, dropPoint.position, Quaternion.identity);
                SetThrowerTag(dropGrenade);
                Rigidbody2D dropGrenadeRB = dropGrenade.GetComponent<Rigidbody2D>();
                dropGrenadeRB.linearVelocity = new Vector2(-dropSpeedX, 0);

                hasDropped = true; 
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
            hasDropped = false; 
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
