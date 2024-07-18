using UnityEngine;

public class CollisionSoundManager : MonoBehaviour
{
    public AudioClip thudSound;   // Sound for obstacle collision
    public AudioClip collectSound; // Sound for bean collection

    private AudioSource audioSource;

    // Volume multipliers for each sound type
    public float thudVolume = 1.5f;
    public float splatVolume = 1.0f;
    public float collectVolume = 1.0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check the tag of the collided object and play the corresponding sound
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            PlaySound(thudSound, thudVolume);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check the tag of the trigger object and play the corresponding sound
        if (other.gameObject.CompareTag("Beans"))
        {
            PlaySound(collectSound, collectVolume);
        }
    }

    void PlaySound(AudioClip clip, float volume)
    {
        audioSource.PlayOneShot(clip, volume);
    }
}
