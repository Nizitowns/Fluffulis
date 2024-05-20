using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public float radius = 0.22f;
    public LayerMask player = 1 << 9;
    public bool Grounded()
    {
        bool isGrounded = Physics.CheckSphere(transform.position, radius, ~player, QueryTriggerInteraction.Ignore);
        //Debug.Log("isGrounded: " + isGrounded);
        return isGrounded;
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
