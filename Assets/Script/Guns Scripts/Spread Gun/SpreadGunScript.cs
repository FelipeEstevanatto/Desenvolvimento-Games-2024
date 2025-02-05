using UnityEngine;

public class SpreadGun : Gun
{
    [SerializeField] private int bulletCount = 3; 
    [SerializeField] private float spreadAngle = 30f; // angle between the two extreme bullets 

    protected override void FireBullet(float direction)
    {
        if (firePoint == null)
        {
            Debug.LogError("FirePoint n�o definido!");
            return;
        }

        // angle for the extreme bullet below the origin bullet
        float startAngle = -spreadAngle / 2f;
        Vector2 directionVector;

        for (int i = 0; i < bulletCount; i++)
        {
            // divides the angle for each bullet
            float angle = startAngle + (spreadAngle / (bulletCount - 1)) * i;

            
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            SetDamage(bullet);
            SetShooterTag(bullet);
            // convert the angle to radians (unity default)
            float radians = Mathf.Deg2Rad * angle;

            // calculating bullet's direction
            //(cos(x), sin(x)) describes the position of a point on a unit circle; direction is always 1 or -1 (player.transform.localScale.x)
            // Vector2(Mathf.Cos(radians), Mathf.Sin(radians)) * direction gets the directional vector that the bullet will follow
            if (playerController != null && playerController.IsLookingUp)
            {
                directionVector = new Vector2(Mathf.Sin(radians), Mathf.Cos(radians));
            }
            else
            {
                directionVector = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)) * direction;
                bullet.transform.localScale = new Vector3(direction * Mathf.Abs(bullet.transform.localScale.x),
                                                          bullet.transform.localScale.y,
                                                          bullet.transform.localScale.z);
            }

            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
            bulletRB.linearVelocity = directionVector * shotSpeed;
        }
    }
}
