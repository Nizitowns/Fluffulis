using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that handles contact with blocks, which pushes them in one of four directions, depending on the vector from player to block.
/// </summary>
public class InteractionHandler : MonoBehaviour
{
    [SerializeField] int groundLayer;
    [SerializeField] int interactableLayer;
    private float registerPushTime = 0.2f;

    /// <summary>
    /// hits is a dictionary for remembering UnitBlocks that were pushed, and how long they have been pushed (as they LERP towards the new grid position).
    /// </summary>
    private Dictionary<UnitBlock, float[]> hits = new Dictionary<UnitBlock, float[]>();
    
    /// <summary>
    /// Initiates the pushing of blocks when the right conditions are met. This includes contact with a block,
    /// and that the push cooldown has passed.
    /// </summary>
    /// <param name="hit"> Information on the object that was hit. </param>
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // if contact with ground, do nothing
        if(hit.gameObject.layer == groundLayer) { return; }
        
        // if contact with something interactable
        if (hit.gameObject.layer == interactableLayer)
        {
            // checks for contact with grid object (block).
            GridObject gridObj = hit.transform.parent.GetComponentInChildren<GridObject>();
            if(gridObj != null)
            {
                gridObj.CheckPush();
            }

            // if contact has UnitBlock component
            UnitBlock uBlock = hit.transform.parent.GetComponentInChildren<UnitBlock>();
            if (uBlock != null)
            {
                // if the uBlock that was pushed wasn't pushed before, or if enough time passed (there's a push cooldown),
                // begin the process to push the block
                if(!hits.ContainsKey(uBlock) || (Time.time - hits[uBlock][1]) > registerPushTime * 2) 
                {
                    hits[uBlock] = new float[] { 0, Time.time };
                }

                // pushes the block and tracks cooldown
                // Note: maybe start a coroutine to log the time passed instead, since this section of code only gets called when the player is in contact with the block.
                hits[uBlock][0] += Time.deltaTime;
                if(hits[uBlock][0] < registerPushTime) { return; }
                uBlock.Push();
                hits[uBlock][0] = 0;
            }

        }
    }
}
