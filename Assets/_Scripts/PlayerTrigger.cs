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
    public ParticleSystem featherParticles; // Reference to the particle system prefab for death effect
    [SerializeField] private float _finishUIDelay = 2f;

    [SerializeField] private AudioSource[] audioSources;      // Array to store all AudioSource components in the scene

    public float distanceToEnableCollider = 2f; // Adjust this value based on your needs



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

private void OnCollisionEnter(Collision other) 
{
     if (other.gameObject.CompareTag("finish"))
        {
            HandleFinish();
        }
        else if(other.gameObject.CompareTag("Traps"))
        {
            HandleDeath();

        // Instantiate death particles
        if (featherParticles != null)
        {
            featherParticles.Play();
        }
        }
}
    void HandleFinish()
    {
        StopPlayerMovement();

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
            gameManager.isGameFinished = false;
            gameManager.joystickPlayerExample.enabled = true;
            gameManager.fartPropulsion.enabled = true;
            gameManager.fartButton.enabled = true;

        // Reset death count for this level when completed successfully
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        SaveManager.Instance.ResetDeathCount(currentLevelIndex);
        Debug.Log($"Level {currentLevelIndex} completed! Death count reset.");

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
    }

    void HandleDeath()
    {
        StopPlayerMovement();

        playerAnimator.SetBool("Die",true);

        Debug.Log("Player Died");
        
        // Increment death count for current level
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        SaveManager.Instance.IncrementDeathCount(currentLevelIndex);
        
        // Check if we should show ads automatically (2+ deaths)
        int deathCount = SaveManager.Instance.GetDeathCount(currentLevelIndex);
        if (deathCount >= 2 && AdsManager.instance != null)
        {
            Debug.Log($"Player died {deathCount} times. Triggering automatic ad...");
            AdsManager.instance.TriggerAdAfterDeath(deathCount);
        }
        
        // Optionally, if you have a GameOver UI or similar, show it here
        if (gameManager != null)
        {
            StartCoroutine(ShowGameOverScreenAfterDelay(_finishUIDelay)); // Adjust the delay as needed
        }
        else
        {
            Debug.LogError("GameManager is not assigned.");
        }
    }

    void StopPlayerMovement()
    {
        // Disable player movement scripts
        var joystickPlayer = GetComponent<JoystickPlayerExample>();
        if (joystickPlayer != null)
        {
            joystickPlayer.enabled = false;
        }

        var fartPropulsion = GetComponent<FartPropulsion>();
        if (fartPropulsion != null)
        {
            fartPropulsion.enabled = false;
        }

        // Disable collider immediately after hitting trap
        GetComponent<Collider>().enabled = false;

        // Start checking distance to ground
        StartCoroutine(CheckDistanceToGround());
    }

    private IEnumerator CheckDistanceToGround()
    {
        
        Collider playerCollider = GetComponent<Collider>();

        while (!playerCollider.enabled)
        {
            // Cast a ray downward to check distance to ground
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit))
            {
                if (hit.collider.CompareTag("Ground") && hit.distance <= distanceToEnableCollider)
                {
                    playerCollider.enabled = true;
                    break;
                }
            }
            yield return new WaitForFixedUpdate();
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

    IEnumerator ShowGameOverScreenAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Assuming you have a game over screen or you want to restart the level
        if (gameManager != null)
        {
            gameManager.ShowGameOverScreen();
        }
        foreach (AudioSource audio in audioSources)
            {
                audio.Pause(); // Pause the audio source
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
