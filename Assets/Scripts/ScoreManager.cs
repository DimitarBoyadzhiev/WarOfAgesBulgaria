using LootLocker.Requests;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public int Score { get; private set; }
    public TextMeshProUGUI scoreText;
    public Leaderboard leaderboard;

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

        leaderboard = GameObject.Find("LeaderboardMenu").GetComponent<Leaderboard>();

    }

    private void Start()
    {

        StartCoroutine(SetupRoutine());
    }

    private void OnLevelWasLoaded(int level)
    {
        leaderboard = GameObject.Find("LeaderboardGame").GetComponent<Leaderboard>();
    }

    private void Update()
    {
        if (scoreText == null)
        {
            FindScoreComponents();
        }
    }

    IEnumerator SetupRoutine()
    {
        yield return LoginRoutine();
        yield return leaderboard.FetchTopHighscoresRoutine();

    }

    IEnumerator LoginRoutine()
    {
        bool done = false;
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                Debug.Log("Player logged in!");
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                done = true;
            }
            else
            {
                Debug.Log("Failed to start session");
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    void FindScoreComponents()
    {
        Canvas canvas = FindAnyObjectByType<Canvas>(); // Find the Canvas
        if (canvas != null)
        {
            Transform scoreTextTransform = canvas.transform.Find("ScoreText");

            if (scoreTextTransform != null)
            {
                scoreText = scoreTextTransform.GetComponent<TextMeshProUGUI>();
                UpdateScoreUI();
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
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        yield return leaderboard.SubmitScoreRoutine(Score);
        yield return leaderboard.FetchTopHighscoresRoutine();
    }
}
