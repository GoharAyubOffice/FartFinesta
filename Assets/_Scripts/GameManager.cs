using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    private JoystickPlayerExample joystickPlayerExample;
    private FartPropulsion fartPropulsion;
    public Button fartButton;

    private bool isGameFinished = false;
    private float startTime;

    void Start()
    {
        joystickPlayerExample = player.GetComponent<JoystickPlayerExample>();
        fartPropulsion = player.GetComponent<FartPropulsion>();

        startScreen.SetActive(true);
        joystickPlayerExample.enabled = false;
        fartPropulsion.enabled = false;

        Time.timeScale = 0;
        startTime = Time.time; // Initialize timer
    }

    void Update()
    {
        if (!isGameFinished)
        {
            float elapsedTime = Time.time - startTime;
            timerText.text = "Time: " + Mathf.Round(elapsedTime).ToString();

            if (Input.touchCount > 0)
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
        Time.timeScale = 0;
    }

    public void ShowGameOverScreen()
    {
        nextLevelScreen.SetActive(true);
        restartButton.gameObject.SetActive(true);
        nextLevelButton.gameObject.SetActive(false); // Hide the next level button on game over
        gameOverMessageText.text = "Try Again"; // Set the game over message
        joystickPlayerExample.enabled = false;
        fartPropulsion.enabled = false;
        Time.timeScale = 0;
    }

    public void OnNextLevelButtonClicked()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string nextSceneName = currentScene.name == "1" ? "2" : "1";
        SceneManager.LoadScene(nextSceneName);
    }

    public void OnRestartButtonClicked()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
