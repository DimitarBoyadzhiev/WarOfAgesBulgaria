using LootLocker.Requests;
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
            DontDestroyOnLoad(gameObject); // Keeps this GameObject active across scenes
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
        }
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

    //Submit player name to leaderboard
    public void SetPlayerName()
    {
        LootLockerSDKManager.SetPlayerName(playerNameInputField.text, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Succesfully set player name!");
            }
            else
            {
                Debug.Log("Could not set player name " + response.errorData.message);
            }
        });
    }
}
