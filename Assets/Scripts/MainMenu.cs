using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Level01");
    }
    
    public void FetchLeaderBoard()
    {
        Leaderboard.instance.FetchLeaderBoard();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
