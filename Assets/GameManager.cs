using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;


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
        }
        else
        {
            Destroy(gameObject); // Prevents duplicates
        }
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
    }
}
