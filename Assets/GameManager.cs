using UnityEngine;

public class GameManager : MonoBehaviour
{
    public void EndGame()
    {
        ScoreManager.Instance.GameOver();
    }
}
