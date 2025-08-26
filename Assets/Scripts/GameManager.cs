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
    [SerializeField] private TMP_Text countdownText;             // Text to show "3", "2", "1", "GO!"
    [SerializeField] private float resumeCountdownSeconds = 3f;  // countdown length

    private float distanceTraveled = 0f;
    private bool isGameOver = false;
    private float invincible = 0.0f;
    // Internal state
    private bool isPaused = false;
    private bool isCountingDown = false;

    private void Start()
    {
        gameOverScreen.SetActive(false); // Hide Game Over screen at start
        LeaderboardScreen.SetActive(false);
        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (countdownText != null) countdownText.gameObject.SetActive(false);
    }

    private void Update()
    {
        invincible -= Time.unscaledDeltaTime;

        // Toggle pause with ESC (don't allow toggling if game over or while countdown)
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver && !isCountingDown)
        {
            if (isPaused) HidePauseMenu();
            else ShowPauseMenu();
        }

        // Update distance only when not game over and not paused (use unscaled time for UI consistency)
        if (!isGameOver && !isPaused)
        {
            distanceTraveled += Time.deltaTime * 8f; // Adjust speed factor as needed
            distanceText.text = distanceTraveled.ToString("F2"); // Show two decimal places
        }


        // Keep final score updated (works even paused; it's just a display)
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
        // If already counting down or not paused, ignore
        if (isCountingDown) return;

        // If we are not currently paused, still run the countdown (useful if called from HUD)
        StartCoroutine(CountdownAndResume());
    }

    private IEnumerator CountdownAndResume()
    {
        isCountingDown = true;
        // Ensure pause menu is hidden during the countdown (or keep it visible and show overlay; here we hide it)
        if (pauseMenu != null) pauseMenu.SetActive(true); // keep menu visible so player knows it's coming from pause
        if (countdownText != null) countdownText.gameObject.SetActive(true);

        // Use realtime waiting because Time.timeScale is 0 while paused
        float remaining = resumeCountdownSeconds;
        while (remaining > 0f)
        {
            // show integer countdown (3,2,1)
            if (countdownText != null)
                countdownText.text = Mathf.CeilToInt(remaining).ToString();

            yield return new WaitForSecondsRealtime(1f);
            remaining -= 1f;
        }

        // show GO!
        if (countdownText != null) countdownText.text = "GO!";
        yield return new WaitForSecondsRealtime(0.6f);

        // hide UI & resume game
        if (countdownText != null) countdownText.gameObject.SetActive(false);
        if (pauseMenu != null) pauseMenu.SetActive(false);

        // resume time and gameplay
        Time.timeScale = 1f;
        isPaused = false;
        isCountingDown = false;

        // give short invincibility so player doesn't die immediately after resuming
        invincible = 1.5f;
    }

    // Called by your HUD Menu Button (top-right). This toggles pause when not paused; 
    // if menu is active and player clicks it again, it will start the resume countdown.
    public void OnHudMenuButtonPressed()
    {
        if (isGameOver || isCountingDown) return;

        if (!isPaused)
        {
            ShowPauseMenu();
        }
        else
        {
            // if already paused, clicking HUD Menu should behave like Resume
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
