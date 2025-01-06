using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float bulletDestroyTime = 1f;
    void Start()
    {
        Destroy(gameObject, bulletDestroyTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            //Adicionar efeito de colisão da bullet na parede(Quando tivermos)
            Destroy(gameObject);
        }
    }
}
