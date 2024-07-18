using Unity.VisualScripting;
using UnityEngine;

public class CollisionEffect : MonoBehaviour
{
    public GameObject starBurstPrefab; // Reference to the StarBurst particle system prefab

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Traps") || (collision.gameObject.CompareTag("Obstacle"))) // Change "Obstacle" to whatever tag your obstacles have
        {
            // Instantiate star burst effect at the collision point
            GameObject starBurst = Instantiate(starBurstPrefab, collision.contacts[0].point, Quaternion.identity);

            // Destroy the star burst after 1 second
            Destroy(starBurst, 1f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Traps")) // Change "Obstacle" to whatever tag your obstacles have
        {
            // Instantiate star burst effect at the collision point
            GameObject starBurst = Instantiate(starBurstPrefab, other.transform.position, Quaternion.identity);

            // Destroy the star burst after 1 second
            Destroy(starBurst, 1f);
        }
    }
}
