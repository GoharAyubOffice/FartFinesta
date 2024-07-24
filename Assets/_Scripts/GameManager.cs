using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject startScreen; // Reference to the "Touch to Start" UI
    public GameObject player;      // Reference to the player GameObject
    public Button fartButton;

    private JoystickPlayerExample joystickPlayerExample;
    private FartPropulsion fartPropulsion;
    [SerializeField] private bool isGameFinished = false; // Track if the game has finished

    void Start()
    {
        // Get references to the player controller scripts
        joystickPlayerExample = player.GetComponent<JoystickPlayerExample>();
        fartPropulsion = player.GetComponent<FartPropulsion>();
        fartButton = fartButton.GetComponent<Button>();

        // Show the start screen and disable player controllers
        startScreen.SetActive(true);
        joystickPlayerExample.enabled = false;
        fartPropulsion.enabled = false;

        // Pause the game initially
        Time.timeScale = 0;
    }

    void Update()
    {
        // Check for touch input to start the game, only if the game is not finished
        if (Input.touchCount > 0 && !isGameFinished)
        {
            StartGame();
        }
    }

    void StartGame()
    {
        // Hide the start screen and enable player controllers
        startScreen.SetActive(false);
        joystickPlayerExample.enabled = true;
        fartPropulsion.enabled = true;

        // Unpause the game
        Time.timeScale = 1;
    }

    public void FinishGame()
    {
        // Stop player movement and jumping
        joystickPlayerExample.enabled = false;
        fartPropulsion.enabled = false;

        // Set the game finished flag
        isGameFinished = true;
        fartButton.enabled = false;

        // Optionally, you can add code here to display a finish screen or other end-game logic
    }
}
