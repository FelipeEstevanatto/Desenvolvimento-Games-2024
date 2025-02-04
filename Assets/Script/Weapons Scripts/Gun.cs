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

    protected PlayerController playerController;

    void Awake()
    {
        currentAmmo = ammoCapacity;
        nextFireTime = 0f;
        if (firePoint == null)
        {
            firePoint = transform.Find("FirePoint");
        }
        playerController = FindFirstObjectByType<PlayerController>();
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
            Debug.LogError("FirePoint n�o definido");
            return;
        }

        Vector3 bulletStartPosition = new Vector3(firePoint.position.x, firePoint.position.y + attackHeightOffset, firePoint.position.z);
        GameObject bullet = Instantiate(bulletPrefab, bulletStartPosition, Quaternion.identity);
        SetDamage(bullet); //passes the damage defined in gun to the bullet
        SetShooterTag(bullet);
        Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();

        if (playerController != null && playerController.IsLookingUp)
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
    }

    protected void SetDamage(GameObject bullet)
    {
        Bullet bulletController = bullet.GetComponent<Bullet>();
        if (bulletController != null)
        {
            bulletController.damage = this.bulletDamage; 
        }
    }

    protected void SetShooterTag(GameObject bullet)
    {
        Bullet bulletController = bullet.GetComponent<Bullet>();
        if (bulletController != null)
        {
            bulletController.shooterTag = transform.root.tag;
        }
    }

    public void Reload()
    {
        currentAmmo = ammoCapacity;
        Debug.Log("Arma recarregada");
    }
}
