using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    private TMP_InputField inputField;
    private Button submitButton;
    public enum DatabaseType { MySQL }
    [SerializeField] private HighscoreTable highscoreTable;
    [SerializeField] private string dbConnectionString = "Server=sql7.freesqldatabase.com:3306;Database=sql7768970;User ID=sql7768970;Password=V939TvZavH;";
    [SerializeField] private DatabaseType dbType = DatabaseType.MySQL;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private int maxHighscoresToDisplay = 10;

    private bool isSubmitting = false;

    private void Awake()
    {
        inputField = transform.Find("NameInputField").GetComponent<TMP_InputField>();
        submitButton = transform.Find("SubmitButton").GetComponent<Button>();
        if (statusText == null)
        {
            Transform statusTextTransform = transform.Find("StatusText");
            if (statusTextTransform != null)
            {
                statusText = statusTextTransform.GetComponent<TextMeshProUGUI>();
            }
        }
    }

    public void Submit()
    {
        int score = ScoreManager.Instance.Score;
        string name = inputField.text;
        if (string.IsNullOrWhiteSpace(name))
        {
            name = "Anonymous";
        }
        
        // Sanitize input to prevent SQL injection
        name = SanitizeInput(name);
        
        // Start submission coroutine
        StartCoroutine(SubmitScore(score, name));

    }
    private IEnumerator SubmitScore(int score, string playerName)
    {
        isSubmitting = true;
        
        if (statusText != null)
        {
            statusText.text = "Submitting score...";
            statusText.gameObject.SetActive(true);
        }
        
        bool success = false;
        string errorMessage = "";
        
        // Use a separate thread for database operations
        yield return new WaitForEndOfFrame();
        
        try
        {
            // Use remote database
            success = InsertScoreToRemoteDB(score, playerName);
        }
        catch (Exception ex)
        {
            Debug.LogError("Database error: " + ex.Message);
            errorMessage = ex.Message;
            success = false;
        }
        
        // Update UI based on result
        if (success)
        {
            if (statusText != null)
            {
                statusText.text = "Score submitted successfully!";
            }
            
            // Refresh the highscore table if available
            if (highscoreTable != null)
            {
                highscoreTable.RefreshHighscores();
            }
        }
        else
        {
            if (statusText != null)
            {
                statusText.text = "Failed to submit score: " + (string.IsNullOrEmpty(errorMessage) ? "Unknown error" : errorMessage);
            }
             // Still add to local scores if database fails
            if (highscoreTable != null)
            {
                highscoreTable.AddEntry(score, playerName);
            }
        }
        
        isSubmitting = false;
        
        // Hide status text after a delay
        yield return new WaitForSeconds(3.0f);
        
        if (statusText != null)
        {
            statusText.gameObject.SetActive(false);
        }
    }
    private bool InsertScoreToRemoteDB(int score, string playerName)
    {
        IDbConnection dbConnection = null;
        IDbCommand dbCommand = null;
        
        try
        {
            // Create the appropriate connection based on database type
            switch (dbType)
            {
                case DatabaseType.MySQL:
                    dbConnection = new MySqlConnection(dbConnectionString);
                    break;
                default:
                    Debug.LogError("Unsupported remote database type");
                    return false;
            }
            
            // Open connection
            dbConnection.Open();
            
            // Create command
            dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = "INSERT INTO Scores (name, score, date_time) VALUES (@PlayerName, @Score, @Date)";
            
            // Add parameters (prevents SQL injection)
            IDbDataParameter nameParam = dbCommand.CreateParameter();
            nameParam.ParameterName = "@PlayerName";
            nameParam.Value = playerName;
            dbCommand.Parameters.Add(nameParam);
            
            IDbDataParameter scoreParam = dbCommand.CreateParameter();
            scoreParam.ParameterName = "@Score";
            scoreParam.Value = score;
            dbCommand.Parameters.Add(scoreParam);
            
            IDbDataParameter dateParam = dbCommand.CreateParameter();
            dateParam.ParameterName = "@Date";
            dateParam.Value = DateTime.Now;
            dbCommand.Parameters.Add(dateParam);
            
            // Execute the command
            int rowsAffected = dbCommand.ExecuteNonQuery();
            
            return rowsAffected > 0;
        }
        finally
        {
            // Clean up
            if (dbCommand != null)
            {
                dbCommand.Dispose();
            }
            
            if (dbConnection != null && dbConnection.State != ConnectionState.Closed)
            {
                dbConnection.Close();
                dbConnection.Dispose();
            }
        }
    }

    private string SanitizeInput(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }
        
        return input.Replace("'", "''")
                   .Replace(";", "")
                   .Replace("--", "")
                   .Replace("/*", "")
                   .Replace("*/", "")
                   .Replace("xp_", "");
    }
}
