using UnityEngine;
using LootLocker.Requests;
using System.Collections;
using TMPro;

public class Leaderboard : MonoBehaviour
{

    public TextMeshProUGUI playerNames;
    public TextMeshProUGUI playerScores;

    private int highScoresCount = 10; //in FetchTopHighscoresRoutine - determines the count of highscores to display
    private string leaderboardKey = "global_highscore";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    //Submit player's score to the online leaderboard
    public IEnumerator SubmitScoreRoutine(int scoreToSubmit)
    {
        bool done = false;
        string playerID = PlayerPrefs.GetString("PlayerID");
        LootLockerSDKManager.SubmitScore(playerID, scoreToSubmit, leaderboardKey, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Success");
                done = true;
            }
            else
            {
                Debug.Log("Failed " + response.errorData.message);
                done = true;
            }
        });

        yield return new WaitWhile(() => done == false);
    }

    //Get selected number of highscores from online leaderboard
    public IEnumerator FetchTopHighscoresRoutine()
    {
        bool done = false;
        LootLockerSDKManager.GetScoreList(leaderboardKey, highScoresCount, (response) =>
        {
            if (response.success)
            {
                string tempPlayerNames = "Names\n";
                string tempPlayerScores = "Scores\n";

                LootLockerLeaderboardMember[] members = response.items;

                for (int i = 0;  i< members.Length; i++)
                {
                    tempPlayerNames += members[i].rank + ". ";
                    if (members[i].player.name != "")
                    {
                        tempPlayerNames += members[i].player.name;
                    }
                    else
                    {
                        tempPlayerNames += members[i].player.id;
                    }
                    tempPlayerScores += members[i].score + "\n";
                    tempPlayerNames += "\n";
                }
                done = true;
                playerNames.text = tempPlayerNames;
                playerScores.text = tempPlayerScores;
            }
            else
            {
                Debug.Log("Failed" + response.errorData.message);
                done = true;
            }
        });

        yield return new WaitWhile(() => done == false);

    }


}
