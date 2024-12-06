using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private CameraFollow cameraScript; // Reference to the camera script

    private void Awake()
    {
        cameraScript = Camera.main.GetComponent<CameraFollow>();

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Traps"))
        {            
             cameraScript.TriggerShake(0.3f, 0.2f); // Example values: 0.3 seconds duration, 0.2 intensity
        }
    }
}
