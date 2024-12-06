using UnityEngine;
using TMPro;

public class CoinPickup : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Reference to the UI Text for displaying score
    private int score = 0; // Variable to track score
    [SerializeField] private GameObject featherParticleEffect;
    private void Start()
    {
        UpdateScoreUI(); // Initialize the score display
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            Debug.Log("Coin Picked");

            SpawnParticles(transform.position, featherParticleEffect);

            // Destroy the parent GameObject of the collider
            Destroy(other.transform.parent.gameObject);

            score++; // Increment the score
            UpdateScoreUI(); // Update the score on the UI
        }
    }
    public void SpawnParticles(Vector3 position, GameObject particlePrefab)
    {
        // Instantiate the particle effect at the given position
        GameObject particles = Instantiate(particlePrefab, position, Quaternion.identity);

        // Destroy the particle effect after a short duration
        Destroy(particles, 2f); // Adjust "2f" to the duration of your particle effect
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
