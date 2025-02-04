using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{
    public TextMeshProUGUI Text;
    [SerializeField] private PlayerController player;
    [SerializeField] private GameObject PausedText;

    //private int score = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Text.text = "Vida: " + player.Health.ToString();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void PauseGame()
    {
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        PausedText.SetActive(!PausedText.activeSelf);
    }

}