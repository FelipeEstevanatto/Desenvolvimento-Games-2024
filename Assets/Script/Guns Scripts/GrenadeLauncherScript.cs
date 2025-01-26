using UnityEngine;

public class GrenadeLauncherScript : Gun
{
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private float launchForce = 10f; 
    [SerializeField] private float launchAngle = 45f; 

    protected override void FireBullet(float direction)
    {
        if (firePoint == null)
        {
            Debug.LogError("FirePoint não definido");
            return;
        }

        GameObject grenadeInstance = Instantiate(grenadePrefab, firePoint.position, Quaternion.identity);

        float angleRad = launchAngle * Mathf.Deg2Rad;

        Vector2 launchVelocity = new Vector2(
                Mathf.Cos(angleRad) * launchForce * direction, // horizontal velocity based on direction
                Mathf.Sin(angleRad) * launchForce // vertical velocity
          );

        Rigidbody2D grenadeRb = grenadeInstance.GetComponent<Rigidbody2D>();
        if (grenadeRb != null)
        {
            grenadeRb.linearVelocity = launchVelocity; // apply the calculated velocity to the grenade
        }
    }
}
