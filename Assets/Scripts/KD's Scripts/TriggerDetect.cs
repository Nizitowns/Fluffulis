using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetect : MonoBehaviour
{
    [HideInInspector]public Dictionary<Transform, Transform> onTheElevator = new Dictionary<Transform, Transform>();
    //[SerializeField] public Elevator elevator;
    Elevator elevator;
    private void Awake()
    {
        elevator = GetComponentInParent<Elevator>();
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    //Debug.Log("trigger enter: " + other.transform.root.name);
    //    if (other.transform.name == "GroundCheck" && other.gameObject.GetComponentInParent<BlockContainer>() == null) { Debug.Log(other.transform.root.name + " is groundcheck"); return; }
    //    if (onTheElevator.ContainsKey(other.transform)) { Debug.Log(other.transform.name + ",  " + other.transform.root.name + "exists"); return; }
    //    if(transform.root == other.transform.root) { return; }
    //    Debug.Log("elevator adds: " + other.transform.root.name);
    //    onTheElevator.Add(other.transform, other.transform.root);
    //    other.transform.root.parent = transform.root;
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    //Debug.Log("trigger exit: " + name);
    //    //Elevator elevator;
    //    //if (!TryGetComponent(out elevator)) { return; }
    //    if (!onTheElevator.ContainsKey(other.transform)) { return; }
    //    if (other.transform.name == "GroundCheck") { return; }
    //    Debug.Log("elevator removes: " + onTheElevator[other.transform].name);
    //    onTheElevator[other.transform].parent = null;
    //    onTheElevator.Remove(other.transform);


    //}

    private void OnTriggerEnter(Collider other)
    {
        Elevator otherElevator = other.GetComponentInParent<Elevator>();
        BlockContainer bC = other.gameObject.GetComponentInParent<BlockContainer>();
        // other is already child of elevator
        if(otherElevator != null && otherElevator.transform == elevator.transform) { return; }
        if (other.transform.name == "GroundCheck" && bC == null) { Debug.Log(other.transform.root.name + " is groundcheck"); return; }
        if (onTheElevator.ContainsKey(other.transform))
        { 
            Debug.Log(other.transform.name + ",  " + other.transform.root.name + "exists"); 
            return; 
        }
        
        // other is not child of elevator, add to dict
        if(otherElevator == null && bC != null) 
        {
            Debug.Log(bC.name + " was added to " + elevator.name);
            onTheElevator.Add(other.transform, bC.transform);
            bC.transform.parent = elevator.transform;
        }
        // otherr is a child of different elevator
        else if(otherElevator != elevator && bC != null)
        {
            Debug.Log(bC.name + " was added to " + elevator.name);
            bC.transform.parent = null;
            otherElevator.GetComponentInChildren<TriggerDetect>().onTheElevator.Remove(other.transform);
            onTheElevator.Add(other.transform, bC.transform);
            bC.transform.parent = elevator.transform;
        }
    }
}
