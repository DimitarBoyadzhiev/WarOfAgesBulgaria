using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public int Score { get; private set; }
    public TextMeshProUGUI scoreText;

    public GameObject gameOverPanel; 
    public TextMeshProUGUI finalScoreText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps this GameObject active across scenes
        }
        else
        {
            Destroy(gameObject); // Prevents duplicates
        }
    }

    private void Update()
    {
        if (scoreText == null)
        {
            FindScoreComponents();
        }
    }

    void FindScoreComponents()
    {
        Canvas canvas = FindAnyObjectByType<Canvas>(); // Find the Canvas
        if (canvas != null)
        {
            Transform scoreTextTransform = canvas.transform.Find("ScoreText");
            Transform gameOverPanelTransform = canvas.transform.Find("GameOverPanel");
            Transform finalScoreTextTransform = gameOverPanelTransform != null ? gameOverPanelTransform.Find("FinalScoreText") : null;

            if (scoreTextTransform != null)
            {
                scoreText = scoreTextTransform.GetComponent<TextMeshProUGUI>();
                UpdateScoreUI();
            }

            if (gameOverPanelTransform != null)
            {
                gameOverPanel = gameOverPanelTransform.gameObject;
            }

            if (finalScoreTextTransform != null)
            {
                finalScoreText = finalScoreTextTransform.GetComponent<TextMeshProUGUI>();
            }
        }
    }


    public void AddScore(int points)
    {
        Score += points;
        UpdateScoreUI();
    }

    public void ResetScore()
    {
        Score = 0;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        
        if (scoreText != null)
        {
            scoreText.text = "Score: " + Score;
        }
        else
        {
            scoreText.text = "Score: 0";
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0f; // Pause the game
        gameOverPanel.SetActive(true); // Show the game over panel
        finalScoreText.text = "Final Score: " + Score; // Display final score
    }
}
