using UnityEngine;
using UnityEngine.UI;

public class UIButtonManager : MonoBehaviour
{
    void Start()
    {
        HideManualVideoAdButton();
    }
    
    private void HideManualVideoAdButton()
    {
        // Find the ViewAd button and hide it since we now use automatic ads
        GameObject viewAdButton = GameObject.Find("ViewAd");
        
        if (viewAdButton != null)
        {
            Debug.Log("Found ViewAd button, hiding it since we now use automatic ads");
            viewAdButton.SetActive(false);
        }
        else
        {
            // Alternative search method - look for button with "Video Ad" text
            Button[] allButtons = FindObjectsOfType<Button>();
            foreach (Button button in allButtons)
            {
                if (button.GetComponentInChildren<TMPro.TextMeshProUGUI>() != null)
                {
                    TMPro.TextMeshProUGUI text = button.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                    if (text.text.Contains("Video Ad"))
                    {
                        Debug.Log("Found Video Ad button by text search, hiding it");
                        button.gameObject.SetActive(false);
                        break;
                    }
                }
            }
        }
    }
}