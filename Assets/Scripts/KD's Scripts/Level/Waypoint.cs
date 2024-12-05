using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Waypoint helps visualize waypoints when debugging by drawing a sphere around the waypoint.
/// </summary>
public class Waypoint : MonoBehaviour
{
    /// <summary>
    /// Draws a sphere of size 0.5f around the transform's position.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, .5f);
    }
}
