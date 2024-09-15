using UnityEngine;

public class PlayerCheckpoint : MonoBehaviour
{
    [SerializeField] private Vector3 lastCheckpointPosition; // Stores the last checkpoint position

    void Start()
    {
        // Set a default position, such as the player's start position
        lastCheckpointPosition = transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player collided with a checkpoint
        if (other.CompareTag("Checkpoint"))
        {
            // Update the last checkpoint position
            lastCheckpointPosition = other.transform.position;
            Debug.Log("Checkpoint updated: " + lastCheckpointPosition);
        }
    }

    public void RespawnAtLastCheckpoint()
    {
        // Move player to the last checkpoint position
        transform.position = lastCheckpointPosition;
    }
}
