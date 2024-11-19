using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;  // Reference to the player's transform
    public Vector3 offset = new Vector3(0f, 2f, -5f); // Offset from player position
    public float smoothSpeed = 0.125f;  // Smoothness of camera movement
    public float rotationSpeed = 5f;    // Speed of camera rotation

    [Header("Shake Settings")]
    public float shakeDuration = 1f;    // How long the shake lasts
    public float shakeAmount = 0.2f;    // Shake intensity
    public float decreaseFactor = 1f; // Speed of shake decrease

    private Vector3 originalPosition;  // Original camera position
    private bool isShaking = false;    // Whether the camera is shaking

    void Start()
    {
        if (playerTransform == null)
        {
            Debug.LogError("Player Transform is not assigned!");
        }

        // Save the original position of the camera
        originalPosition = transform.localPosition;
    }

    void LateUpdate()
    {
        if (playerTransform != null)
        {
            // Camera follow logic
            Vector3 desiredPosition = playerTransform.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            // Smoothly rotate towards the player's direction
            Quaternion targetRotation = Quaternion.LookRotation(playerTransform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Camera shake logic
        if (isShaking && shakeDuration > 0)
        {
            transform.localPosition = originalPosition + Random.insideUnitSphere * shakeAmount;
            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else if (isShaking)
        {
            shakeDuration = 0f;
            isShaking = false;
            transform.localPosition = originalPosition;
        }
    }

    // Public method to trigger the camera shake
    public void TriggerShake(float duration, float amount)
    {
        shakeDuration = duration;
        shakeAmount = amount;
        isShaking = true;
        originalPosition = transform.localPosition; // Ensure we reset the original position
    }
}
