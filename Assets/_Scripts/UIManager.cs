using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI fartPowerText;
    public FartPropulsion fartPropulsion;

    private void Update()
    {
        pointsText.text = "Points: " + fartPropulsion.playerPoints;
        fartPowerText.text = "Fart Power: " + fartPropulsion.fartPower;
    }
}
