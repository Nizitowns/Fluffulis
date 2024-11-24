using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A component for checking if the player is grounded.
/// </summary>
public class GroundCheck : MonoBehaviour
{
    public float radius = 0.22f;
    public LayerMask player = 1 << 9;

    /// <summary>
    /// Checks if the player is grounded.
    /// </summary>
    /// <returns> True if player is touching ground, false otherwise. </returns>
    public bool Grounded()
    {
        bool isGrounded = Physics.CheckSphere(transform.position, radius, ~player, QueryTriggerInteraction.Ignore);
        return isGrounded;    
    }

    /// <summary>
    /// Draws a sphere gizmo, viewable in play mode for debugging.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
