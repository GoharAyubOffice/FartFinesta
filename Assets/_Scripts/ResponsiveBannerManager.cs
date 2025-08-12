using UnityEngine;
using UnityEngine.Advertisements;

public class ResponsiveBannerManager : MonoBehaviour
{
    [Header("Banner Configuration")]
    [SerializeField] private bool enableAdaptiveBanner = true;
    [SerializeField] private float bannerHeightPercentage = 0.08f; // 8% of screen height
    [SerializeField] private float minBannerHeight = 50f; // Minimum banner height in pixels
    [SerializeField] private float maxBannerHeight = 90f; // Maximum banner height in pixels
    
    private static ResponsiveBannerManager instance;
    public static ResponsiveBannerManager Instance => instance;
    
    void Awake()
    {
        if (instance == null)
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
        if (enableAdaptiveBanner)
        {
            SetupResponsiveBanner();
        }
    }
    
    private void SetupResponsiveBanner()
    {
        Debug.Log("Setting up responsive banner ad system");
        LogDeviceInfo();
        
        // Wait for AdsManager to initialize
        if (AdsManager.instance != null)
        {
            StartCoroutine(WaitForAdsInitialization());
        }
    }
    
    private System.Collections.IEnumerator WaitForAdsInitialization()
    {
        // Wait until Unity Ads is initialized
        while (!Advertisement.isInitialized)
        {
            yield return new WaitForSeconds(0.5f);
        }
        
        yield return new WaitForSeconds(1f); // Additional delay to ensure everything is ready
        
        SetupAdaptiveBanner();
    }
    
    private void SetupAdaptiveBanner()
    {
        if (AdsManager.instance == null)
        {
            Debug.LogError("AdsManager not found! Cannot setup adaptive banner.");
            return;
        }
        
        // Calculate ideal banner height based on screen size
        float idealHeight = CalculateIdealBannerHeight();
        Debug.Log($"Calculated ideal banner height: {idealHeight}px");
        
        // Set banner position with proper spacing
        SetBannerPosition();
        
        // Load the banner
        AdsManager.instance.LoadBannerAd();
    }
    
    private float CalculateIdealBannerHeight()
    {
        float screenHeight = Screen.height;
        float targetHeight = screenHeight * bannerHeightPercentage;
        
        // Clamp between min and max values
        targetHeight = Mathf.Clamp(targetHeight, minBannerHeight, maxBannerHeight);
        
        // Adjust for different DPI ranges
        float dpi = Screen.dpi > 0 ? Screen.dpi : 160f; // Default to 160 if DPI is not available
        
        if (dpi >= 480) // XXXHDPI
        {
            targetHeight = Mathf.Max(targetHeight, 70f);
        }
        else if (dpi >= 320) // XXHDPI
        {
            targetHeight = Mathf.Max(targetHeight, 60f);
        }
        else if (dpi >= 240) // XHDPI
        {
            targetHeight = Mathf.Max(targetHeight, 55f);
        }
        else if (dpi >= 160) // HDPI
        {
            targetHeight = Mathf.Max(targetHeight, 50f);
        }
        
        return targetHeight;
    }
    
    private void SetBannerPosition()
    {
        // Unity Ads banner positioning - using BOTTOM_CENTER for best compatibility
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Debug.Log("Banner position set to BOTTOM_CENTER");
    }
    
    private void LogDeviceInfo()
    {
        Debug.Log("=== Device Information for Banner Optimization ===");
        Debug.Log($"Screen Resolution: {Screen.width}x{Screen.height}");
        Debug.Log($"Screen DPI: {Screen.dpi}");
        Debug.Log($"Screen Orientation: {Screen.orientation}");
        Debug.Log($"Platform: {Application.platform}");
        Debug.Log($"Device Model: {SystemInfo.deviceModel}");
        
        // Calculate screen inches for reference
        if (Screen.dpi > 0)
        {
            float screenInches = Mathf.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height) / Screen.dpi;
            Debug.Log($"Estimated Screen Size: {screenInches:F1} inches");
        }
        
        Debug.Log("=== Banner Configuration ===");
        Debug.Log($"Target Banner Height %: {bannerHeightPercentage * 100}%");
        Debug.Log($"Min Banner Height: {minBannerHeight}px");
        Debug.Log($"Max Banner Height: {maxBannerHeight}px");
    }
    
    // Method to adjust banner for orientation changes
    void Update()
    {
        // Handle orientation changes
        if (Input.deviceOrientation != DeviceOrientation.Unknown && 
            Input.deviceOrientation != DeviceOrientation.FaceUp && 
            Input.deviceOrientation != DeviceOrientation.FaceDown)
        {
            // Orientation changed, might need to readjust banner
            // Unity Ads handles most of this automatically, but we log it for debugging
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                // Mobile platforms - banner should auto-adjust
            }
        }
    }
    
    // Public method to force banner refresh (useful for testing)
    public void RefreshBanner()
    {
        if (AdsManager.instance != null)
        {
            Debug.Log("Refreshing banner ad...");
            AdsManager.instance.HideBannerAd();
            SetupAdaptiveBanner();
        }
    }
    
    // Method to get safe area for UI layout (accounts for banner space)
    public float GetBannerHeightForUILayout()
    {
        float bannerHeight = CalculateIdealBannerHeight();
        
        // Add some padding for safe area
        float padding = 10f;
        
        return bannerHeight + padding;
    }
}