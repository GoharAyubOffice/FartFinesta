using UnityEngine;
using UnityEngine.EventSystems; // Make sure this is included for UI interaction

public class FartPropulsion : MonoBehaviour
{
    public float continuousForce = 10f; // Adjusted continuous force
    private Rigidbody rb;

    public int fartPower = 1;
    public int playerPoints = 0;
    private const int maxFartPower = 100; // Maximum fart power

    [SerializeField] private AudioSource audioSource;     // Reference to the AudioSource component
    public AudioClip fartSound;          // The fart sound clip
    public ParticleSystem jumpParticles; // Reference to the particle system

    public bool isGameOver = false;    // Track if the game is over

    public GameObject gameOverScreen; // Reference to the game over UI

    private Animator animator;           // Reference to the Animator component

    private bool isHoldingJumpButton = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
        animator = GetComponent<Animator>(); // Get the Animator component

        rb.mass = 1f; // Set to the desired mass for consistent jump
        Physics.gravity = new Vector3(0, -9.81f, 0) * 2f; // Ensure gravity is set correctly

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

        // Apply continuous force while holding the button
        if (isHoldingJumpButton && !isGameOver)
        {
            ApplyContinuousFartForce();
        }
    }

    public void OnJumpButtonDown(BaseEventData eventData)
    {
        if (!isGameOver)
        {
            isHoldingJumpButton = true;
            // Optionally, you might want to play sound or particles here
            if (!audioSource.isPlaying)
            {
                audioSource.Play(); // Play the fart sound continuously
            }
        }
    }

    public void OnJumpButtonUp(BaseEventData eventData)
    {
        if (!isGameOver)
        {
            isHoldingJumpButton = false;
            // Stop particle system when button is released
            if (jumpParticles.isPlaying)
            {
                jumpParticles.Stop();
            }
        }
    }

    void ApplyContinuousFartForce()
    {
        if (fartPower > 0)
        {
            // Apply continuous upward force
            rb.AddForce(new Vector3(0,50,0) * continuousForce, ForceMode.Acceleration);
            rb.velocity *= 0.25f;

            // Ensure audio plays continuously while holding
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }

            // Play jump particles continuously
            if (!jumpParticles.isPlaying)
            {
                jumpParticles.Play();
            }

            // Set isJumping to true
            animator.SetBool("isJumping", true);
            animator.SetBool("isFall", false);

            // Decrease fart power over time while holding
            DecreaseFartPower(Time.deltaTime * 5);
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
            // Set isJumping and isFalling to false when the player lands on the ground
            animator.SetBool("isJumping", false);
            animator.SetBool("isFall", false);
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
        gameOverScreen.SetActive(true);

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
