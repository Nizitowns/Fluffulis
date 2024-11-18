using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    [SerializeField] int groundLayer;
    [SerializeField] int interactableLayer;
    private float registerPushTime = 0.2f;
    //private float timeElapsed = 0;
    //private float timeHit = 0;
    private Dictionary<UnitBlock, float[]> hits = new Dictionary<UnitBlock, float[]>();
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
                if(!hits.ContainsKey(uBlock) || (Time.time - hits[uBlock][1]) > registerPushTime * 2) 
                {
                    hits[uBlock] = new float[] { 0, Time.time };
                    //hits[uBlock][0] = 0;
                    //hits[uBlock][1] = Time.time;
                }
                
                hits[uBlock][0] += Time.deltaTime;
                //Debug.Log("ublock time: " + hits[uBlock][0]);
                if(hits[uBlock][0] < registerPushTime) { return; }
                //Debug.Log("ublock push....");
                uBlock.Push();
                hits[uBlock][0] = 0;
            }

        }
    }
}
