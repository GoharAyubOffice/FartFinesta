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
    private bool isFacingRight = true;   // Track the direction the player is facing

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;      // Ensure rotation is frozen to prevent unexpected behavior
        Physics.gravity *= gravityScale; // Scale default gravity
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    void Update()
    {
        // Update animator with movement state only for horizontal movement
        bool isMovingHorizontally = Mathf.Abs(variableJoystick.Horizontal) > 0.1f; // Adjust threshold as needed
        animator.SetBool("isWalking", isMovingHorizontally); // Assumes the Animator has a boolean parameter named "isWalking"

        // Flip the player sprite based on the direction of movement
        if (isMovingHorizontally)
        {
            if (variableJoystick.Horizontal > 0 && !isFacingRight)
            {
                Flip();
            }
            else if (variableJoystick.Horizontal < 0 && isFacingRight)
            {
                Flip();
            }
        }
    }

    public void FixedUpdate()
    {
        // Movement based on joystick input
        Vector3 direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
        rb.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1; // Flip the x scale to change direction
        transform.localScale = scaler;
    }
}
