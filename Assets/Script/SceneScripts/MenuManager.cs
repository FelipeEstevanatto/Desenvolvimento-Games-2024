using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class MenuManager : MonoBehaviour
{
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip inGameMusic;

    // [SerializeField] private GameObject hoverSprite;

    // private Vector3 originalScale;
    // private Coroutine scaleCoroutine;

    public void Start()
    {
        // Set the game to run in fullscreen mode at the native resolution
        // Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.FullScreenWindow);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        AudioManager.instance.PlayMusic(mainMenuMusic);
    }

    public void LoadScene(string sceneName = "Level1")
    {
        SceneManager.LoadScene(sceneName);
        AudioManager.instance.UnpauseMusic();
        AudioManager.instance.PlayMusic(inGameMusic);
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        // Stop playing the scene in the editor
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // Quit the application
        Application.Quit();
        #endif
    }
}
