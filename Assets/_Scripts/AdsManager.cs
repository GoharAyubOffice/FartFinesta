using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static AdsManager instance;
    [SerializeField] private string _androidGameId = "5689273";
    [SerializeField] private string _iOSGameId = "5689272";
    [SerializeField] private bool _testMode = true;
    private string _gameId;

private void Awake() 
{
    if(instance == null)
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    else
    {
        Destroy(gameObject);
    }
}
    void Start()
    {
        InitializeAds();
    }

    public void InitializeAds()
    {
        _gameId = (Application.platform == RuntimePlatform.IPhonePlayer) ? _iOSGameId : _androidGameId;
        Advertisement.Initialize(_gameId, _testMode, this);
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        Debug.Log($"Game ID: {_gameId}, Test Mode: {_testMode}");
        LoadAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    // Ad Unit IDs - these should be replaced with actual IDs from Unity Ads Dashboard
    private string interstitialAdUnitId = "Interstitial_Android";
    private string rewardedVideoAdUnitId = "Rewarded_Android";
    private string bannerAdUnitId = "Banner_Android";
    
    private bool isInterstitialLoaded = false;
    private bool isRewardedVideoLoaded = false;
    private bool isBannerLoaded = false;
    private bool isBannerShowing = false;
    
    // Ad cooldown and auto-triggering
    private float lastAdShowTime = 0f;
    private const float AD_COOLDOWN = 30f; // 30 seconds between auto ads
    
    public void LoadInterstitialAd()
    {
        string adUnitId = GetPlatformAdUnitId(interstitialAdUnitId);
        Debug.Log($"Loading interstitial ad: {adUnitId}");
        Advertisement.Load(adUnitId, this);
    }
    
    public void LoadRewardedVideoAd()
    {
        string adUnitId = GetPlatformAdUnitId(rewardedVideoAdUnitId);
        Debug.Log($"Loading rewarded video ad: {adUnitId}");
        Advertisement.Load(adUnitId, this);
    }
    
    public void LoadBannerAd()
    {
        string adUnitId = GetPlatformAdUnitId(bannerAdUnitId);
        Debug.Log($"Loading responsive banner ad: {adUnitId}");
        Debug.Log($"Screen resolution: {Screen.width}x{Screen.height}, DPI: {Screen.dpi}");
        
        // Set position to bottom center
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        
        // Load banner with responsive sizing
        Advertisement.Banner.Load(adUnitId, new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        });
    }
    
    private void OnBannerLoaded()
    {
        Debug.Log("Banner ad loaded successfully");
        Debug.Log($"Banner size will adapt to screen: {Screen.width}x{Screen.height}");
        isBannerLoaded = true;
        ShowBannerAd();
    }
    
    private void OnBannerError(string message)
    {
        Debug.LogError($"Banner ad failed to load: {message}");
        Debug.LogError("This might be due to:");
        Debug.LogError("1. Incorrect Banner Ad Unit ID");
        Debug.LogError("2. Network connectivity issues");
        Debug.LogError("3. Ad inventory not available");
        Debug.LogError("4. Unity Ads configuration issues");
        isBannerLoaded = false;
        
        // Retry after a delay
        StartCoroutine(RetryBannerLoad());
    }
    
    private System.Collections.IEnumerator RetryBannerLoad()
    {
        yield return new WaitForSeconds(10f); // Wait 10 seconds before retry
        
        if (!isBannerLoaded && Advertisement.isInitialized)
        {
            Debug.Log("Retrying banner ad load...");
            LoadBannerAd();
        }
    }
    
    public void ShowBannerAd()
    {
        if (isBannerLoaded && !isBannerShowing)
        {
            string adUnitId = GetPlatformAdUnitId(bannerAdUnitId);
            Advertisement.Banner.Show(adUnitId);
            isBannerShowing = true;
            Debug.Log($"Banner ad shown at bottom center of screen");
            Debug.Log($"Banner will automatically resize for device: {SystemInfo.deviceModel}");
            Debug.Log($"Screen safe area: {Screen.safeArea}");
        }
        else if (!isBannerLoaded)
        {
            Debug.LogWarning("Cannot show banner ad - not loaded yet");
        }
        else if (isBannerShowing)
        {
            Debug.Log("Banner ad is already showing");
        }
    }
    
    public void HideBannerAd()
    {
        if (isBannerShowing)
        {
            Advertisement.Banner.Hide();
            isBannerShowing = false;
            Debug.Log("Banner ad hidden");
        }
    }
    
    public void LoadAd()
    {
        LoadInterstitialAd();
        LoadRewardedVideoAd();
        LoadBannerAd();
    }
    
    // Auto ad triggering method called from PlayerTrigger
    public void TriggerAdAfterDeath(int deathCount)
    {
        float timeSinceLastAd = Time.time - lastAdShowTime;
        
        if (timeSinceLastAd < AD_COOLDOWN)
        {
            Debug.Log($"Ad cooldown active. {AD_COOLDOWN - timeSinceLastAd:F1} seconds remaining.");
            return;
        }
        
        // Show interstitial ad after 2-3 deaths, rewarded video after 4+ deaths
        if (deathCount >= 4)
        {
            ShowRewardedVideoAd();
        }
        else if (deathCount >= 2)
        {
            ShowInterstitialAd();
        }
        
        lastAdShowTime = Time.time;
    }

    public void ShowInterstitialAd()
    {
        string adUnitId = GetPlatformAdUnitId(interstitialAdUnitId);
        
        if (!Advertisement.isInitialized)
        {
            Debug.LogError("Unity Ads not initialized! Cannot show interstitial ad.");
            return;
        }
        
        if (isInterstitialLoaded)
        {
            Debug.Log($"Showing interstitial ad: {adUnitId}");
            Advertisement.Show(adUnitId, this);
        }
        else
        {
            Debug.LogWarning("Interstitial ad not ready. Loading new ad...");
            LoadInterstitialAd();
        }
    }
    
    public void ShowRewardedVideoAd()
    {
        string adUnitId = GetPlatformAdUnitId(rewardedVideoAdUnitId);
        
        if (!Advertisement.isInitialized)
        {
            Debug.LogError("Unity Ads not initialized! Cannot show rewarded video ad.");
            return;
        }
        
        if (isRewardedVideoLoaded)
        {
            Debug.Log($"Showing rewarded video ad: {adUnitId}");
            Advertisement.Show(adUnitId, this);
        }
        else
        {
            Debug.LogWarning("Rewarded video ad not ready. Loading new ad...");
            LoadRewardedVideoAd();
        }
    }
    
    [System.Obsolete("Use ShowInterstitialAd() or ShowRewardedVideoAd() instead")]
    public void ShowAd()
    {
        ShowInterstitialAd();
    }
    
    private string GetPlatformAdUnitId(string baseAdUnitId)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            return baseAdUnitId.Replace("_Android", "_iOS");
        }
        return baseAdUnitId;
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log($"Ad loaded: {placementId}");
        
        // Track which ad type was loaded
        if (placementId.Contains("Interstitial"))
        {
            isInterstitialLoaded = true;
        }
        else if (placementId.Contains("Rewarded"))
        {
            isRewardedVideoLoaded = true;
        }
        else if (placementId.Contains("Banner"))
        {
            isBannerLoaded = true;
        }
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogError($"Failed to load Ad Unit {placementId}: {error.ToString()} - {message}");
        
        // Reset loaded status
        if (placementId.Contains("Interstitial"))
        {
            isInterstitialLoaded = false;
        }
        else if (placementId.Contains("Rewarded"))
        {
            isRewardedVideoLoaded = false;
        }
        else if (placementId.Contains("Banner"))
        {
            isBannerLoaded = false;
        }
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {placementId}: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log($"Started showing Ad Unit: {placementId}");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log($"Ad Unit clicked: {placementId}");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log($"Ad Unit {placementId} completed - {showCompletionState.ToString()}");
        
        // Handle rewarded video completion
        if (placementId.Contains("Rewarded") && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            Debug.Log("Rewarded video completed! Player should receive reward.");
            OnRewardedVideoCompleted();
        }
        
        // Reset loaded status and load next ad
        if (placementId.Contains("Interstitial"))
        {
            isInterstitialLoaded = false;
            LoadInterstitialAd();
        }
        else if (placementId.Contains("Rewarded"))
        {
            isRewardedVideoLoaded = false;
            LoadRewardedVideoAd();
        }
    }
    
    private void OnRewardedVideoCompleted()
    {
        // This method can be overridden or use events to handle rewards
        Debug.Log("Player watched complete rewarded video - granting reward!");
        
        // Grant extra life or retry functionality
        GameManager.canGrantExtraLife = true;
        
        // Find the GameManager in the scene and trigger retry/continue
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            // Grant the player another chance to continue from where they died
            StartCoroutine(RestartLevelAfterReward(gameManager));
        }
        else
        {
            Debug.LogWarning("GameManager not found! Cannot grant reward.");
        }
    }
    
    private System.Collections.IEnumerator RestartLevelAfterReward(GameManager gameManager)
    {
        yield return new WaitForSeconds(0.5f); // Small delay for user feedback
        
        // Restart the current level as a reward
        gameManager.OnRestartButtonClicked();
        
        Debug.Log("Level restarted as reward for watching video ad!");
    }
}
