using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    private TMP_InputField inputField;
    private Button submitButton;
    [SerializeField] private HighscoreTable highscoreTable;

    private void Awake()
    {
        inputField = transform.Find("NameInputField").GetComponent<TMP_InputField>();
        submitButton = transform.Find("SubmitButton").GetComponent<Button>();
    }

    public void Submit()
    {
        int score = ScoreManager.Instance.Score;
        string name = inputField.text;

        highscoreTable.AddEntry(score, name);
    }
}
