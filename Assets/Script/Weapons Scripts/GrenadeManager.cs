using UnityEngine;

public class GrenadeManager : MonoBehaviour
{
    [SerializeField] private GameObject grenadePrefab; 
    [SerializeField] private Transform grenadeSpawnPoint;
    [SerializeField] private int maxGrenades = 3; 
    private int currentGrenades; 
    [SerializeField] private float throwForce = 10f; 
    [SerializeField] private float throwAngle = 45f; // degrees
    [SerializeField] private GameObject player;
    public int CurrentGrenades => currentGrenades;
    public int MaxGrenades => maxGrenades;

    private void Start()
    {
        currentGrenades = maxGrenades; // Initialize the number of grenades
    }

    private void Update()
    {
        // key '3' throws grenade
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            float direction = Mathf.Sign(player.transform.localScale.x); // gets the player's direction 
            ThrowGrenade(direction); 
        }
    }

    public void ThrowGrenade(float direction)
    {
        if (currentGrenades > 0) 
        {
            // instantiate the grenade 
            GameObject grenadeInstance = Instantiate(grenadePrefab, grenadeSpawnPoint.position, Quaternion.identity);
            //updates the thrower tag in grenade script, so it gives damage to enemy
            Grenade grenadeController = grenadeInstance.GetComponent<Grenade>();
            grenadeController.throwerTag = player.tag;

            // converts the throw angle in radians
            float angleRad = throwAngle * Mathf.Deg2Rad;

            // calculate the velocity of the grenade based on angle and force
            Vector2 throwVelocity = new Vector2(
                Mathf.Cos(angleRad) * throwForce * direction, // horizontal velocity based on direction
                Mathf.Sin(angleRad) * throwForce // vertical velocity
            );

            Rigidbody2D grenadeRb = grenadeInstance.GetComponent<Rigidbody2D>(); 
            
            // ignore collision between the player and the grenade (CASO A GRANADA NÃO DEVA INTERAGIR COM OUTROS OBJETOS EM CENA, FAZER COM LAYERS EM VEZ DE SCRIPT)
            Physics2D.IgnoreCollision(grenadeInstance.GetComponent<Collider2D>(), player.GetComponent<Collider2D>());

            if (grenadeRb != null)
            {
                grenadeRb.linearVelocity = throwVelocity; // apply the calculated velocity to the grenade
            }

            currentGrenades--; // decrease the number of grenades
            Debug.Log($"Granadas restantes: {currentGrenades}"); 
        }
        else
        {
            Debug.Log("Sem granadas disponíveis"); // If no grenades are left, print a message
        }
    }
}
