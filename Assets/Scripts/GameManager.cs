using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using NUnit.Framework;
using UnityEngine.SocialPlatforms.Impl;
using System.Collections.Generic;
using Unity.Services.Leaderboards.Models;
using System.Collections;

public class GameManager : MonoBehaviour
{
    // Regular HUD UI Elements
    [Header("HUD UI")]
    [SerializeField] private TMP_Text distanceText;
    [SerializeField] private TMP_Text finalScore;
    [SerializeField] private GameObject gameOverScreen;

    // Leaderboard UI Elements
    [Header("Leaderboard UI")]
    [SerializeField] private GameObject LeaderboardScreen;
    [SerializeField] private TMP_Text[] namesText;
    [SerializeField] private TMP_Text[] scoresText;

    // Pause Menu UI Elements
    [Header("Pause Menu UI")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private TMP_Text countdownText;             // Text Template
    [SerializeField] private float resumeCountdownSeconds = 3f;  // Countdown length

    private float distanceTraveled = 0f;
    private bool isGameOver = false;
    private float invincible = 0.0f;
    // Internal state
    private bool isPaused = false;
    private bool isCountingDown = false;

    private void Start()
    {
        gameOverScreen.SetActive(false); 
        LeaderboardScreen.SetActive(false);
        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (countdownText != null) countdownText.gameObject.SetActive(false);
    } // Hide UI screens at start

    private void Update()
    {
        invincible -= Time.unscaledDeltaTime;

        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver && !isCountingDown)
        {
            if (isPaused) HidePauseMenu();
            else ShowPauseMenu();
        }

        // Update distance only when not game over and not paused
        if (!isGameOver && !isPaused)
        {
            distanceTraveled += Time.deltaTime * 8f;
            distanceText.text = distanceTraveled.ToString("F2");
        }

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
        gameOverScreen.SetActive(false); // Hide Game Over screen
        Time.timeScale = 1f; // Play the game
        invincible = 2.0f;
    }


    // Pause Menu Methods:
    private void ShowPauseMenu()
    {
        if (isGameOver || isCountingDown) return;

        isPaused = true;
        if (pauseMenu != null) pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    private void HidePauseMenu()
    {
        if (isCountingDown) return;

        isPaused = false;
        if (pauseMenu != null) pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    // Called by the Resume button (and by the Pause HUD Menu button when used to resume)
    public void ResumeWithCountdown()
    {
        if (isCountingDown) return;

        // If we are not currently paused, still run the countdown (useful if called from HUD)
        StartCoroutine(CountdownAndResume());
    }

    private IEnumerator CountdownAndResume()
    {
        isCountingDown = true;

        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (countdownText != null) countdownText.gameObject.SetActive(true);


        float remaining = resumeCountdownSeconds;
        while (remaining > 0f)
        {
            if (countdownText != null)
                countdownText.text = Mathf.CeilToInt(remaining).ToString();

            yield return new WaitForSecondsRealtime(1f);
            remaining -= 1f;
        }

        if (countdownText != null) countdownText.text = "GO!";
        yield return new WaitForSecondsRealtime(0.6f);

        // Hide UI & resume game
        if (countdownText != null) countdownText.gameObject.SetActive(false);
        if (pauseMenu != null) pauseMenu.SetActive(false);

        // Resume time and gameplay as well as scores
        Time.timeScale = 1f;
        isPaused = false;
        isCountingDown = false;

        invincible = 1.5f; // Invincibility Frames
    }

    public void OnHudMenuButtonPressed()
    {
        if (isGameOver || isCountingDown) return;

        if (!isPaused)
        {
            ShowPauseMenu();
        }
        else
        {
            ResumeWithCountdown();
        }
    }



    // Button Funcitons:

    // Main Menu (scene 0)
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void PlayAgain()
    {
        Time.timeScale = 1f; // Reset time scale
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
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
