using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip inGameMusic;


    public void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        AudioManager.instance.PlayMusic(mainMenuMusic);
    }

    public void LoadScene(string sceneName = "Level1")
    {
        SceneManager.LoadScene(sceneName);
        AudioManager.instance.PlayMusic(inGameMusic);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
