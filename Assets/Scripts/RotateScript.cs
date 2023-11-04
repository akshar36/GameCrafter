using UnityEngine;

public class RotateScript: MonoBehaviour
{
    public float rotationSpeed = 90.0f; // Adjust the rotation speed as needed (90 degrees per second)

    private float timeSinceRotation = 0f;
    private float rotationInterval = 5f; // Time interval between rotations (5 seconds)

    private void Update()
    {
        // Rotate the parent GameObject (the entire scene) clockwise
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        // Track time since the last rotation
        timeSinceRotation += Time.deltaTime;

        // Check if it's time to rotate again (every 5 seconds)
        if (timeSinceRotation >= rotationInterval)
        {
            // Reset the timer
            timeSinceRotation = 0f;
        }
    }
}
