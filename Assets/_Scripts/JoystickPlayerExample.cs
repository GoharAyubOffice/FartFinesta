using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickPlayerExample : MonoBehaviour
{
    public float speed = 20f;
    public VariableJoystick variableJoystick;
    public Rigidbody rb;

    public static bool isInParticleCollision = false;
    public int fartPower = 1;
    public int playerPoints = 0;

    public float gravityScale = 2f;

    public ParticleSystem jumpParticles;
    public ParticleSystem dustParticles;
    private Animator animator;
    private float originalRotationY;
    private const float rotationAngle = 90f; // Rotation angle for left/right movement
    [SerializeField] private float rotationTransitionSpeed = 5f; // Speed of rotation transition

    public bool isWalking { get; private set; }
    [SerializeField] private bool isGrounded;
    [SerializeField] private float isGroundedTransform;

    [SerializeField] private AudioSource audioSource;
    public float footStepSoundVolume = 1.5f;
    public AudioClip footStepSound;   // Sound for footstep

    private bool isPlayingFootstep = false; // To prevent overlapping sounds

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

        animator.SetBool("isWalking", isWalking);

        // Calculate and set target rotation
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
            // Smoothly transition to the original rotation when not moving horizontally
            transform.rotation = Quaternion.Euler(0, originalRotationY, 0);
        }

        // Control the dust particles emission based on movement
        var emission = dustParticles.emission;
        emission.enabled = isWalking;

        // Check if player is grounded
        isGrounded = IsGrounded();
        animator.SetBool("isGrounded", isGrounded);
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

    private bool IsGrounded()
    {
        // Check if the player is grounded using a raycast
        return Physics.Raycast(transform.position, Vector3.down, isGroundedTransform);
    }

    private void PlayFootstepSound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = footStepSound;
            audioSource.loop = true; // Loop the footstep sound
            audioSource.Play();
            isPlayingFootstep = true;
        }
    }

    private void StopFootstepSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            isPlayingFootstep = false;
        }
    }
}
