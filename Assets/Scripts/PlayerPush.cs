using UnityEngine;


/// <summary>
/// Manages the interaction between the character and other physics objects it can push.
/// </summary>
public class PlayerPush : MonoBehaviour
{
    [SerializeField]
    private float pushPower = 2.0f; // Configurable push power for easier adjustments.

    /// <summary>
    /// Responds to collisions with rigidbodies to apply a pushing force.
    /// </summary>
    /// <param name="hit">Information about the collider hit.</param>
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // Return if no Rigidbody is attached or if it's kinematic (non-moveable)
        if (body == null || body.isKinematic)
        {
            return;
        }

        // Avoid pushing objects below by checking the y component of the movement direction
        if (hit.moveDirection.y < -0.3)
        {
            return;
        }

        // Calculate push direction from move direction, restrict push to horizontal plane
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // Apply the push with configured power
        body.velocity = pushDir * pushPower;
    }
}
