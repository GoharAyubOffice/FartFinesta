using UnityEngine;
using UnityEngine.UI;

public class BannerSafeAreaManager : MonoBehaviour
{
    [Header("UI Elements to Adjust")]
    [SerializeField] private RectTransform[] uiElementsToAdjust;
    [SerializeField] private Canvas mainCanvas;
    
    [Header("Banner Settings")]
    [SerializeField] private float estimatedBannerHeight = 80f; // Estimated banner height in pixels
    [SerializeField] private bool autoDetectCanvas = true;
    [SerializeField] private bool adjustForSafeArea = true;
    
    private float originalBottomPadding;
    private bool hasAdjusted = false;
    
    void Start()
    {
        if (autoDetectCanvas && mainCanvas == null)
        {
            mainCanvas = FindObjectOfType<Canvas>();
        }
        
        // Wait a frame for everything to initialize
        StartCoroutine(AdjustUIForBanner());
    }
    
    private System.Collections.IEnumerator AdjustUIForBanner()
    {
        // Wait for banner to potentially load
        yield return new WaitForSeconds(2f);
        
        AdjustUIElements();
    }
    
    private void AdjustUIElements()
    {
        if (hasAdjusted) return;
        
        float bannerHeight = CalculateBannerHeight();
        Debug.Log($"Adjusting UI elements for banner height: {bannerHeight}px");
        
        foreach (RectTransform uiElement in uiElementsToAdjust)
        {
            if (uiElement != null)
            {
                AdjustElementForBanner(uiElement, bannerHeight);
            }
        }
        
        hasAdjusted = true;
    }
    
    private float CalculateBannerHeight()
    {
        // Calculate banner height based on screen size and DPI
        float screenHeight = Screen.height;
        float dpi = Screen.dpi > 0 ? Screen.dpi : 160f;
        
        // Base banner height as percentage of screen
        float bannerHeight = screenHeight * 0.08f; // 8% of screen height
        
        // Clamp to reasonable values
        bannerHeight = Mathf.Clamp(bannerHeight, 50f, 100f);
        
        // Adjust for high DPI screens
        if (dpi >= 480) // XXXHDPI
        {
            bannerHeight = Mathf.Max(bannerHeight, 80f);
        }
        else if (dpi >= 320) // XXHDPI
        {
            bannerHeight = Mathf.Max(bannerHeight, 70f);
        }
        else if (dpi >= 240) // XHDPI
        {
            bannerHeight = Mathf.Max(bannerHeight, 60f);
        }
        
        // Add padding for safe area
        bannerHeight += 10f;
        
        return bannerHeight;
    }
    
    private void AdjustElementForBanner(RectTransform element, float bannerHeight)
    {
        // Get the current anchored position
        Vector2 anchoredPos = element.anchoredPosition;
        Vector2 sizeDelta = element.sizeDelta;
        Vector2 anchorMin = element.anchorMin;
        Vector2 anchorMax = element.anchorMax;
        
        // Check if element is positioned at bottom of screen
        bool isBottomAnchored = anchorMin.y == 0 || anchorMax.y <= 0.2f;
        
        if (isBottomAnchored)
        {
            // Adjust bottom-anchored elements
            float adjustmentInCanvasSpace = bannerHeight;
            
            // Convert to canvas space if needed
            if (mainCanvas != null)
            {
                adjustmentInCanvasSpace = bannerHeight / mainCanvas.scaleFactor;
            }
            
            // Move element up by banner height
            anchoredPos.y += adjustmentInCanvasSpace;
            element.anchoredPosition = anchoredPos;
            
            Debug.Log($"Adjusted {element.name} by {adjustmentInCanvasSpace} units for banner safe area");
        }
        
        // Special handling for full-screen elements
        if (anchorMin.y == 0 && anchorMax.y == 1)
        {
            // This is a full-screen element, adjust its bottom margin
            element.offsetMin = new Vector2(element.offsetMin.x, bannerHeight);
            Debug.Log($"Adjusted full-screen element {element.name} with bottom offset: {bannerHeight}");
        }
    }
    
    // Public method to manually adjust UI
    public void RefreshUILayout()
    {
        hasAdjusted = false;
        AdjustUIElements();
    }
    
    // Method to add new UI elements that need adjustment
    public void AddUIElementToAdjust(RectTransform newElement)
    {
        if (newElement == null) return;
        
        // Resize array and add new element
        RectTransform[] newArray = new RectTransform[uiElementsToAdjust.Length + 1];
        for (int i = 0; i < uiElementsToAdjust.Length; i++)
        {
            newArray[i] = uiElementsToAdjust[i];
        }
        newArray[uiElementsToAdjust.Length] = newElement;
        uiElementsToAdjust = newArray;
        
        // Adjust the new element immediately
        if (hasAdjusted)
        {
            float bannerHeight = CalculateBannerHeight();
            AdjustElementForBanner(newElement, bannerHeight);
        }
    }
    
    void OnValidate()
    {
        // Auto-detect canvas in editor
        if (autoDetectCanvas && mainCanvas == null)
        {
            mainCanvas = FindObjectOfType<Canvas>();
        }
    }
}