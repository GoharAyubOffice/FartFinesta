using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject startScreen; // Reference to the "Touch to Start" UI
    public GameObject player;      // Reference to the player GameObject

    private JoystickPlayerExample joystickPlayerExample;
    private FartPropulsion fartPropulsion;

    void Start()
    {
        // Get references to the player controller scripts
        joystickPlayerExample = player.GetComponent<JoystickPlayerExample>();
        fartPropulsion = player.GetComponent<FartPropulsion>();

        // Show the start screen and disable player controllers
        startScreen.SetActive(true);
        joystickPlayerExample.enabled = false;
        fartPropulsion.enabled = false;

        // Pause the game initially
        Time.timeScale = 0;
    }

    void Update()
    {
        // Check for touch input to start the game
        if (Input.touchCount > 0)
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
}
