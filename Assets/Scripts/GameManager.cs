using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text distanceText;
    [SerializeField] private GameObject gameOverScreen;

    private float distanceTraveled = 0f;
    private bool isGameOver = false;

    private void Start()
    {
        gameOverScreen.SetActive(false); // Hide Game Over screen at start
    }

    private void FixedUpdate()
    {
        if (!isGameOver)
        {
            distanceTraveled += Time.deltaTime * 8f; // Adjust speed factor as needed
            distanceText.text = distanceTraveled.ToString("F2"); // Show two decimal places
        }

        distanceTraveled += Time.deltaTime * 8f; // Adjust speed factor as needed
        distanceText.text = distanceTraveled.ToString("F2"); // Display up to 2 decimal places
    }

    public void GameOver()
    {
        isGameOver = true;
        gameOverScreen.SetActive(true); // Show Game Over screen
        Time.timeScale = 0f; // Pause the game
    }

    // Button Funcitons:
    public void PlayAgain()
    {
        Time.timeScale = 1f; // Reset time scale
        SceneManager.LoadScene(0);
    }
}
