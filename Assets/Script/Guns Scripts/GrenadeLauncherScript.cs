using UnityEngine;

public class GrenadeLauncher : Gun
{
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private float launchForce = 10f; 
    [SerializeField] private float launchAngle = 45f; 

    protected override void FireBullet(float direction)
    {
        if (firePoint == null)
        {
            Debug.LogError("FirePoint n�o definido");
            return;
        }

        GameObject grenadeInstance = Instantiate(grenadePrefab, firePoint.position, Quaternion.identity);
        SetThrowerTag(grenadeInstance);
        Rigidbody2D grenadeRb = grenadeInstance.GetComponent<Rigidbody2D>();
        Vector2 launchVelocity;

        float angleRad = launchAngle * Mathf.Deg2Rad;

        if (playerController != null && playerController.IsLookingUp == true)
        {
            launchVelocity = new Vector2(
                    Mathf.Sin(angleRad) * launchForce, // horizontal velocity based on direction
                    Mathf.Cos(angleRad) * launchForce // vertical velocity
              );
        }
        else
        {
            launchVelocity = new Vector2(
                    Mathf.Cos(angleRad) * launchForce * direction, // horizontal velocity based on direction
                    Mathf.Sin(angleRad) * launchForce // vertical velocity
              );
        }

        if (grenadeRb != null)
        {
            grenadeRb.linearVelocity = launchVelocity; // apply the calculated velocity to the grenade
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
