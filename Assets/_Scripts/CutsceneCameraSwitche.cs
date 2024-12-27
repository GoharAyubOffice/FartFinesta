using UnityEngine;
using UnityEngine.Playables;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;

public class CutsceneCameraSwitcher : MonoBehaviour
{
     public PlayableDirector cutsceneTimeline; // Reference to the PlayableDirector
    public CinemachineVirtualCamera cutsceneCamera; // Reference to the cutscene camera
    public Camera mainGameplayCamera; // Reference to the main gameplay camera
    public Camera CSCamera; // Reference to the main gameplay camera

    [SerializeField] private GameObject gameManager;
    [SerializeField] private GameObject musicManager;
    
    [SerializeField] private GameObject uiManager;
    [SerializeField] private GameObject Canvas;

    private void Start()
    {
       

        // Disable the main gameplay camera at the start and enable the cutscene camera
        mainGameplayCamera.enabled = false;
        cutsceneCamera.enabled = true;

        // Play the cutscene timeline
        cutsceneTimeline.Play();

        // Subscribe to the event when the cutscene finishes
        cutsceneTimeline.stopped += OnCutsceneFinished;
        
    }

    void OnCutsceneFinished(PlayableDirector pd)
    {
        // Enable the main gameplay camera and disable the cutscene camera when the cutscene is done
        cutsceneCamera.enabled = false;
        CSCamera.enabled=false;
        mainGameplayCamera.enabled = true;
        Canvas.SetActive(true);
        gameManager.SetActive(true);
        musicManager.SetActive(true);
        uiManager.SetActive(true);
    }
}