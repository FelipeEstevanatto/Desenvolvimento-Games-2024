using UnityEngine;

public class HealthPack : MonoBehaviour
{
    [SerializeField] private float healthAmount = 50f; // health amount that the player will receive
    private PlayerController playerController;

    // Update is called once per frame
    void Update()
    {
        if (playerController != null && Input.GetKeyDown(KeyCode.E))
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.HealSoundClip);
            // Give health to the player
            playerController.GiveHealth(healthAmount);

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
