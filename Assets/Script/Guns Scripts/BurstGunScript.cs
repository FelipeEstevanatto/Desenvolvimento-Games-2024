using System.Collections;
using UnityEngine;

public class BurstGun: Gun
{
    [SerializeField] int burstBulletsTotal = 3;
    [SerializeField] float shotDelay;
    protected override void FireBullet(float direction)
    {
        StartCoroutine(BurstShot(direction));
    }

    private IEnumerator BurstShot(float direction)
    {
        for (int i = 0; i < burstBulletsTotal; i++)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.rifleBurstClip);

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            SetDamage(bullet); //passes the damage defined in gun to the bullet
            SetShooter(bullet);
            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
            if (playerController != null && playerController.IsLookingUp && shooter.tag == "Player")
            {
                bulletRB.linearVelocity = new Vector2(0, shotSpeed);
            }
            else
            {
                bullet.transform.localScale = new Vector3(direction * Mathf.Abs(bullet.transform.localScale.x),
                                                          bullet.transform.localScale.y,
                                                          bullet.transform.localScale.z);
                bulletRB.linearVelocity = new Vector2(direction * shotSpeed, 0);
            }
            yield return new WaitForSeconds(shotDelay);
        }
    }
}

