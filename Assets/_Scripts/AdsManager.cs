using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static AdsManager instance;
    [SerializeField] private string _androidGameId = "your_android_game_id";
    [SerializeField] private string _iOSGameId = "your_ios_game_id";
    [SerializeField] private bool _testMode = true;
    private string _gameId;

private void Awake() 
{
    if(instance == null)
    {
        instance = this;
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
        LoadAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    public void LoadAd()
    {
        string adUnitId = "Interstitial_Android"; // Use your actual Ad Unit ID
        Advertisement.Load(adUnitId, this);
    }

    public void ShowAd()
    {
        string adUnitId = "Interstitial_Android"; // Use your actual Ad Unit ID
        Advertisement.Show(adUnitId, this);
        
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log($"Ad loaded: {placementId}");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Failed to load Ad Unit {placementId}: {error.ToString()} - {message}");
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

        LoadAd();  // Load the next ad after showing the current one
    }
}
