using UnityEngine;

public class Rotator : MonoBehaviour
{
    public Vector3 rotationAxis = Vector3.up; // Axis of rotation (default is the Y-axis)
    public float rotationSpeed = 10f;         // Speed of rotation

    void Update()
    {
        // Rotate the object around the specified axis at the given speed
        transform.Rotate(-rotationAxis * rotationSpeed * Time.deltaTime);
    }
}
