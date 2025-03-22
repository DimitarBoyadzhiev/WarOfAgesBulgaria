using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Data;
using MySql.Data.MySqlClient;

public class HighscoreTable : MonoBehaviour
{

    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> highscoreEntryTransformList;
    [SerializeField] private bool useRemoteDatabase = true;
    [SerializeField] private string dbConnectionString = "-";
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private int maxHighscoresToDisplay = 10;


    private void Awake()
    {

        entryContainer = transform.Find("highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        highscoreEntryTransformList = new List<Transform>();
        InitializeHighscoreTable();

    }

    private void InitializeHighscoreTable()
    {
        ClearHighscoreEntries();
        
        if (useRemoteDatabase)
        {
            StartCoroutine(LoadHighscoresFromDatabase());
        }
        else
        {
            LoadLocalHighscores();
        }
    }
    private void LoadLocalHighscores()
    {
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
        
        // Check if we have valid data
        if (highscores == null || highscores.highscoreEntryList == null)
        {
            highscores = new Highscores
            {
                highscoreEntryList = new List<HighscoreEntry>()
            };
        }
        
        // Sort the highscores
        SortHighscores(highscores.highscoreEntryList);
        
        // Display highscores
        DisplayHighscores(highscores.highscoreEntryList);
    }

    private IEnumerator LoadHighscoresFromDatabase()
    {
        if (statusText != null)
        {
            statusText.text = "Loading highscores...";
            statusText.gameObject.SetActive(true);
        }
        
        List<HighscoreEntry> highscoreEntries = new List<HighscoreEntry>();
        bool success = false;
        string errorMessage = "";
        
        yield return new WaitForEndOfFrame();
        
        try
        {
            using (MySqlConnection connection = new MySqlConnection(dbConnectionString))
            {
                connection.Open();
                
                using (MySqlCommand command = new MySqlCommand("SELECT name, score, date_time FROM Scores ORDER BY score DESC LIMIT @Limit", connection))
                {
                    command.Parameters.AddWithValue("@Limit", maxHighscoresToDisplay);
                    
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader.GetString("name");
                            int score = reader.GetInt32("score");
                            DateTime dateTime = reader.GetDateTime("date_time");
                            
                            HighscoreEntry entry = new HighscoreEntry
                            {
                                name = name,
                                score = score,
                                dateTime = dateTime
                            };
                            
                            highscoreEntries.Add(entry);
                        }
                    }
                }
            }
            
            success = true;
        }
        catch (Exception ex)
        {
            Debug.LogError("Database error: " + ex.Message);
            errorMessage = ex.Message;
            success = false;
        }
        
        if (success)
        {
            if (statusText != null)
            {
                statusText.text = "Highscores loaded successfully!";
            }
            
            // Display the highscores
            DisplayHighscores(highscoreEntries);
        }
        else
        {
            if (statusText != null)
            {
                statusText.text = "Failed to load highscores: " + 
                    (string.IsNullOrEmpty(errorMessage) ? "Unknown error" : errorMessage);
            }
            
            // Fall back to local highscores
            LoadLocalHighscores();
        }
        
        // Hide status text after a delay
        yield return new WaitForSeconds(3.0f);
        
        if (statusText != null)
        {
            statusText.gameObject.SetActive(false);
        }
    }

    public void RefreshHighscores()
    {
        InitializeHighscoreTable();
    }

    private void SortHighscores(List<HighscoreEntry> highscoreList)
    {
        for (int i = 0; i < highscoreList.Count; i++)
        {
            for (int j = i + 1; j < highscoreList.Count; j++)
            {
                if (highscoreList[j].score > highscoreList[i].score)
                {
                    HighscoreEntry temp = highscoreList[i];
                    highscoreList[i] = highscoreList[j];
                    highscoreList[j] = temp;
                }
            }
        }
    }

    private void DisplayHighscores(List<HighscoreEntry> highscoreList)
    {
        ClearHighscoreEntries();
        
        foreach (var entry in highscoreList)
        {
            CreateHighscoreEntryTransform(entry, entryContainer, highscoreEntryTransformList);
        }
    }
    
    private void ClearHighscoreEntries()
    {
        // Remove old entries
        foreach (Transform entry in highscoreEntryTransformList)
        {
            Destroy(entry.gameObject);
        }
        
        highscoreEntryTransformList.Clear();
    }

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 100f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);
        
        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            default: rankString = rank + "th"; break;
            case 1: rankString = "1st"; break;
            case 2: rankString = "2nd"; break;
            case 3: rankString = "3rd"; break;
        }
        
        entryTransform.Find("posText").GetComponent<TextMeshProUGUI>().text = rankString;
        entryTransform.Find("scoreText").GetComponent<TextMeshProUGUI>().text = highscoreEntry.score.ToString();
        entryTransform.Find("nameText").GetComponent<TextMeshProUGUI>().text = highscoreEntry.name;
        
        // Optional: Show date/time for remote scores
        if (useRemoteDatabase && entryTransform.Find("dateText") != null)
        {
            entryTransform.Find("dateText").GetComponent<TextMeshProUGUI>().text = 
                highscoreEntry.dateTime.ToString("MM/dd/yyyy");
        }
        
        entryTransform.Find("background").gameObject.SetActive(rank % 2 == 1);
        
        if (rank == 1)
        {
            entryTransform.Find("posText").GetComponent<TextMeshProUGUI>().color = Color.green;
            entryTransform.Find("scoreText").GetComponent<TextMeshProUGUI>().color = Color.green;
            entryTransform.Find("nameText").GetComponent<TextMeshProUGUI>().color = Color.green;
        }
        
        transformList.Add(entryTransform);
    }

    public void AddEntry(int score, string name)
    {
        // Add to local high scores
        AddHighscoreEntry(score, name);
        
        // Refresh the display
        if (!useRemoteDatabase)
        {
            RefreshHighscores();
        }
        // If using remote database, the GameOverPanel will handle the database insertion
    }

    private void AddHighscoreEntry(int score, string name)
    {
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = name, dateTime = DateTime.Now };
        
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
        
        if (highscores == null)
        {
            highscores = new Highscores
            {
                highscoreEntryList = new List<HighscoreEntry>()
            };
        }
        
        if (highscores.highscoreEntryList == null)
        {
            highscores.highscoreEntryList = new List<HighscoreEntry>();
        }
        
        highscores.highscoreEntryList.Add(highscoreEntry);
        
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }

    [System.Serializable] private class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList;
    }
    
    [System.Serializable] private class HighscoreEntry
    {
        public int score;
        public string name;
        public DateTime dateTime;
    }

}
