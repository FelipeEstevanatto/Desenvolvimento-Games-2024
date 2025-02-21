using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip inGameMusic;
    [SerializeField] private Sprite mutedSprite;
    [SerializeField] private Sprite unmutedSprite;
    [SerializeField] private Button muteButton;

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

    public void MuteButton()
    {
        AudioManager.instance.ToggleMute();
        // Toggle the mute icon
        muteButton.image.sprite = AudioManager.instance.IsMuted ? mutedSprite : unmutedSprite;
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
