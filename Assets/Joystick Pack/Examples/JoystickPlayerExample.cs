using UnityEngine;

public class JoystickPlayerExample : MonoBehaviour
{
    public float speed;
    public VariableJoystick variableJoystick;
    public Rigidbody rb;

    public static bool isInParticleCollision = false;
    public int fartPower = 1;
    public int playerPoints = 0;

    public float jumpForce = 10f;       // Force applied when jumping
    public float gravityScale = 2f;     // Adjust gravity scale for realistic fall
    private bool isGameOver = false;    // Track if the game is over

    private const int maxFartPower = 100; // Maximum fart power

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;      // Ensure rotation is frozen to prevent unexpected behavior
        Physics.gravity *= gravityScale; // Scale default gravity
    }

    private void Update()
    {
        if (isInParticleCollision)
        {
            DecreaseFartPower(1 * Time.deltaTime); // Decrease fart power continuously
        }

        if (fartPower <= 0 && !isGameOver)
        {
            GameOver();
        }

        if (Input.GetMouseButtonDown(0) && !isGameOver)
        {
            // Check if the touch is on the right side of the screen
            if (Input.mousePosition.x > Screen.width / 2)
            {
                Jump();
            }
        }
    }

    public void FixedUpdate()
    {
        // Movement based on joystick input
        Vector3 direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
        rb.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }

    void Jump()
    {
        if (fartPower > 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // Reset y velocity to zero for consistent jump
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);     // Apply jump force instantly
            DecreaseFartPower(1); // Decrease fart power by 1 on jump
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

    private void GameOver()
    {
        isGameOver = true;
        // Stop player movement
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        // Display game over UI or restart game logic
        Debug.Log("Game Over! Fart Power is 0.");
    }
}
