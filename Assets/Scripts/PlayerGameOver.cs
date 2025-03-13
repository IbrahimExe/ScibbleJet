using UnityEngine;

public class PlayerGameOver : MonoBehaviour
{
    [SerializeField] private GameManager gameManager; // Assign in Inspector

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            gameManager.GameOver();
        }
    }
}
