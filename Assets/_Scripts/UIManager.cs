using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Needed for SceneManager
using UnityEngine.UI; // Needed for UI elements like Button and Slider

public class UIManager : MonoBehaviour
{
    // References to various UI elements
    public TextMeshProUGUI pointsText;       // Reference to the points text UI element
    public TextMeshProUGUI fartPowerText;    // Reference to the fart power text UI element
    public TextMeshProUGUI levelText;        // Reference to the level text UI element
    public TextMeshProUGUI levelTextUI;      // Reference to the level text UI element
    public FartPropulsion fartPropulsion;    // Reference to the FartPropulsion script

    // Pause menu UI elements
    public GameObject pauseMenuUI;           // Reference to the pause menu UI
    public Button pauseButton;               // Reference to the pause button
    public Button resumeButton;              // Reference to the resume button
    public Button exitButton;                // Reference to the exit button
    public Slider soundSlider;               // Reference to the sound slider

    public AudioSource backgroundAudio;      // Reference to the background audio source
    private AudioSource[] playerAudioSources; // Array to store player-specific audio sources

    public static bool isPaused = false;     // Static to track if the game is paused

    private Animator[] animators;            // Array to store all animators in the scene
    private AudioSource[] audioSources;      // Array to store all AudioSource components in the scene

    private void Start()
    {
        // Initialize UI with default values and current scene name
        UpdateUI();

        // Set the slider value to the current audio volume
        soundSlider.value = backgroundAudio.volume;

        // Add listener to handle volume changes
        soundSlider.onValueChanged.AddListener(SetVolume);

        // Add button listeners
        pauseButton.onClick.AddListener(Pause);
        resumeButton.onClick.AddListener(Resume);
        exitButton.onClick.AddListener(ExitGame);

        // Ensure pause menu UI is hidden initially
        pauseMenuUI.SetActive(false);

        // Find all animators and audio sources in the scene
        animators = FindObjectsOfType<Animator>();
        audioSources = FindObjectsOfType<AudioSource>();

        // Find and assign the player-specific audio sources (assuming they are children or specific GameObjects)
        playerAudioSources = FindObjectsOfType<AudioSource>(); // Or use specific logic to find only player-related sources
    }

    private void Update()
    {
        // Continuously update UI with current values
        UpdateUI();
    }

    // Method to update UI text
    private void UpdateUI()
    {
        fartPowerText.text = "" + fartPropulsion.fartPower;

        // Get the current scene name
        string levelName = SceneManager.GetActiveScene().name;
        levelText.text = "Level: " + levelName;
        levelTextUI.text = "Level: " + levelName;
    }

    public void Pause()
    {
        if (!isPaused)
        {
            pauseMenuUI.SetActive(true);     // Show the pause menu
            Time.timeScale = 0f;             // Pause the game

            // Pause all animators
            foreach (Animator anim in animators)
            {
                anim.speed = 0f;
            }

            // Pause all audio sources
            foreach (AudioSource audio in audioSources)
            {
                audio.Pause(); // Pause the audio source
            }

            isPaused = true;
        }
    }

    public void Resume()
    {
        if (isPaused)
        {
            pauseMenuUI.SetActive(false);    // Hide the pause menu
            Time.timeScale = 1f;             // Resume the game

            // Resume all animators
            foreach (Animator anim in animators)
            {
                anim.speed = 1f;
            }

            // Resume all audio sources
            foreach (AudioSource audio in audioSources)
            {
                audio.UnPause(); // Unpause the audio source
            }

            isPaused = false;
        }
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;  // Ensure time scale is reset before exiting
        Application.Quit();   // Exit the application
    }

    public void SetVolume(float volume)
    {
        // Set volume for background audio
        backgroundAudio.volume = volume;

        // Set volume for player-specific audio sources
        foreach (AudioSource playerAudio in playerAudioSources)
        {
            playerAudio.volume = volume;
        }
    }
}
