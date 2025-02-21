using UnityEngine;

public class LevelStartupManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.instance.PlayMusic(AudioManager.instance.inGameMusic);
    }
}
