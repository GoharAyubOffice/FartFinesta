using UnityEngine;
using TMPro;

public class CoinPickup : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Reference to the UI Text for displaying score
    private int score = 0; // Variable to track score

    private void Start()
    {
        UpdateScoreUI(); // Initialize the score display
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            Debug.Log("Coin Picked");

            // Destroy the parent GameObject of the collider
            Destroy(other.transform.parent.gameObject);

            score++; // Increment the score
            UpdateScoreUI(); // Update the score on the UI
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
        else
        {
            Debug.LogWarning("Score Text UI not assigned!");
        }
    }
}
