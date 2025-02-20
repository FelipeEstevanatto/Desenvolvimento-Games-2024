using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Weapon weaponPrefab; 
    private bool isPlayerInRange = false;
    private WeaponManager playerWeaponManager;
    private float distanceFromGround = 1.25f;

    private void Start()
    {
        AdjustHeight();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerWeaponManager = other.GetComponent<WeaponManager>();
            if (playerWeaponManager != null)
            {
                isPlayerInRange = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            playerWeaponManager = null;
        }
    }

    private void Update()
    {
        if (isPlayerInRange)
        {
            if(weaponPrefab is Gun)
            {
                //reload if the weapon pickup is the same as the one in the pickupweapon slot
                if (weaponPrefab is Gun && playerWeaponManager.pickupWeaponInstance != null && weaponPrefab.GetType() == playerWeaponManager.pickupWeaponInstance.GetType())
                {
                    Gun playerPickUpGun = playerWeaponManager.pickupWeaponInstance as Gun;
                    if (playerPickUpGun.currentAmmo == playerPickUpGun.ammoCapacity)
                    {
                        return;
                    }
                    else
                    {
                        playerPickUpGun.Reload(); 
                        Destroy(gameObject);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.E))
                {
                    EquipWeaponPickUp(weaponPrefab);
                }
            }
        }
    }
    private void EquipWeaponPickUp(Weapon weaponPrefab)
    {
        if (playerWeaponManager != null)
        {
            playerWeaponManager.PickupWeapon(weaponPrefab);
            if (!playerWeaponManager.baseWeaponInstance.gameObject.activeSelf)
            {
                playerWeaponManager.EquipWeapon(playerWeaponManager.pickupWeaponInstance);
            }
            Destroy(gameObject);
        }
    }

    public void AdjustHeight()
    {
        Vector3 initialPosition = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(initialPosition, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Ground"));

        if (hit.collider != null)
        {
            Vector3 groundPosition = hit.point;
            transform.position = new Vector3(initialPosition.x, groundPosition.y + distanceFromGround, initialPosition.z);
        }
    }
}
