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
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (playerWeaponManager != null)
            {
                playerWeaponManager.PickupWeapon(weaponPrefab);
                Destroy(gameObject); 
            }
        }
    }
}
