using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScript : MonoBehaviour
{
    public void ExitToMenu()
    {
        // Load the main menu scene
        SceneManager.LoadScene("MainTitle");
    }
}
