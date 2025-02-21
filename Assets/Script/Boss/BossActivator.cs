using Unity.Cinemachine;
using UnityEngine;

public class BossActivator : MonoBehaviour
{
    [SerializeField] private GameObject bossBattleObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            bossBattleObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
