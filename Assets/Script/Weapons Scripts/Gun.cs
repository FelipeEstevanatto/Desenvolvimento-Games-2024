using UnityEngine;

public abstract class Gun : Weapon
{
    public GameObject bulletPrefab;
    public Transform firePoint; 
    [SerializeField] private int ammoCapacity; 
    private int currentAmmo;

    void Start()
    {
        currentAmmo = ammoCapacity;
    }

    public override void Attack(float direction)
    {
        if (currentAmmo > 0) 
        {
            FireBullet(direction);
            currentAmmo--; 
            Debug.Log("Ammo: " + currentAmmo);
        }
        else
        {
            Debug.Log("Sem muni��o!");
        }
    }

    // Instantiate the bullets
    protected virtual void FireBullet(float direction)
    {
        if (firePoint == null)
        {
            Debug.LogError("FirePoint n�o definido!");
            return;
        }

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.transform.localScale = new Vector3(direction * Mathf.Abs(bullet.transform.localScale.x),
                                                  bullet.transform.localScale.y,
                                                  bullet.transform.localScale.z);

        Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
        bulletRB.linearVelocity = new Vector2(direction * shotSpeed, 0);
    }

    //Ainda n�o usei pra nada, mas � uma fun��o que n�o precisar� de override
    public void Reload()
    {
        currentAmmo = ammoCapacity;
        Debug.Log("Arma recarregada!");
    }
}
