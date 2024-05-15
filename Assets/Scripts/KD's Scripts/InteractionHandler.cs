using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    [SerializeField] int groundLayer;
    [SerializeField] int interactableLayer;
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.layer == groundLayer) { return; }
        if(hit.gameObject.layer == interactableLayer)
        {
            Debug.Log("collide with interactable" + hit.gameObject.name);
            hit.gameObject.TryGetComponent<GridObject>(out GridObject grid);
            grid.CheckPush();
        }
    }
}
