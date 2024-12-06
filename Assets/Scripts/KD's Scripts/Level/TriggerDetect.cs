using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TriggerDetect handles elevator functionality when in use or not in use.
/// </summary>
public class TriggerDetect : MonoBehaviour
{
    [HideInInspector]public Dictionary<Transform, Transform> onTheElevator = new Dictionary<Transform, Transform>();
    Elevator elevator;
    /// <summary>
    /// Finds the elevator component TriggerDetect is associated with.
    /// </summary>
    private void Awake()
    {
        elevator = GetComponentInParent<Elevator>();
    }
    /// <summary>
    /// Ensures that blocks on an elevator also move with the elevator.
    /// </summary>
    /// <param name="other"> A collider that has collided with the elevator. </param>
    private void OnTriggerEnter(Collider other)
    {
        Elevator otherElevator = other.GetComponentInParent<Elevator>();
        BlockContainer bC = other.gameObject.GetComponentInParent<BlockContainer>();
        // other is already child of elevator
        if(otherElevator != null && otherElevator.transform == elevator.transform) { return; }
        if (other.transform.name == "GroundCheck" && bC == null) { Debug.Log(other.transform.root.name + " is groundcheck"); return; }
        if (onTheElevator.ContainsKey(other.transform))
        { 
            return; 
        }
        
        // other is not child of elevator, add to dict
        if(otherElevator == null && bC != null) 
        {
            onTheElevator.Add(other.transform, bC.transform);
            bC.transform.parent = elevator.transform;
        }
        // otherr is a child of different elevator
        else if(otherElevator != elevator && bC != null)
        {
            bC.transform.parent = null;
            otherElevator.GetComponentInChildren<TriggerDetect>().onTheElevator.Remove(other.transform);
            onTheElevator.Add(other.transform, bC.transform);
            bC.transform.parent = elevator.transform;
        }
    }
    /// <summary>
    /// Ensures that blocks off the elevator do not move with the elevator.
    /// </summary>
    /// <param name="other"> A collider that has exited the elevator. </param>
    private void OnTriggerExit(Collider other)
    {
        Elevator otherElevator = other.GetComponentInParent<Elevator>();
        BlockContainer bC = other.gameObject.GetComponentInParent<BlockContainer>();
        if(onTheElevator.ContainsKey(other.transform))
        {
            if(otherElevator == elevator && bC != null) 
            {
                bC.transform.parent = null;
                onTheElevator.Remove(other.transform);
            }
        }
    }
}
