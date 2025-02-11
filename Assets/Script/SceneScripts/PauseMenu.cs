using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public Slider musicVolumeSlider;
    public Slider SFXVolumeSlider;

    public TextMeshProUGUI musicText;
    public TextMeshProUGUI SFXText;

    private bool isPaused = false;
    private PlayerController playerController; // Reference to the player controller script

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>(); // Find the player controller script in the scene

        SFXVolumeSlider.value = AudioManager.instance.sfxVolume;
        musicVolumeSlider.value = AudioManager.instance.musicVolume;
        SFXText.text = (AudioManager.instance.sfxVolume * 100).ToString("F0");
        musicText.text = (AudioManager.instance.musicVolume * 100).ToString("F0");

        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        SFXVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        pauseMenuUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        AudioManager.instance.UnpauseMusic();
        isPaused = false;

        if (playerController != null)
        {
            playerController.enabled = true; // Enable the player controller
        }
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        AudioManager.instance.PauseMusic();
        isPaused = true;

        if (playerController != null)
        {
            playerController.enabled = false; // Disable the player controller
        }
    }

    void SetMusicVolume(float volume)
    {
        AudioManager.instance.SetMusicVolume(volume);
        musicText.text = (volume * 100).ToString("F0");
    }

    void SetSFXVolume(float volume)
    {
        AudioManager.instance.SetSFXVolume(volume);
        SFXText.text = (volume * 100).ToString("F0");
    }

    public void Reload()
    {
        Time.timeScale = 1f;
        AudioManager.instance.UnpauseMusic();
        ScoreManager.instance.ResetScore();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainTitle");
    }

    public void SetScreenMode(int modeIndex)
    {
        // Suppose your dropdown has options in this order:
        // 0 = Fullscreen
        // 1 = Windowed
        // 2 = Borderless Window
        Debug.Log("Screen mode index: " + modeIndex);
        switch (modeIndex)
        {
            case 0: // Fullscreen (Exclusive)
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                Screen.fullScreen = true;
                break;
            case 1: // Windowed
                Screen.fullScreenMode = FullScreenMode.Windowed;
                Screen.fullScreen = false;
                // Set a default resolution and allow resizing
                Screen.SetResolution(1280, 720, false);
                break;
            case 2: // Borderless
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                Screen.fullScreen = true; 
                break;
        }
    }
}
