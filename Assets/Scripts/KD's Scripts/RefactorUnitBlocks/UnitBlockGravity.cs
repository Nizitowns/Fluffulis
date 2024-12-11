using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBlockGravity : MonoBehaviour
{
    UnitBlock block;
    private void Awake()
    {
        block = gameObject.GetComponentInParent<UnitBlock>();
    }
    private void Start()
    {
        block.currentContainer.ReceiveGravity();
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log(name + "exits " + other.name + " and " + block.currentContainer.transform.name + "receives gravity");
        block.currentContainer.ReceiveGravity();
    }

}
