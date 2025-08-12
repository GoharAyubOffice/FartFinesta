using UnityEngine;
using UnityEngine.UI;

public class VideoAdButton : MonoBehaviour
{
    private Button videoAdButton;
    
    void Start()
    {
        videoAdButton = GetComponent<Button>();
        
        if (videoAdButton != null)
        {
            videoAdButton.onClick.RemoveAllListeners();
            videoAdButton.onClick.AddListener(OnVideoAdButtonClicked);
        }
        else
        {
            Debug.LogError("VideoAdButton script must be attached to a GameObject with a Button component!");
        }
    }
    
    private void OnVideoAdButtonClicked()
    {
        Debug.Log("Video Ad button clicked!");
        
        if (AdsManager.instance != null)
        {
            AdsManager.instance.ShowRewardedVideoAd();
        }
        else
        {
            Debug.LogError("AdsManager instance not found! Make sure AdsManager is in the scene.");
        }
    }
}