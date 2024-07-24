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

    private int playerPoints;

    void Start()
    {
        playerAnimator = GetComponent<Animator>(); // Get the Animator component
        player = GetComponent<Transform>();
        audioSource = GetComponent<AudioSource>();
        playerPoints = GetComponent<FartPropulsion>().playerPoints;

        // Ensure the finish UI is initially inactive
        finishUI.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("finish"))
        {
            // Stop player movement (assuming you have a script controlling movement)
            GetComponent<JoystickPlayerExample>().enabled = false; // Disable movement script
            GetComponent<FartPropulsion>().enabled = false;

            // Set the win animation
            playerAnimator.SetTrigger("Win");
            playerAnimator.SetBool("isWalking", false);

            // Play the clapping sound
            audioSource.PlayOneShot(clappingSound);

            // Start coroutine to show finish UI after a delay
            StartCoroutine(ShowFinishUIAfterDelay(2f)); // Adjust the delay as needed

            Debug.Log("Game Over - Player reached the finish point!");
        }
    }

    IEnumerator ShowFinishUIAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Activate the finish UI
        finishUI.SetActive(true);

        // Update the points and congratulations text
        congratsText.text = "Congratulations!";
    }

    // This function will be called when the next level button is clicked
    public void OnNextLevelButtonClicked()
    {
        // Load the next level
        string currentSceneName = SceneManager.GetActiveScene().name;
        int currentLevelNumber;
        if (int.TryParse(currentSceneName, out currentLevelNumber))
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

    // This function will be called when the restart button is clicked
    public void OnRestartButtonClicked()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
