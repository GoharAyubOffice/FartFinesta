using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Needed for SceneManager

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI pointsText;       // Reference to the points text UI element
    public TextMeshProUGUI fartPowerText;    // Reference to the fart power text UI element
    public TextMeshProUGUI levelText;        // Reference to the level text UI element
    public FartPropulsion fartPropulsion;    // Reference to the FartPropulsion script

    private void Start()
    {
        // Initialize UI with default values and current scene name
        UpdateUI();
    }

    private void Update()
    {
        // Continuously update UI with current values
        UpdateUI();
    }

    // Method to update UI text
    private void UpdateUI()
    {
        pointsText.text = "Points: " + fartPropulsion.playerPoints;
        fartPowerText.text = "Fart: " + fartPropulsion.fartPower;

        // Get the current scene name
        string levelName = SceneManager.GetActiveScene().name;
        levelText.text = "Level: " + levelName;
    }
}
