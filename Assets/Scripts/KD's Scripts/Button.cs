using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [Tooltip("Color/TypeID for comparison when checking correct block. 0 is default/any.")]
    [SerializeField] public int typeID = 0;
    [SerializeField] LayerMask Interactable = 1 << 6;
    private void OnTriggerEnter(Collider other)
    {
        if ((Interactable & (1 << other.gameObject.layer)) != 0)
        {
            BlockContainer bC = other.GetComponentInParent<BlockContainer>();
            if (bC != null) { if (bC.typeID != typeID && typeID != 0) { return; } }
            Debug.Log(other.name + " has hit button");
            ButtonManager.buttonActivated?.Invoke();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if ((Interactable & (1 << other.gameObject.layer)) != 0)
        {
            BlockContainer bC = other.GetComponentInParent<BlockContainer>();
            if(bC != null) { if (bC.typeID != typeID && typeID != 0) { return; } }
            Debug.Log(other.name + " exits button");
            ButtonManager.buttonDeactivated?.Invoke();
        }
    }
}
