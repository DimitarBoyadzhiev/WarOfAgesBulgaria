using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keeps this GameObject active across scenes
        }
        else
        {
            Destroy(gameObject); // Prevents duplicates
        }
    }

    public void EndGame()
    {
        ScoreManager.Instance.GameOver();
    }
}
