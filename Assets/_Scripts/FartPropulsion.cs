using UnityEngine;
using UnityEngine.EventSystems;

public class FartPropulsion : MonoBehaviour
{
    public float continuousForce = 10f; // Adjusted continuous force
    private Rigidbody rb;

    public int fartPower = 1;
    public int playerPoints = 0;
    private const int maxFartPower = 100; // Maximum fart power

    [SerializeField] private AudioSource audioSource;     // Reference to the AudioSource component
    public ParticleSystem jumpParticles; // Reference to the particle system

    public bool isGameOver = false;    // Track if the game is over

    public GameObject gameOverScreen; // Reference to the game over UI

    [SerializeField] private Animator animator;           // Reference to the Animator component

    private bool isHoldingJumpButton = false;

    public float normalGravityScale = 2f; // Gravity scale for normal gameplay
    public float fallGravityScale = 5f;   // Gravity scale for faster falling

    private bool isFlying = false; // Track if the player is currently flying

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>(); // Get the Animator component

        rb.mass = 1f; // Set to the desired mass for consistent jump
        Physics.gravity = new Vector3(0, -9.81f, 0) * normalGravityScale; // Set normal gravity

        // Ensure the audio source is set to loop
        audioSource.loop = true;
    }

    void Update()
    {
        if (fartPower <= 0 && !isGameOver)
        {
            GameOver();
        }

        // Check if the player is falling
        bool isFalling = !IsGrounded() && rb.linearVelocity.y < 0;

        if (isFalling)
        {
            // Apply fall gravity when falling
            rb.AddForce(Vector3.down * (fallGravityScale - normalGravityScale) * 9.81f, ForceMode.Acceleration);

            // Start playing the fly sound only if it's not already playing
            if (!isFlying)
            {
                PlayRocketSound();
                isFlying = true;
            }
        }
        else if (IsGrounded())
        {
            // Stop the rocket sound when grounded
            StopRocketSound();
            isFlying = false; // Reset flying state
        }

        // Update animator parameters
        animator.SetBool("isFalling", isFalling);

        // Apply continuous force while holding the jump button
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

            // Set the jump animation immediately
            animator.SetBool("isJumping", true);
            animator.SetBool("isFalling", false);

            // Ensure the rocket sound starts playing
            if (!isFlying)
            {
                PlayRocketSound();
                isFlying = true;
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

            // Set jump animation to false when the button is released
            animator.SetBool("isJumping", false);
        }
    }

    void ApplyContinuousFartForce()
    {
        if (fartPower > 0)
        {
            // Apply continuous upward force
            rb.AddForce(new Vector3(0, 50, 0) * continuousForce, ForceMode.Acceleration);
            rb.linearVelocity *= 0.25f;

            // Play jump particles continuously
            if (!jumpParticles.isPlaying)
            {
                jumpParticles.Play();
            }

            // Ensure the rocket sound continues to play
            if (!isFlying)
            {
                PlayRocketSound();
                isFlying = true;
            }

            // Set isJumping to true
            animator.SetBool("isJumping", true);
            animator.SetBool("isFalling", false);

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
        if (collision.gameObject.CompareTag("Obstacle") || IsGrounded())
        {
            // Set isJumping and isFalling to false when the player lands on the ground
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);

            // Stop the rocket sound when the player lands on the ground
            StopRocketSound();
            isFlying = false; // Reset flying state
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
        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;

        // Display game over UI or restart game logic
        FindObjectOfType<GameManager>().ShowGameOverScreen();

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

    private void PlayRocketSound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    private void StopRocketSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}