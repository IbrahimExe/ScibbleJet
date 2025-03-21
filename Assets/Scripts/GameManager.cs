using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using NUnit.Framework;
using UnityEngine.SocialPlatforms.Impl;
using System.Collections.Generic;
using Unity.Services.Leaderboards.Models;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text distanceText;
    [SerializeField] private TMP_Text finalScore;
    [SerializeField] private GameObject gameOverScreen;

    // Leaderboard UI Elements
    [Header("Leaderboard UI")]
    [SerializeField] private GameObject LeaderboardScreen;
    [SerializeField] private TMP_Text[] namesText;
    [SerializeField] private TMP_Text[] scoresText;

    private float distanceTraveled = 0f;
    public bool isGameOver = false;
    private float invincible = 0.0f;

    private void Start()
    {
        gameOverScreen.SetActive(false); // Hide Game Over screen at start
        LeaderboardScreen.SetActive(false);
    }

    private void Update()
    {
        invincible -= Time.deltaTime;

        if (!isGameOver)
        {
            distanceTraveled += Time.deltaTime * 8f; // Adjust speed factor as needed
            distanceText.text = distanceTraveled.ToString("F2"); // Show two decimal places
        }

        distanceTraveled += Time.deltaTime * 8f; // Adjust speed factor as needed
        distanceText.text = distanceTraveled.ToString("F2"); // Display up to 2 decimal places

        finalScore.text = distanceTraveled.ToString("F2");


        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            PlayAgain();
        }
    }

    public void GameOver()
    {
        if (invincible <= 0.0f)
        {
            isGameOver = true;
            SendScoreToLeaderboard();
            gameOverScreen.SetActive(true); // Show Game Over screen
            Time.timeScale = 0f; // Pause the game
        }
    }

    public void GameContinue()
    {
        isGameOver = false;
        gameOverScreen.SetActive(false); // Show Game Over screen
        Time.timeScale = 1f; // Pause the game
        invincible = 2.0f;
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
        LeaderboardScreen.SetActive(true);
    }

    public void LeaderboardBack()
    {
        LeaderboardScreen.SetActive(false);
    }

    public void ShowLeaderboardUI(List<LeaderboardEntry> entries)
    {
        for (int i = 0; i < scoresText.Length; i++)
        {
            {
                if(entries.Count <= i)
                {
                    scoresText[i].text = "";
                    namesText[i].text = "";
                }
                else
                {
                    scoresText[i].text = entries[i].Score.ToString("#.00");
                    namesText[i].text = entries[i].PlayerName.ToString().Split('#')[0];
                }
            }
        }
    }
}
