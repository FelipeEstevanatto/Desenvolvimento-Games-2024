using UnityEngine;

public abstract class Gun : Weapon
{
    public GameObject bulletPrefab;
    public float shotSpeed = 20f;
    [HideInInspector] public Transform firePoint;
    [SerializeField] private float bulletDamage = 10f;
    public int ammoCapacity = 20;
    public float fireRate = 0.1f;
    public int bulletsPerShot = 1;
    [HideInInspector] public int currentAmmo;
    [HideInInspector] public float nextFireTime;

    void Awake()
    {
        currentAmmo = ammoCapacity;
        nextFireTime = 0f;
        if (firePoint == null)
        {
            firePoint = transform.Find("FirePoint");
        }
    }

    public override void Attack(float direction)
    {
        if (currentAmmo > 0)
        {
            FireBullet(direction);
            currentAmmo -= bulletsPerShot;
            Debug.Log("Ammo: " + currentAmmo);
        }
    }

    // Instantiate the bullets
    protected virtual void FireBullet(float direction)
    {
        if (firePoint == null)
        {
            Debug.LogError("FirePoint não definido");
            return;
        }

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        SetDamage(bullet); //passes the damage defined in gun to the bullet
        bullet.transform.localScale = new Vector3(direction * Mathf.Abs(bullet.transform.localScale.x),
                                                  bullet.transform.localScale.y,
                                                  bullet.transform.localScale.z);

        Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
        bulletRB.linearVelocity = new Vector2(direction * shotSpeed, 0);
    }

    public void SetDamage(GameObject bullet)
    {
        Bullet bulletController = bullet.GetComponent<Bullet>();
        if (bulletController != null)
        {
            bulletController.damage = this.bulletDamage; 
        }
    }
    public void Reload()
    {
        currentAmmo = ammoCapacity;
        Debug.Log("Arma recarregada");
    }
}
