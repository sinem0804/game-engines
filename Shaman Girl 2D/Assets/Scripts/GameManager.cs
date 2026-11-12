using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Score")]
    public int score = 0;
    public int highscore = 0;

    [Header("UI")]
    public TMP_Text scoreText;
    public TMP_Text highscoreText;

    private void Awake()
    {
        // Singleton: only one GameManager can exist
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Load the saved highscore
        highscore = PlayerPrefs.GetInt("Highscore", 0);
    }

    private void Start()
    {
        UpdateScoreUI();
    }

    public void AddScore(int points)
    {
        score += points;

        if (score > highscore)
        {
            highscore = score;
            PlayerPrefs.SetInt("Highscore", highscore);
            PlayerPrefs.Save();
        }

        UpdateScoreUI();

        Debug.Log("Score: " + score);
        Debug.Log("Highscore: " + highscore);
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;

        if (highscoreText != null)
            highscoreText.text = "Best: " + highscore;
    }
}
