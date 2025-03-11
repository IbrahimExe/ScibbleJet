using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text distanceText;
    private float distanceTraveled = 0f;

    private void Start()
    {
       
    }

    private void Update()
    {
            distanceTraveled += Time.deltaTime * 8f; // Adjust speed factor as needed
            distanceText.text = distanceTraveled.ToString("F2"); // Display up to 2 decimal places
    }
}
