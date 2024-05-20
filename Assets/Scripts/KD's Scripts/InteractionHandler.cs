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
        //Debug.Log("collide with " + hit.gameObject.name);
        if (hit.gameObject.layer == interactableLayer)
        {
            //Debug.Log("collide with interactable" + hit.gameObject.name);
            GridObject gridObj = hit.transform.parent.GetComponentInChildren<GridObject>();
            if(gridObj != null)
            {
                gridObj.CheckPush();
            }
            UnitBlock uBlock = hit.transform.parent.GetComponentInChildren<UnitBlock>();
            if (uBlock != null)
            {
                uBlock.Push();
                //Debug.Log("uBlock push");
            }

        }
    }
}
