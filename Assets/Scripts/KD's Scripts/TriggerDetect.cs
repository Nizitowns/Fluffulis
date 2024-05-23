using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetect : MonoBehaviour
{
    private Dictionary<Transform, Transform> onTheElevator = new Dictionary<Transform, Transform>();
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("trigger enter: " + other.transform.root.name);
        if (other.transform.name == "GroundCheck" && other.gameObject.GetComponentInParent<BlockContainer>() == null) { Debug.Log(other.transform.root.name + " is groundcheck"); return; }
        if (onTheElevator.ContainsKey(other.transform)) { Debug.Log(other.transform.name + ",  " + other.transform.root.name + "exists"); return; }
        if(transform.root == other.transform.root) { return; }
        Debug.Log("elevator adds: " + other.transform.root.name);
        onTheElevator.Add(other.transform, other.transform.root);
        other.transform.root.parent = transform.root;
    }
    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("trigger exit: " + name);
        //Elevator elevator;
        //if (!TryGetComponent(out elevator)) { return; }
        if (!onTheElevator.ContainsKey(other.transform)) { return; }
        if (other.transform.name == "GroundCheck") { return; }
        Debug.Log("elevator removes: " + onTheElevator[other.transform].name);
        onTheElevator[other.transform].parent = null;
        onTheElevator.Remove(other.transform);


    }
}
