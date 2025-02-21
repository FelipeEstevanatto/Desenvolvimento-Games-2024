using UnityEngine;

public class AmmoPack : MonoBehaviour
{
    [SerializeField] private int ammo = 50;
    private PlayerController playerController;

    // Update is called once per frame
    void Update()
    {
        if (playerController != null && Input.GetKeyDown(KeyCode.E))
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.HealSoundClip);
            // Give grenades to the player
            playerController.GiveGrenades(ammo);

            // Destroy the health pack
            Destroy(gameObject);
        }
    }

    // Called when the Collider other enters the trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player is in range");
            playerController = other.GetComponent<PlayerController>();
        }
    }

    // Called when the Collider other exits the trigger
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerController = null;
            Debug.Log("Player left the range");
        }
    }
}
