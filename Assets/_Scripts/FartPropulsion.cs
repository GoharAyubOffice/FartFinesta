using UnityEngine;

public class FartPropulsion : MonoBehaviour
{
    public float fartForce = 10f;
    public float maxFartForce = 20f;
    public float tapTimeThreshold = 0.5f; // Adjust as needed
    private float tapStartTime;
    private Rigidbody rb;

    public int fartPower = 1;
    public int playerPoints = 0;
    private const int maxFartPower = 100; // Maximum fart power

    [SerializeField] private AudioSource audioSource;     // Reference to the AudioSource component
    public AudioClip fartSound;          // The fart sound clip
    public ParticleSystem jumpParticles; // Reference to the particle system

    public bool isGameOver = false;    // Track if the game is over

    [SerializeField] private Animator animator;           // Reference to the Animator component

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
        animator = GetComponent<Animator>(); // Get the Animator component

        if (animator == null)
        {
            Debug.LogError("Animator component not found on the GameObject.");
        }
    }

    void Update()
    {
        if (fartPower <= 0 && !isGameOver)
        {
            GameOver();
        }

        // Update animator parameters
        if (!IsGrounded() && rb.velocity.y < 0)
        {
            animator.SetBool("isFall", true);
        }
        if (IsGrounded())
        {
            animator.SetBool("isFall", false);
        }
    }

    public void OnJumpButtonDown()
    {
        if (!isGameOver)
        {
            float tapDuration = Time.time - tapStartTime;
            float tapForce = Mathf.Lerp(fartForce, maxFartForce, tapDuration / tapTimeThreshold);
            ApplyFartForce(tapForce);
        }
    }

    void ApplyFartForce(float force)
    {
        if (fartPower > 0)
        {
            rb.velocity = Vector3.zero; // Reset velocity
            rb.AddForce(Vector3.up * force, ForceMode.Impulse);
            DecreaseFartPower(1); // Decrease fart power by 1 on jump
            audioSource.PlayOneShot(fartSound); // Play the fart sound
            jumpParticles.Play(); // Play jump particles

            // Set isJumping to true
            if (animator != null)
            {
                animator.SetBool("isJumping", true);
                animator.SetBool("isFall", false);
            }
        }
    }

    private bool IsGrounded()
    {
        // Check if the player is grounded using a raycast
        return Physics.Raycast(transform.position, Vector3.down, 0.2f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            // Set isJumping and isFall to false when the player lands on the ground
            if (animator != null)
            {
                animator.SetBool("isJumping", false);
                animator.SetBool("isFall", false);
            }
        }
    }

    public void IncreaseFartPower(int amount)
    {
        fartPower += amount;
        if (fartPower > maxFartPower)
        {
            fartPower = maxFartPower;
        }
        // Update UI if necessary
    }

    public void DecreaseFartPower(float amount)
    {
        fartPower -= (int)amount;
        if (fartPower < 0) fartPower = 0;
        // Update UI if necessary
    }

    public void AddPoints(int points)
    {
        playerPoints += points;
        // Update UI if necessary
    }

    public void GameOver()
    {
        isGameOver = true;
        // Stop player movement
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        // Display game over UI or restart game logic
        Debug.Log("Game Over! Fart Power is 0.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Beans"))
        {
            IncreaseFartPower(1);
            AddPoints(100);
            Destroy(other.gameObject); // Destroy the collectible
        }
        else if (other.CompareTag("Traps"))
        {
            DecreaseFartPower(10); // Decrease fart power by 10 for hitting a trap
            // Play collision sound
            AudioSource collisionAudio = other.GetComponent<AudioSource>();
            if (collisionAudio != null)
            {
                collisionAudio.Play();
            }
        }
    }
}
