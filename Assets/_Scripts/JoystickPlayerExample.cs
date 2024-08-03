using UnityEngine;

public class JoystickPlayerExample : MonoBehaviour
{
    public float speed = 20f;
    public VariableJoystick variableJoystick;
    public Rigidbody rb;

    public static bool isInParticleCollision = false;
    public int fartPower = 1;
    public int playerPoints = 0;

    public float gravityScale = 2f;

    public AudioClip fartSound;
    public ParticleSystem jumpParticles;
    public ParticleSystem dustParticles;
    private Animator animator;
    private float originalRotationY;
    private const float rotationAngle = 90f;
    [SerializeField] private float rotationAngleMoveSpeed = 2f;


    public bool isWalking { get; private set; }
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        Physics.gravity = new Vector3(0, -9.81f, 0) * gravityScale;
        animator = GetComponent<Animator>();

        originalRotationY = transform.eulerAngles.y;
        isWalking = true;
    }

    void Update()
    {
        // Determine if the player is walking
        isWalking = Mathf.Abs(variableJoystick.Horizontal) > 0.1f;
        animator.SetBool("isWalking", isWalking);

        // Rotate the player based on the joystick input
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
            // Return to the original rotation when not moving horizontally
            Quaternion targetRotation = Quaternion.Euler(0, originalRotationY, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationAngleMoveSpeed);
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
        return Physics.Raycast(transform.position, Vector3.down, 0.2f);
    }
}
