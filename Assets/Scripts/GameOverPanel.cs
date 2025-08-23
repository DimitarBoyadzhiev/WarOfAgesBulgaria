using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    private TMP_InputField inputField;
    private Button submitButton;
    [SerializeField] private HighscoreTable highscoreTable;
    private int score;

    private void Awake()
    {
        inputField = transform.Find("NameInputField").GetComponent<TMP_InputField>();
        submitButton = transform.Find("SubmitButton").GetComponent<Button>();
    }

    public void Submit()
    {
        score = ScoreManager.Instance.Score;
        string name = inputField.text;

        highscoreTable.AddEntry(score, name);
    }
}