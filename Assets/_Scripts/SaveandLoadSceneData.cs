using UnityEngine;

public class SaveandLoadSceneData : MonoBehaviour
{
    void Awake()
        {
            // To load the saved progress (typically in your starting scene):
            SaveManager.Instance.LoadProgress();
        }
}
