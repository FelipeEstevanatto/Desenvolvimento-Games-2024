using UnityEngine;

public class HealthPack : MonoBehaviour
{
    [SerializeField] private float healthAmount = 50f; // health amount that the player will receive

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Get the PlayerController component
            PlayerController playerController = FindFirstObjectByType<PlayerController>();

            // Check if the player is in range
            if (Vector2.Distance(transform.position, playerController.transform.position) < 1.5f)
            {
                // Give health to the player
                playerController.GiveHealth(healthAmount);

                // Destroy the health pack
                Destroy(gameObject);
            }
        }
    }

    // Called when the Collider other enters the trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player is in range");
        }
    }
}
