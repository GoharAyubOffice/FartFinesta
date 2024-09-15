using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject startScreen; // Reference to the "Touch to Start" UI
    public GameObject nextLevelScreen; // Reference to the next level UI
    public GameObject player; // Reference to the player GameObject

    public GameObject[] silverStars; // Array of silver star GameObjects
    public GameObject[] goldenStars; // Array of golden star GameObjects
    public TextMeshProUGUI timerText; // Reference to the timer UI text

    public GameObject restartButton; // Reference to the restart button
    public GameObject nextLevelButton; // Reference to the next level button

    public TextMeshProUGUI gameOverMessageText; // Reference to the game over message text

    public  JoystickPlayerExample joystickPlayerExample;
    public  FartPropulsion fartPropulsion;
    public  Button fartButton;

    public bool isGameFinished = false;
    private float startTime;

    [SerializeField] private int targetFrameRate = 60;
    [SerializeField] private float _UIShowDelay = 2f;

    void Start()
    {
        joystickPlayerExample = player.GetComponent<JoystickPlayerExample>();
        fartPropulsion = player.GetComponent<FartPropulsion>();

        startScreen.SetActive(true);
        joystickPlayerExample.enabled = false;
        fartPropulsion.enabled = false;

        Time.timeScale = 0;
        startTime = Time.time; // Initialize timer

        Application.targetFrameRate = 60;
    }

    void Update()
    {
        if (!isGameFinished && !UIManager.isPaused) // Check if the game is not paused
        {
            float elapsedTime = Time.time - startTime;
            timerText.text = "Time: " + Mathf.Round(elapsedTime).ToString();

            if (Input.touchCount > 0 && Time.timeScale == 0) // Start game only if it's paused
            {
                StartGame();
            }
        }
    }

    void StartGame()
    {
        startScreen.SetActive(false);
        joystickPlayerExample.enabled = true;
        fartPropulsion.enabled = true;
        Time.timeScale = 1;

        Application.targetFrameRate = targetFrameRate;
        QualitySettings.vSyncCount = 0; // 0 means don't sync to VBlank
    }

    public void FinishGame()
    {
        if (isGameFinished) return; // Ensure the game finish logic runs only once

            isGameFinished = true;
            joystickPlayerExample.enabled = false;
            fartPropulsion.enabled = false;
            fartButton.enabled = false;
    

        // Calculate elapsed time
        float elapsedTime = Time.time - startTime;

        // Determine star count based on time
        int starCount = 1; // Default to 1 star
        if (elapsedTime <= 20)
        {
            starCount = 3;
        }
        else if (elapsedTime <= 25)
        {
            starCount = 2;
        }

        // Show stars based on the calculated star count
        ShowStars(starCount);

        // Show the next level screen and buttons
        ShowNextLevelScreen();
    }

    void ShowStars(int count)
    {
        // Show silver stars
        foreach (var star in silverStars)
        {
            star.SetActive(true);
        }

        // Show golden stars based on count
        for (int i = 0; i < goldenStars.Length; i++)
        {
            if (i < count)
            {
                goldenStars[i].SetActive(true);
            }
            else
            {
                goldenStars[i].SetActive(false);
            }
        }
    }

    public void ShowNextLevelScreen()
    {
        nextLevelScreen.SetActive(true);
        restartButton.gameObject.SetActive(true);
        nextLevelButton.gameObject.SetActive(true);
        joystickPlayerExample.enabled = false;
        fartPropulsion.enabled = false;
        UIManager.isPaused = true; // Set the global pause state
        Time.timeScale = 0;
    }

    public void ShowGameOverScreen()
    {
        // Start the coroutine to show the game over screen after a delay
        StartCoroutine(DelayedShowGameOverScreen(_UIShowDelay)); // Adjust the delay time (in seconds) as needed
    }

    private IEnumerator DelayedShowGameOverScreen(float delay)
    {
        // Disable player controls immediately
        joystickPlayerExample.enabled = false;
        fartPropulsion.enabled = false;

        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Show the game over screen UI
        nextLevelScreen.SetActive(true);
        restartButton.gameObject.SetActive(true);
        nextLevelButton.gameObject.SetActive(false); // Hide the next level button on game over
        gameOverMessageText.text = "Try Again"; // Set the game over message

        // Pause the game after showing the UI
        UIManager.isPaused = true; // Set the global pause state
    }

    public void OnNextLevelButtonClicked()
    {
        // Get the current scene
        Scene currentScene = SceneManager.GetActiveScene();
        int currentSceneIndex = currentScene.buildIndex;

        // Determine the next level index
        int nextSceneIndex = currentSceneIndex + 1;

        // Check if the next level index is within the available scenes
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            // Load the next level
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            // If there are no more levels, restart at level 1
            SceneManager.LoadScene(0);
        }

        // Clear the global pause state before loading the next level
        UIManager.isPaused = false;
    }

    public void OnRestartButtonClicked()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        UIManager.isPaused = false; // Clear the global pause state before restarting
        SceneManager.LoadScene(currentScene.name);
    }
}
