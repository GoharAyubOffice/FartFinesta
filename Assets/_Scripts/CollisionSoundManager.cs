using UnityEngine;

public class CollisionSoundManager : MonoBehaviour
{
    public AudioClip thudSound;   // Sound for obstacle collision
    public AudioClip collectSound; // Sound for bean collection
    public AudioClip hitSound; // Sound for bean collection
    public AudioClip feathercoinSound;

    [SerializeField] private AudioSource audioSource;

    // Volume multipliers for each sound type
    public float thudVolume = 1.5f;
    public float splatVolume = 1.0f;
    public float collectVolume = 1.0f;
    public float hitVolume = 1.0f;
    public float feathercoinVolume = 1.0f;

    void OnCollisionEnter(Collision collision)
    {
        // Check the tag of the collided object and play the corresponding sound
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            PlaySound(thudSound, thudVolume);
        }
        if(collision.gameObject.CompareTag("Traps"))
        {
            PlaySound(hitSound, hitVolume);
        }
       
    }

    void OnTriggerEnter(Collider other)
    {
        // Check the tag of the trigger object and play the corresponding sound
        if (other.gameObject.CompareTag("Beans"))
        {
            PlaySound(collectSound, collectVolume);
        }
        if (other.gameObject.CompareTag("Coin"))
        {
            PlaySound(feathercoinSound, collectVolume);

            // Spawn particles


        }
    }

    public void SpawnParticles(Vector3 position, GameObject particlePrefab)
    {
        // Instantiate the particle effect at the given position
        GameObject particles = Instantiate(particlePrefab, position, Quaternion.identity);

        // Destroy the particle effect after a short duration
        Destroy(particles, 2f); // Adjust "2f" to the duration of your particle effect
    }

    void PlaySound(AudioClip clip, float volume)
    {
        audioSource.PlayOneShot(clip, volume);
    }
}
