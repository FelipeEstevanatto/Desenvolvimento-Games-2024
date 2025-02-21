using UnityEngine;

public abstract class Gun : Weapon
{
    public GameObject bulletPrefab;
    public float shotSpeed = 20f;
    [HideInInspector] public Transform firePoint;
    [SerializeField] private float bulletDamage = 10f;
    public float fireRate = 0.1f;
    public int bulletsPerShot = 1;
    public int ammoCapacity = 20;
    [HideInInspector] public int currentAmmo;
    [HideInInspector] public float nextFireTime;

    protected GameObject shooter;
    protected PlayerController playerController;

    private void Awake()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        currentAmmo = ammoCapacity;
        nextFireTime = 0f;
        shooter = transform.root.gameObject;
        if (shooter.tag == "Enemy")
        {
            currentAmmo = int.MaxValue;
        }
        if (firePoint == null)
        {
            firePoint = transform.Find("FirePoint");
        }
    }

    protected virtual void Update()
    {
        CheckAndRotateGun();
    }


    public override void Attack(float direction)
    {
        if (currentAmmo > 0 && Time.time >= nextFireTime)
        {
            FireBullet(direction);
            currentAmmo -= bulletsPerShot;
            // Debug.Log("Ammo: " + currentAmmo);
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

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        SetDamage(bullet); //passes the damage defined in gun to the bullet
        SetShooter(bullet);
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

    protected void SetShooter(GameObject bullet)
    {
        Bullet bulletController = bullet.GetComponent<Bullet>();
        if (bulletController != null)
        {
            bulletController.shooter = shooter;
        }
    }

    public void Reload()
    {
        currentAmmo = ammoCapacity;
        Debug.Log("Arma recarregada");
    }

    protected void CheckAndRotateGun()
    {
        if (playerController == null) return;

        if (shooter.tag == "Player")
        {
            if (playerController.IsLookingUp == true)
            {
                transform.rotation = playerController.transform.localScale.x > 0 ? Quaternion.Euler(0f,0f,90f) : Quaternion.Euler(0f,0f,-90f); //rotation fix based on player's direction
            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
    }
}
