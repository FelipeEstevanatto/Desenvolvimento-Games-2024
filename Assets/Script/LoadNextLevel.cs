using UnityEngine;

public class LoadNextLevel : MonoBehaviour
{
    [SerializeField] private string levelName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            LoadLevel();
        }
    }

    private void LoadLevel()
    {
        if (levelName != null)
        {
            Application.LoadLevel(levelName);
        }
    }
}
