using UnityEngine;

public class BeansCollectible : MonoBehaviour
{
    public int fartPowerIncrease = 20;
    public int points = 100; // Increase points by 100

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FartPropulsion fart = other.GetComponent<FartPropulsion>();
            if (fart != null)
            {
                fart.IncreaseFartPower(fartPowerIncrease);
                fart.AddPoints(points);
            }

            // Optionally, play a sound or particle effect here

            // Destroy the beans object after collection
            Destroy(gameObject);
        }
    }
}
