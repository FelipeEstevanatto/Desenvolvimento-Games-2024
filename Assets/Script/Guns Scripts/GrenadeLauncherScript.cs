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

        AudioManager.instance.PlaySFX(AudioManager.instance.grenadeLauncherThump);

        GameObject grenadeInstance = Instantiate(grenadePrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D grenadeRb = grenadeInstance.GetComponent<Rigidbody2D>();

        if (grenadeRb == null)
        {
            Debug.LogError("Rigidbody2D não encontrado no prefab da granada");
            return;
        }

        float angle = playerController != null && playerController.IsLookingUp && shooter.CompareTag("Player") ? 70f : launchAngle;
        Vector2 launchVelocity = CalculateLaunchVelocity(angle, direction);

        grenadeInstance.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(launchVelocity.y, launchVelocity.x) * Mathf.Rad2Deg);
        SetThrower(grenadeInstance);

        // Then apply velocity
        grenadeRb.linearVelocity = launchVelocity;
    }
    private Vector2 CalculateLaunchVelocity(float angle, float direction)
    {
        float angleRad = angle * Mathf.Deg2Rad;
        float x = Mathf.Cos(angleRad) * launchForce * direction;
        float y = Mathf.Sin(angleRad) * launchForce;
        return new Vector2(x, y);
    }

    private void SetThrower(GameObject grenade)
    {
        if (grenade != null)
        {
            Grenade grenadeController = grenade.GetComponent<Grenade>();
            grenadeController.thrower = transform.root.gameObject;
        }
    }
}
