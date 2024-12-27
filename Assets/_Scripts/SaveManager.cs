// SaveManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    private static SaveManager instance;
    private const string SCENE_KEY = "LastPlayedScene";
    private const string LEVEL_KEY = "CurrentLevel";
    private bool isFirstLoad = true;

    public static SaveManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("MobileSaveManager");
                instance = go.AddComponent<SaveManager>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        // Only load saved progress when game first starts
        if (isFirstLoad)
        {
            LoadProgress();
            isFirstLoad = false;
        }
    }

    // Convert build index to actual scene name (add 1 to match your scene naming)
    private string GetSceneNameFromBuildIndex(int buildIndex)
    {
        return (buildIndex + 1).ToString();
    }

    // Convert scene name to build index (subtract 1 to match Unity's indexing)
    private int GetBuildIndexFromSceneName(string sceneName)
    {
        if (int.TryParse(sceneName, out int sceneNumber))
        {
            return sceneNumber - 1;
        }
        return 0; // Default to first scene if parsing fails
    }

    public void SaveProgress(int buildIndex)
    {
        string actualSceneName = GetSceneNameFromBuildIndex(buildIndex);
        PlayerPrefs.SetString(SCENE_KEY, actualSceneName);
        PlayerPrefs.SetInt(LEVEL_KEY, buildIndex);
        PlayerPrefs.Save();
        Debug.Log($"Progress saved: Scene {actualSceneName}, Build Index {buildIndex}");
    }

    public void LoadProgress()
    {
        if (PlayerPrefs.HasKey(SCENE_KEY))
        {
            string savedScene = PlayerPrefs.GetString(SCENE_KEY);
            int savedBuildIndex = GetBuildIndexFromSceneName(savedScene);

            // Only load if we're in level 1 (this means game just started)
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                SceneManager.LoadScene(savedBuildIndex);
                GameEvents.OnLevelLoaded?.Invoke(savedBuildIndex);
                Debug.Log($"Loading saved progress: Scene {savedScene}, Build Index {savedBuildIndex}");
            }
        }
        // If no save data exists, just stay in level 1
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey(SCENE_KEY);
        PlayerPrefs.DeleteKey(LEVEL_KEY);
        PlayerPrefs.Save();
        Debug.Log("Progress reset complete");
    }
}

public static class GameEvents
{
    public static System.Action<int> OnLevelLoaded;
}