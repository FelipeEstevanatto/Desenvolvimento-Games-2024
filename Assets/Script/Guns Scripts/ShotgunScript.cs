using UnityEngine;


//SCRIPT ALEATÓRIO DO CHAT GPT SÓ PRA PODER TESTAR SE O OVERRIDE FUNCIONAVA CORRETAMENTE
public class Shotgun : Gun
{
    [SerializeField] private int pellets = 5; // N�mero de proj�teis por disparo
    [SerializeField] private float spread = 15f; // �ngulo de espalhamento

    protected override void FireBullet(float direction)
    {
        for (int i = 0; i < pellets; i++)
        {
            float angle = Random.Range(-spread / 2, spread / 2);
            Quaternion bulletRotation = Quaternion.Euler(0, 0, angle);
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, bulletRotation);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.linearVelocity = new Vector2(direction * shotSpeed, 0);
        }

        Debug.Log("Espingarda: V�rios proj�teis disparados!");
    }
}
