using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Score")]
    public int score = 0;
    public int highscore = 0;

    [Header("Magic Runes")]
    public int runesCollected = 0;
    public int runesRequired = 4;

    [Header("UI")]
    public TMP_Text scoreText;
    public TMP_Text highscoreText;
    public TMP_Text runesText;

    private void Awake()
    {
        // Singleton: only one GameManager can exist
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        highscore = PlayerPrefs.GetInt("Highscore", 0);
    }

    private void Start()
    {
        UpdateScoreUI();
        UpdateRunesUI();
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
    }

    public void CollectRune(WitchMovement witch)
    {
        runesCollected++;

        UpdateRunesUI();

        Debug.Log("Runes collected: " + runesCollected
                  + "/" + runesRequired);

        if (runesCollected >= runesRequired)
        {
            witch.magicLeapUnlocked = true;
            Debug.Log("MAGIC LEAP UNLOCKED!");
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;

        if (highscoreText != null)
            highscoreText.text = "Best: " + highscore;
    }

    private void UpdateRunesUI()
    {
        if (runesText != null)
            runesText.text = "Runes: " + runesCollected
                           + "/" + runesRequired;
    }
}
