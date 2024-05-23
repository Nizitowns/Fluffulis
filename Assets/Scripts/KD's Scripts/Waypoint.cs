using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, .5f);
    }
}
