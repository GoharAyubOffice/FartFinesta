using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerTrigger : MonoBehaviour
{
    public Animator playerAnimator; // Reference to the player's Animator component
    [SerializeField] private Transform player;
    [SerializeField] private AudioSource audioSource;
    public GameObject finishUI; // Reference to the finish UI Canvas
    public TextMeshProUGUI congratsText; // Reference to the congratulations TextMeshPro

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
            audioSource.Play();

            // Show the finish UI and update the text
            ShowFinishUI();

            Debug.Log("Game Over - Player reached the finish point!");
        }
    }

    void ShowFinishUI()
    {
        // Activate the finish UI
        finishUI.SetActive(true);

        // Update the points and congratulations text
        congratsText.text = "Congratulations!";
    }

    // This function will be called when the next level button is clicked
    public void OnNextLevelButtonClicked()
    {
        // Load the next level
        // Assuming your levels are named "Level1", "Level2", etc.
        string currentSceneName = SceneManager.GetActiveScene().name;
        int currentLevelNumber = int.Parse(currentSceneName.Replace("Level", ""));
        string nextLevelName = "Level" + (currentLevelNumber + 1);

        SceneManager.LoadScene(nextLevelName);
    }
}
