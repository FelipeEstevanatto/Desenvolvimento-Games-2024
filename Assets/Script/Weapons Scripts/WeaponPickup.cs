using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Weapon weapon; 
    private bool isPlayerInRange = false;
    private WeaponManager playerWeaponManager; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // get the WeaponManager component from the player
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
                playerWeaponManager.PickupWeapon(weapon); 
                Destroy(gameObject); 
            }
        }
    }
}
