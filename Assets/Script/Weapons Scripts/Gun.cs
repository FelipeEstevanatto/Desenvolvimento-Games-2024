using System;
using UnityEngine;

public class Gun : Weapon
{
    [SerializeField] private GameObject bulletPrefab;
    public Transform firePoint;

    public override void Attack(float direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.transform.localScale = new Vector3(direction * Mathf.Abs(bullet.transform.localScale.x),
                                                  bullet.transform.localScale.y,
                                                  bullet.transform.localScale.z);

        Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
        bulletRB.linearVelocity = new Vector2(direction * shotSpeed, 0);
    }
}
