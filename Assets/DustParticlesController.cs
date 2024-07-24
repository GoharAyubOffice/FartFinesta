using UnityEngine;

public class DustParticlesController : MonoBehaviour
{
    public ParticleSystem dustParticles; // Reference to the Particle System
    private JoystickPlayerExample playerController; // Reference to the player controller

    void Start()
    {
        playerController = GetComponent<JoystickPlayerExample>();
        if (dustParticles == null)
        {
            Debug.LogError("Dust particles not assigned.");
        }
    }

    void Update()
    {
        if (playerController.isWalking && !dustParticles.isPlaying)
        {
            dustParticles.Play();
        }
        else if (!playerController.isWalking && dustParticles.isPlaying)
        {
            dustParticles.Stop();
        }
    }
}
