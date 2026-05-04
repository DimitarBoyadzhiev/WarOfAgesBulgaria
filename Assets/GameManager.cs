using LootLocker.Requests;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public GameObject NameInputPanel;

    public TMP_InputField playerNameInputField;


    //Gold generation logic
    public int gold;
    public float cooldown;
    float lastGenerated;
    int goldAmount = 2;



    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to sceneLoaded event
        }
        else
        {
            Destroy(gameObject); // Prevents duplicates
        }
        gameObject.SetActive(false);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Level01") // Check if Level01 is loaded
        {
            gameObject.SetActive(true); // Activate GameObject
            Time.timeScale = 1f;
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe to prevent memory leaks
    }

    private void Update()
    {
        GenerateGold();
    }

    public void AddGold(int gold)
    {
        this.gold += gold;
    }

    void GenerateGold()
    {
        if (Time.time - lastGenerated < cooldown)
        {
            return;
        }
        lastGenerated = Time.time;
        gold += goldAmount;
    }

    public void EndGame()
    {
        ScoreManager.Instance.GameOver();
        NameInputPanel.SetActive(true);
    }

    public void DestroyGameManager()
    {
        Destroy(gameObject);
    }

    private void GetLeaderBoard()
    {
        ScoreManager.Instance.FetchLeaderBoard();
    }

    public void OnSubmit()
    {
        StartCoroutine(SetPlayerName());
        GetLeaderBoard();
    }

    float timeout = 10f;
    float timer = 0f;
    //Submit player name to leaderboard
    private IEnumerator SetPlayerName()
    {
        bool done = false;
        LootLockerSDKManager.SetPlayerName(playerNameInputField.text, (response) =>
        {
            if (response.success)
            {
                done = true;
                Debug.Log("Succesfully set player name!");
            }
            else
            {
                Debug.Log("Could not set player name " + response.errorData.message);
            }
        });

        yield return new WaitWhile(() =>
        {
            timer += Time.deltaTime;
            return !done && timer < timeout;
        });
    }
}
