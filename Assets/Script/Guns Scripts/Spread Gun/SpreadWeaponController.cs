using UnityEngine;

//CÓDIGO DE UMA APLICAÇÃO ANTIGA QUE IREI TENTAR FAZER DEPOIS USANDO O NOVO SISTEMA DE WEAPON MANAGER
public class SpreadWeaponController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform[] firePoints;
    [SerializeField] private GameObject ammoType;

    [SerializeField] private float shotSpeed;
    [SerializeField] private float shotCounter;
    [SerializeField] private float fireRate;


    private Animator playerAnim;

    void Start()
    {
        playerAnim = player.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            shotCounter -= Time.deltaTime;
            if (shotCounter <= 0)
            {
                shotCounter = fireRate;
                Shoot();
            }

            playerAnim.SetTrigger("Shoot");
        }
        else
        {
            shotCounter = 0;
        }
    }

    void Shoot()
    {
        foreach (Transform firePoint in firePoints)
        {
            GameObject bullet = Instantiate(ammoType, firePoint.position, Quaternion.identity);
            float direction = Mathf.Sign(player.transform.localScale.x);

            bullet.transform.localScale = new Vector3(direction * Mathf.Abs(bullet.transform.localScale.x),
                                                      bullet.transform.localScale.y,
                                                      bullet.transform.localScale.z);

            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
            bulletRB.linearVelocity = new Vector2(direction * shotSpeed, 0);
            Destroy(bullet.gameObject, 1f);
        }
    }
}
