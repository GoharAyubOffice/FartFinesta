using UnityEngine;
using UnityEngine.SceneManagement;

public class AdsDebugUI : MonoBehaviour
{
    private bool showDebugUI = false;
    private Rect windowRect = new Rect(20, 20, 300, 400);
    
    void Update()
    {
        // Toggle debug UI with F1 key (for testing purposes)
        if (Input.GetKeyDown(KeyCode.F1))
        {
            showDebugUI = !showDebugUI;
        }
    }
    
    void OnGUI()
    {
        if (!showDebugUI) return;
        
        windowRect = GUI.Window(0, windowRect, DebugWindow, "Ads Debug Panel");
    }
    
    void DebugWindow(int windowID)
    {
        GUILayout.BeginVertical();
        
        GUILayout.Label("=== Death Counter Info ===");
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        int deathCount = SaveManager.Instance.GetDeathCount(currentLevel);
        int totalDeaths = SaveManager.Instance.GetTotalDeathCount();
        
        GUILayout.Label($"Current Level: {currentLevel}");
        GUILayout.Label($"Deaths This Level: {deathCount}");
        GUILayout.Label($"Total Deaths: {totalDeaths}");
        
        if (GUILayout.Button("Reset Death Count (This Level)"))
        {
            SaveManager.Instance.ResetDeathCount(currentLevel);
        }
        
        if (GUILayout.Button("Reset All Death Counts"))
        {
            SaveManager.Instance.ResetAllDeathCounts();
        }
        
        GUILayout.Label("=== Ads Status ===");
        if (AdsManager.instance != null)
        {
            GUILayout.Label("AdsManager: Active");
            
            if (GUILayout.Button("Force Show Interstitial"))
            {
                AdsManager.instance.ShowInterstitialAd();
            }
            
            if (GUILayout.Button("Force Show Rewarded Video"))
            {
                AdsManager.instance.ShowRewardedVideoAd();
            }
            
            if (GUILayout.Button("Hide Banner Ad"))
            {
                AdsManager.instance.HideBannerAd();
            }
            
            if (GUILayout.Button("Show Banner Ad"))
            {
                AdsManager.instance.ShowBannerAd();
            }
            
            if (GUILayout.Button("Reload Banner Ad"))
            {
                AdsManager.instance.HideBannerAd();
                AdsManager.instance.LoadBannerAd();
            }
            
            if (GUILayout.Button("Simulate Death (Auto Ad)"))
            {
                AdsManager.instance.TriggerAdAfterDeath(deathCount + 1);
            }
        }
        else
        {
            GUILayout.Label("AdsManager: Not Found!");
        }
        
        GUILayout.Label("=== Banner Info ===");
        GUILayout.Label($"Screen: {Screen.width}x{Screen.height}");
        GUILayout.Label($"DPI: {Screen.dpi}");
        GUILayout.Label($"Safe Area: {Screen.safeArea}");
        
        if (ResponsiveBannerManager.Instance != null)
        {
            float bannerHeight = ResponsiveBannerManager.Instance.GetBannerHeightForUILayout();
            GUILayout.Label($"Banner Height: {bannerHeight:F0}px");
            
            if (GUILayout.Button("Refresh Banner Layout"))
            {
                ResponsiveBannerManager.Instance.RefreshBanner();
            }
        }
        
        GUILayout.Label("=== Unity Ads Info ===");
        GUILayout.Label($"Ads Initialized: {UnityEngine.Advertisements.Advertisement.isInitialized}");
        GUILayout.Label($"Platform: {Application.platform}");
        
        if (GUILayout.Button("Close Debug Panel"))
        {
            showDebugUI = false;
        }
        
        GUILayout.EndVertical();
        
        GUI.DragWindow();
    }
}