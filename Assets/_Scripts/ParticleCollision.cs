using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    public int fartPowerDecrease = 1;

    void OnParticleCollision(GameObject other)
    {
        // Handle collision here

        if (other.CompareTag("Player"))
        {
            FartPropulsion fart = other.GetComponent<FartPropulsion>();
            if (fart != null)
            {
                fart.DecreaseFartPower(fartPowerDecrease);
            }

            Debug.Log("Fart Power Decrease");

            // Optionally, play a sound or particle effect here

        }
    }
}