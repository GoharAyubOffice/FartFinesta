using UnityEngine;

public class FartPropulsion : MonoBehaviour
{
    public float fartForce = 10f;
    public float maxFartForce = 20f;
    public float tapTimeThreshold = 0.5f; // Adjust as needed
    private float tapStartTime;
    private float screenWidth;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        screenWidth = Screen.width;
    }

    void Update()
    {
        // Check if there is a touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Check if the touch is on the right half of the screen
            if (touch.position.x > screenWidth / 2 && touch.phase == TouchPhase.Began)
            {
                tapStartTime = Time.time;
            }
            else if (touch.position.x > screenWidth / 2 && touch.phase == TouchPhase.Ended)
            {
                float tapDuration = Time.time - tapStartTime;
                float tapForce = Mathf.Lerp(fartForce, maxFartForce, tapDuration / tapTimeThreshold);

                // Call method to apply force to the character
                ApplyFartForce(tapForce);
            }
        }
    }

    void ApplyFartForce(float force)
    {
        rb.velocity = Vector3.zero; // Reset velocity
        rb.AddForce(Vector3.up * force, ForceMode.Impulse);
    }
}
