using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject startScreen; // Reference to the "Touch to Start" UI
    public GameObject nextLevelScreen; // Reference to the next level UI
    public GameObject player;      // Reference to the player GameObject

    public GameObject[] silverStars; // Array of silver star GameObjects
    public GameObject[] goldenStars; // Array of golden star GameObjects
    public TextMeshProUGUI timerText; // Reference to the timer UI text

    private JoystickPlayerExample joystickPlayerExample;
    private FartPropulsion fartPropulsion;
    public Button fartButton;

    private bool isGameFinished = false;
    private float startTime;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player GameObject is not assigned.");
            return;
        }

        joystickPlayerExample = player.GetComponent<JoystickPlayerExample>();
        fartPropulsion = player.GetComponent<FartPropulsion>();

        if (joystickPlayerExample == null)
        {
            Debug.LogError("JoystickPlayerExample component is missing on the player GameObject.");
        }

        if (fartPropulsion == null)
        {
            Debug.LogError("FartPropulsion component is missing on the player GameObject.");
        }

        if (startScreen == null || nextLevelScreen == null || timerText == null)
        {
            Debug.LogError("One or more UI elements are not assigned.");
        }

        startScreen.SetActive(true);
        joystickPlayerExample.enabled = false;
        fartPropulsion.enabled = false;

        Time.timeScale = 0;
        startTime = Time.time; // Initialize timer
    }

    void Update()
    {
        if (isGameFinished)
            return;

        if (timerText == null)
        {
            Debug.LogError("Timer Text is not assigned.");
            return;
        }

        float elapsedTime = Time.time - startTime;
        timerText.text = "Time: " + Mathf.Round(elapsedTime).ToString();

        if (Input.touchCount > 0)
        {
            StartGame();
        }
    }

    void StartGame()
    {
        startScreen.SetActive(false);
        if (joystickPlayerExample != null)
        {
            joystickPlayerExample.enabled = true;
        }
        if (fartPropulsion != null)
        {
            fartPropulsion.enabled = true;
        }
        Time.timeScale = 1;
    }

    public void FinishGame()
    {
        if (isGameFinished) return; // Ensure the game finish logic runs only once

        isGameFinished = true;
        if (joystickPlayerExample != null)
        {
            joystickPlayerExample.enabled = false;
        }
        if (fartPropulsion != null)
        {
            fartPropulsion.enabled = false;
        }
        if (fartButton != null)
        {
            fartButton.enabled = false;
        }

        // Calculate elapsed time
        float elapsedTime = Time.time - startTime;
        Debug.Log("Game finished. Elapsed Time: " + elapsedTime);

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
    }

    void ShowStars(int count)
    {
        Debug.Log("Showing stars with count: " + count);

        if (silverStars.Length == 0 || goldenStars.Length == 0)
        {
            Debug.LogError("Star arrays are not properly assigned.");
            return;
        }

        // Show silver stars
        foreach (var star in silverStars)
        {
            if (star != null)
            {
                star.SetActive(true);
            }
        }

        // Show golden stars based on count
        for (int i = 0; i < goldenStars.Length; i++)
        {
            if (goldenStars[i] != null)
            {
                goldenStars[i].SetActive(i < count);
            }
        }
    }

    public void ShowNextLevelScreen()
    {
        Invoke(nameof(ShowNextLevelScreenDelayed), 2f);
    }

    void ShowNextLevelScreenDelayed()
    {
        if (nextLevelScreen != null)
        {
            nextLevelScreen.SetActive(true);
        }
        if (joystickPlayerExample != null)
        {
            joystickPlayerExample.enabled = false;
        }
        if (fartPropulsion != null)
        {
            fartPropulsion.enabled = false;
        }
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
