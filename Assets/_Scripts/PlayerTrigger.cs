using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerTrigger : MonoBehaviour
{
    public Animator playerAnimator; // Reference to the player's Animator component
    [SerializeField] private Transform player;
    [SerializeField] private AudioSource audioSource;
    public GameObject finishUI; // Reference to the finish UI Canvas
    public TextMeshProUGUI congratsText; // Reference to the congratulations TextMeshPro
    public AudioClip clappingSound; // Reference to the clapping sound clip

    public GameManager gameManager;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        player = GetComponent<Transform>();
        audioSource = GetComponent<AudioSource>();

        if (finishUI != null)
        {
            // Ensure the finish UI is initially inactive
            finishUI.SetActive(false);
        }
        else
        {
            Debug.LogError("Finish UI GameObject is not assigned.");
        }

        if (congratsText == null)
        {
            Debug.LogError("CongratsText is not assigned.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("finish"))
        {
            // Stop player movement
            var joystickPlayer = GetComponent<JoystickPlayerExample>();
            if (joystickPlayer != null)
            {
                joystickPlayer.enabled = false;
            }
            else
            {
                Debug.LogWarning("JoystickPlayerExample script not found.");
            }

            var fartPropulsion = GetComponent<FartPropulsion>();
            if (fartPropulsion != null)
            {
                fartPropulsion.enabled = false;
            }
            else
            {
                Debug.LogWarning("FartPropulsion script not found.");
            }

            // Set the win animation
            if (playerAnimator != null)
            {
                playerAnimator.SetTrigger("Win");
                playerAnimator.SetBool("isWalking", false);
            }
            else
            {
                Debug.LogError("Player Animator is not assigned.");
            }

            if (gameManager != null)
            {
                gameManager.FinishGame();
            }
            else
            {
                Debug.LogError("GameManager is not assigned.");
            }

            // Play the clapping sound
            if (audioSource != null && clappingSound != null)
            {
                audioSource.PlayOneShot(clappingSound);
            }
            else
            {
                Debug.LogError("AudioSource or clappingSound is not assigned.");
            }

            // Start coroutine to show finish UI after a delay
            StartCoroutine(ShowFinishUIAfterDelay(1f)); // Adjust the delay as needed

            congratsText.text = "Congratulations!";

            Debug.Log("Player reached the finish point!");
        }
    }

    IEnumerator ShowFinishUIAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        if (finishUI != null)
        {
            // Activate the finish UI
            finishUI.SetActive(true);
        }
        else
        {
            Debug.LogError("Finish UI is not assigned.");
        }
    }

    public void OnNextLevelButtonClicked()
    {
        // Load the next level
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (int.TryParse(currentSceneName, out int currentLevelNumber))
        {
            string nextLevelName = (currentLevelNumber + 1).ToString();
            if (Application.CanStreamedLevelBeLoaded(nextLevelName))
            {
                SceneManager.LoadScene(nextLevelName);
            }
            else
            {
                Debug.LogError("Scene '" + nextLevelName + "' couldn't be loaded. Ensure it is added to the build settings.");
            }
        }
        else
        {
            Debug.LogError("Current scene name is not a valid number.");
        }
    }

    public void OnRestartButtonClicked()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
