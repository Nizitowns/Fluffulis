using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBlockGravity : MonoBehaviour
{
    UnitBlock block;
    /// <summary>
    /// Finds the UnitBlock component associated with this component.
    /// </summary>
    private void Awake()
    {
        block = gameObject.GetComponentInParent<UnitBlock>();
    }
    /// <summary>
    /// Attempts to apply gravity at the start of the scene.
    /// </summary>
    private void Start()
    {
        block.currentContainer.ReceiveGravity();
    }
    /// <summary>
    /// Attempts to apply gravity whenever the UnitBlock exits contact with another collider.
    /// </summary>
    /// <param name="other"> The other collider that the UnitBlock exits contact from. </param>
    private void OnTriggerExit(Collider other)
    {
        block.currentContainer.ReceiveGravity();
    }

}
