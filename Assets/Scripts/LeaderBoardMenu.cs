using TMPro;
using UnityEngine;

public class LeaderBoardMenu : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerNames;
    [SerializeField]
    private TextMeshProUGUI playerScores;

    private void Start()
    {
        Leaderboard.instance.OnLoadFetchFinished += SetTexts;
    }

    private void OnDestroy()
    {
        Leaderboard.instance.OnLoadFetchFinished -= SetTexts;
    }

    public void SetTexts()
    {
        playerNames.text = Leaderboard.instance.playerNamesText;
        playerScores.text = Leaderboard.instance.playerScoresText;
    }
}
