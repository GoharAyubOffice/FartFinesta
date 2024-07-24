using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    public Animator playerAnimator; // Reference to the player's Animator component
    [SerializeField] private Transform player;
    [SerializeField] private AudioSource audioSource;
    public AudioClip clappingSound; // Reference to the clapping sound clip

    private GameManager gameManager; // Reference to the GameManager script

    void Start()
    {
        playerAnimator = GetComponent<Animator>(); // Get the Animator component
        player = GetComponent<Transform>();
        audioSource = GetComponent<AudioSource>();

        // Get reference to the GameManager script
        gameManager = FindObjectOfType<GameManager>();
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
            if (clappingSound != null)
            {
                audioSource.clip = clappingSound;
                audioSource.Play();
            }

            Debug.Log("Game Over - Player reached the finish point!");

            // Call FinishGame method from GameManager
            if (gameManager != null)
            {
                gameManager.FinishGame();
            }
        }
    }
}
