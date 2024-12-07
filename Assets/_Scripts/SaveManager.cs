using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    private const string LastSceneKey = "LastPlayedScene";

    // Save the current scene's name
    public static void SaveSceneData()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString(LastSceneKey, currentSceneName);
        PlayerPrefs.Save();
        Debug.Log("Scene saved: " + currentSceneName);
    }

    // Load the saved scene
    public static void LoadSavedScene()
    {
        if (PlayerPrefs.HasKey(LastSceneKey))
        {
            string savedSceneName = PlayerPrefs.GetString(LastSceneKey);

            // Check if the saved scene exists in the build settings
            if (SceneExistsInBuildSettings(savedSceneName))
            {
                SceneManager.LoadScene(savedSceneName);
            }
            else
            {
                Debug.LogWarning("Saved scene not found in build settings: " + savedSceneName);
            }
        }
        else
        {
            Debug.Log("No saved scene found. Starting from the first scene.");
        }
    }

    // Check if a scene exists in the build settings
    private static bool SceneExistsInBuildSettings(string sceneName)
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < sceneCount; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);

            if (name == sceneName)
                return true;
        }
        return false;
    }
}
