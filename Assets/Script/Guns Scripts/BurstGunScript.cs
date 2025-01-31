using System.Collections;
using UnityEngine;

public class BurstGun: Gun
{
    [SerializeField] int burstBulletsTotal;
    [SerializeField] float shotDelay;
    protected override void FireBullet(float direction)
    {
        StartCoroutine(BurstShot(direction));
    }

    IEnumerator BurstShot(float direction)
    {
        for (int i = 0; i < burstBulletsTotal; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            SetDamage(bullet); //passes the damage defined in gun to the bullet
            bullet.transform.localScale = new Vector3(direction * Mathf.Abs(bullet.transform.localScale.x),
                                                        bullet.transform.localScale.y,
                                                        bullet.transform.localScale.z);

            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
            bulletRB.linearVelocity = new Vector2(direction * shotSpeed, 0);
            yield return new WaitForSeconds(shotDelay);
        }
    }

    

}

