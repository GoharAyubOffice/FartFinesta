using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public CameraFollow cameraScript; // Reference to the camera script

    void Start()
    {
        if (cameraScript == null)
        {
            Debug.LogError("CameraFollowWithShake script is not assigned!");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Traps"))
        {
            // Trigger camera shake on collision with an obstacle
            cameraScript.TriggerShake(0.3f, 0.2f); // Example values: 0.5 seconds duration, 0.5 intensity
        }
    }
}
