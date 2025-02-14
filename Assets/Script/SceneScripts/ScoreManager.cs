using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int Score => score;


    private int score = 0;

    public static ScoreManager instance;
    [SerializeField] private TextMeshProUGUI scoreText; // Reference to the UI Text element

    [SerializeField] private TextMeshProUGUI deathScoreText; // Reference to the UI Text element


    private void Start()
    {
        if (scoreText == null || deathScoreText == null)
        {
            Debug.LogError("ScoreManager: UI Text references are not assigned.");
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int scoreValue)
    {
        Debug.Log($"Adding score: {scoreValue}");
        score += scoreValue;
        UpdateScoreText();
    }

    public void ResetScore()
    {
        score = 0;
    }

    public void SetScore(int newScore)
    {
        score = newScore;
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
            deathScoreText.text = "Score: " + score.ToString();
        }
    }
}
