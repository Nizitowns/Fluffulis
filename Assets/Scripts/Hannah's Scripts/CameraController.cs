using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float rotationSpeed = 5.0f; // Speed of the rotation interpolation
    public Vector3 offset; // Offset from the player position
    private Vector3 targetOffset; // Target offset for smooth transition

    void Start()
    {
        // Initialize the target offset based on initial offset
        targetOffset = offset;
        // Set the initial position of the camera
        UpdateCameraPosition();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RotateCamera(90); // Rotate right
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RotateCamera(-90); // Rotate left
        }

        // Smoothly interpolate the current offset to the target offset
        offset = Vector3.Lerp(offset, targetOffset, rotationSpeed * Time.deltaTime);
        UpdateCameraPosition();
    }

    private void RotateCamera(int angle)
    {
        // Calculate rotation around the player
        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        targetOffset = rotation * targetOffset;

        // Rotate the player to align with the camera's new orientation
        //  player.rotation = Quaternion.Euler(0, player.eulerAngles.x + angle, 0);
    }

    private void UpdateCameraPosition()
    {
        // Update the position and rotation of the camera based on the new offset
        Vector3 newPos = player.position + offset;
        transform.position = newPos;
        transform.LookAt(player.position);
    }
}
