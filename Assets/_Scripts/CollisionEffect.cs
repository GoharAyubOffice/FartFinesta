using Unity.VisualScripting;
using UnityEngine;

public class CollisionEffect : MonoBehaviour
{
    public GameObject starBurstPrefab; // Reference to the StarBurst particle system prefab

    public int fartPowerDecrease = 1;

    [SerializeField] private FartPropulsion fart;

    private void Start()
    {
         fart = GetComponent<FartPropulsion>();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Traps")) // Change "Traps" to whatever tag your obstacles have
        {
            // Instantiate star burst effect at the collision point
            GameObject starBurst = Instantiate(starBurstPrefab, collision.contacts[0].point, Quaternion.identity);

            if (fart != null)
            {
                fart.DecreaseFartPower(fartPowerDecrease);
            }

            Debug.Log("Fart Power Decrease");

            // Destroy the star burst after 1 second
            Destroy(starBurst, 1f);
        }
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            GameObject starBurst = Instantiate(starBurstPrefab, collision.contacts[0].point, Quaternion.identity);
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
