using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Weapon weaponPrefab; // Prefab da arma a ser pega
    private bool isPlayerInRange = false;
    private WeaponManager playerWeaponManager;

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
                    playerPickUpGun.Reload(); 
                    Destroy(gameObject);
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
            Destroy(gameObject);
        }
    }
}
