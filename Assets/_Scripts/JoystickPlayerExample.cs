using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickPlayerExample : MonoBehaviour
{
    public float speed = 20f;
    public VariableJoystick variableJoystick;
    public Rigidbody rb;

    public ParticleSystem jumpParticles;
    public ParticleSystem dustParticles;
    private Animator animator;
    private float originalRotationY;
    private const float rotationAngle = 90f; // Rotation angle for left/right movement

    public bool isWalking { get; private set; }
    [SerializeField] private bool isGrounded;
    [SerializeField] private float isGroundedTransform;

    [SerializeField] private AudioSource audioSource;
    public float footStepSoundVolume = 1.5f;
    public AudioClip footStepSound;   // Sound for footstep

    private bool isPlayingFootstep = false; // To prevent overlapping sounds
    [SerializeField] private float gravityScale;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        Physics.gravity = new Vector3(0, -9.81f, 0) * gravityScale;
        animator = GetComponent<Animator>();

        originalRotationY = transform.eulerAngles.y;
        isWalking = false;

        audioSource.volume = footStepSoundVolume; // Set the footstep sound volume
    }

    void Update()
    {
        // Determine if the player is walking
        isWalking = Mathf.Abs(variableJoystick.Horizontal) > 0.1f;

        if (isWalking && isGrounded && !isPlayingFootstep)
        {
            PlayFootstepSound(); // Play footstep sound when walking
        }
        else if (!isWalking || !isGrounded)
        {
            StopFootstepSound(); // Stop the sound when not walking or in the air
        }

        // Handle rotation when walking
        if (isWalking)
        {
            if (variableJoystick.Horizontal > 0)
            {
                RotatePlayer(rotationAngle); // Rotate to the right
            }
            else if (variableJoystick.Horizontal < 0)
            {
                RotatePlayer(-rotationAngle); // Rotate to the left
            }
        }
        else
        {
            // Smoothly transition back to the original rotation when not moving horizontally
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, originalRotationY, 0), Time.deltaTime * 10f);
        }

        // Control the dust particles emission based on movement
        var emission = dustParticles.emission;
        emission.enabled = isWalking;

        // Update animator parameters
        isGrounded = IsGrounded();
        animator.SetBool("isGrounded", isGrounded);

        // If the player is jumping, disable the walking animation
        if (animator.GetBool("isJumping"))
        {
            animator.SetBool("isWalking", false);
        }
        else
        {
            animator.SetBool("isWalking", isWalking);
        }
    }

    void FixedUpdate()
    {
        // Movement based on joystick input
        Vector3 direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
        rb.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }

    void RotatePlayer(float angle)
    {
        // Rotate the player around the Y-axis
        transform.rotation = Quaternion.Euler(0, originalRotationY + angle, 0);
    }

    public void OnJumpButtonDown(BaseEventData eventData)
    {
        if (isGrounded)
        {
            // Set the jump animation and make sure it overrides the walking animation
            animator.SetBool("isJumping", true);
            animator.SetBool("isWalking", false);

            // Apply the jump force
            rb.AddForce(Vector3.up * 10f, ForceMode.VelocityChange);

            // Play jump particles
            if (jumpParticles != null)
            {
                jumpParticles.Play();
            }
        }
    }

    public void OnJumpButtonUp(BaseEventData eventData)
    {
        // Reset jumping state when the button is released
        animator.SetBool("isJumping", false);
    }

    private bool IsGrounded()
    {
        // Check if the player is grounded using a raycast
        return Physics.Raycast(transform.position, Vector3.down, isGroundedTransform);
    }

    private void PlayFootstepSound()
    {
        audioSource.clip = footStepSound;
        audioSource.loop = true; // Loop the footstep sound
        audioSource.Play();
        isPlayingFootstep = true;
    }

    private void StopFootstepSound()
    {
        audioSource.Stop();
        isPlayingFootstep = false;
    }
}
