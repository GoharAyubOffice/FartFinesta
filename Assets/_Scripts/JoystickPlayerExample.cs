using UnityEngine;

public class JoystickPlayerExample : MonoBehaviour
{
    public float speed;
    public VariableJoystick variableJoystick;
    public Rigidbody rb;

    public static bool isInParticleCollision = false;
    public int fartPower = 1;
    public int playerPoints = 0;

    public float gravityScale = 2f;     // Adjust gravity scale for realistic fall

    [SerializeField] private AudioSource audioSource;     // Reference to the AudioSource component
    public AudioClip fartSound;          // The fart sound clip
    public ParticleSystem jumpParticles; // Reference to the particle system
    private Animator animator;           // Reference to the Animator component
    private float originalRotationY;     // Store the original Y rotation of the player
    private const float rotationAngle = 90f; // Angle to rotate for left/right movement

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;      // Ensure rotation is frozen to prevent unexpected behavior
        Physics.gravity *= gravityScale; // Scale default gravity
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
        animator = GetComponent<Animator>(); // Get the Animator component

        // Store the original Y rotation of the player
        originalRotationY = transform.eulerAngles.y;
    }

    void Update()
    {
        // Update animator with movement state only for horizontal movement
        bool isMovingHorizontally = Mathf.Abs(variableJoystick.Horizontal) > 0.1f; // Adjust threshold as needed
        animator.SetBool("isWalking", isMovingHorizontally); // Assumes the Animator has a boolean parameter named "isWalking"

        // Rotate the player based on the joystick input
        if (isMovingHorizontally)
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
            // Return to the original rotation when not moving horizontally
            Quaternion targetRotation = Quaternion.Euler(0, originalRotationY, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
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
}
