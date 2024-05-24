using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBlockGravity : MonoBehaviour
{
    UnitBlock block;
    //public delegate void ReceiveGravity();
    //public ReceiveGravity receiveGravity;
    private void Awake()
    {
        block = gameObject.GetComponentInParent<UnitBlock>();
    }
    private void Start()
    {
        //receiveGravity?.Invoke();
        block.currentContainer.ReceiveGravity();
    }
    private void OnTriggerExit(Collider other)
    {
        //receiveGravity?.Invoke();
        Debug.Log(name + "exits " + other.name + " and " + block.currentContainer.transform.name + "receives gravity");
        block.currentContainer.ReceiveGravity();
    }

}
