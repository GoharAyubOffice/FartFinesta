using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI fartPowerText;
    public JoystickPlayerExample playerController;

    private void Update()
    {
        pointsText.text = "Points: " + playerController.playerPoints;
        fartPowerText.text = "Fart Power: " + playerController.fartPower;
    }
}
