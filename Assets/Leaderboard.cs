using LootLocker.Requests;
using System;
using System.Collections;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    public static Leaderboard instance;

    [HideInInspector]
    public string playerNamesText;
    [HideInInspector]
    public string playerScoresText;

    public event Action OnLoadFetchFinished;

    private int highScoresCount = 10; //in FetchTopHighscoresRoutine - determines the count of highscores to display
    private string leaderboardKey = "score";

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
        //gameObject.SetActive(false);
    }

    private void Start()
    {
        FetchLeaderBoard();
    }

    private void SubmitScorre()
    {
        StartCoroutine(SubmitScoreRoutine(ScoreManager.Instance.Score));
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

    public void FetchLeaderBoard()
    {
        StartCoroutine(FetchTopHighscoresRoutine());
    }


    float timeout = 10f;
    float timer = 0f;
    //Get selected number of highscores from online leaderboard
    private IEnumerator FetchTopHighscoresRoutine()
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
                playerNamesText = tempPlayerNames;
                playerScoresText = tempPlayerScores;
                OnLoadFetchFinished?.Invoke();
            }
            else
            {
                Debug.Log("Failed" + response.errorData.message);
                done = true;
            }
        });

        yield return new WaitWhile(() =>
        {
            timer += Time.deltaTime;
            return !done && timer < timeout;
        });

    }


}
