using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using NUnit.Framework;
using UnityEngine.SocialPlatforms.Impl;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text distanceText;
    [SerializeField] private TMP_Text finalScore;
    [SerializeField] private GameObject gameOverScreen;

    private float distanceTraveled = 0f;
    private bool isGameOver = false;

    private void Start()
    {
        gameOverScreen.SetActive(false); // Hide Game Over screen at start
    }

    private void Update()
    {
        if (!isGameOver)
        {
            distanceTraveled += Time.deltaTime * 8f; // Adjust speed factor as needed
            distanceText.text = distanceTraveled.ToString("F2"); // Show two decimal places
        }

        distanceTraveled += Time.deltaTime * 8f; // Adjust speed factor as needed
        distanceText.text = distanceTraveled.ToString("F2"); // Display up to 2 decimal places

        finalScore.text = distanceTraveled.ToString("F2");
    }

    public void GameOver()
    {
        isGameOver = true;
        SendScoreToLeaderboard();
        gameOverScreen.SetActive(true); // Show Game Over screen
        Time.timeScale = 0f; // Pause the game
    }

    // Button Funcitons:
    public void PlayAgain()
    {
        Time.timeScale = 1f; // Reset time scale
        SceneManager.LoadScene(0);
    }

    private void SendScoreToLeaderboard()
    {
        UGSManager.Instance.AddScore("HighScore", distanceTraveled);
    }

    public void LoadLeaderboard()
    {
        UGSManager.Instance.GetScores("HighScore");
    }

    //public void ShowLeaderboardUI(List<LeaderboardEntry> entries)
    //{

    //}
}
