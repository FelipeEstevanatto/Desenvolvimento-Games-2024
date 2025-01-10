using UnityEngine;

public abstract class Gun : Weapon
{
    public GameObject bulletPrefab;
    public float shotSpeed;
    public Transform firePoint;
    [SerializeField] private int bulletDamage;
    [SerializeField] private int ammoCapacity;
    public float fireRate;
    public int bulletsPerShot = 1;
    [HideInInspector] public int currentAmmo;
    [HideInInspector] public float nextFireTime;

    void Awake()
    {
        currentAmmo = ammoCapacity;
        nextFireTime = 0f;
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
        SetDamage(bullet);
        bullet.transform.localScale = new Vector3(direction * Mathf.Abs(bullet.transform.localScale.x),
                                                  bullet.transform.localScale.y,
                                                  bullet.transform.localScale.z);

        Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
        bulletRB.linearVelocity = new Vector2(direction * shotSpeed, 0);
    }

    public void SetDamage(GameObject bullet)
    {
        BulletController bulletController = bullet.GetComponent<BulletController>();
        if (bulletController != null)
        {
            bulletController.damage = this.bulletDamage; 
        }
    }

    //Aplicar Reload com um botao pressionável e adicionar sistema de pente das armas 
    public void Reload()
    {
        currentAmmo = ammoCapacity;
        Debug.Log("Arma recarregada");
    }
}
