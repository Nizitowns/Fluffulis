using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBlock : MonoBehaviour
{
    /// <summary>
    /// Layers that cause blockage
    /// BlockLayers / IsBlocked() need to be adjusted to account for adjacent blocks apart of same container
    /// </summary>
    [SerializeField] LayerMask BlockLayers = ~0;
    public Grid grid;
    private BlockContainer originalContainer;
    public BlockContainer currentContainer;
    private BoxCollider blockCollider;
    private MeshRenderer meshRenderer;
    private Transform groundCheck;

    private void Awake()
    {
        // set originalContainer
        originalContainer = currentContainer = transform.GetComponentInParent<BlockContainer>();
        grid = GameObject.Find("GridManager").GetComponent<Grid>();
        blockCollider = GetComponentInChildren<BoxCollider>();
        groundCheck = GetComponentInChildren<Rigidbody>().transform;
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        Snap(transform.position);

        
    }

    //public delegate void ReceivePush(UnitBlock block);
    //public ReceivePush receivePush;
    //public void Push() { receivePush?.Invoke(this); }
    public void Push() { currentContainer.ReceivePush(this); }
    public Vector3 GetPushDirection()
    {
        //if (!isPushable) { return; }
        //if (pushing) { return; }
        Vector3[] directions = { transform.forward, -transform.forward, transform.right, -transform.right };
        foreach (Vector3 dir in directions)
        {
            if (IsPushDirection(dir))
            {
                return -dir;
            }
        }
        return Vector3.zero;
    }

    private bool IsPushDirection(Vector3 direction)
    {
        //Debug.Log("Checking push direction: " + direction);
        RaycastHit[][] hits;
        if (Mathf.Approximately(direction.x, 0))
        {
            Debug.DrawRay(transform.position, direction, Color.red, 3f);
            Debug.DrawRay(transform.position + transform.right * 0.33f, direction, Color.red, 3f);
            Debug.DrawRay(transform.position - transform.right * 0.33f, direction, Color.red, 3f);
            hits = new RaycastHit[3][] {
                Physics.RaycastAll(transform.position + transform.right * 0.33f, direction),
                Physics.RaycastAll(transform.position + transform.right * -0.33f, direction),
                Physics.RaycastAll(transform.position, direction),
                };
        }
        else {
            Debug.DrawRay(transform.position, direction, Color.green, 3f);
            Debug.DrawRay(transform.position + transform.forward * 0.33f, direction, Color.green, 3f);
            Debug.DrawRay(transform.position - transform.forward * 0.33f, direction, Color.green, 3f);
            hits = new RaycastHit[3][] {
                Physics.RaycastAll(transform.position + transform.forward * 0.33f, direction),
                Physics.RaycastAll(transform.position + transform.forward * -0.33f, direction),
                Physics.RaycastAll(transform.position, direction),
                };
        }
        for (int i=0; i<3; i++)
        {
            for(int j=0; j<hits[i].Length; j++)
            {
                //Debug.Log(hits[i][j].transform.name + ", " + hits[i][j].transform.root.name + " was hit");
                if (hits[i][j].transform.CompareTag("Player")) 
                {
                    //Debug.Log(hits[i][j].transform.name + ", " + hits[i][j].transform.root.name + " was hit");
                    return true; 
                }
                
            }
        }
        return false;
    }
    //public bool IsBlocked(Vector3 direction)
    //{
    //    //Debug.Log(name + " is being checked for block (UnitBlock)");
    //    Debug.DrawRay(transform.position, direction * 1.2f, Color.red);

    //    if (Physics.Raycast(transform.position, direction, 1.2f, BlockLayers, QueryTriggerInteraction.Collide)) 
    //    { 
    //        foreach(RaycastHit hit in Physics.RaycastAll(transform.position, direction, 1.2f, BlockLayers, QueryTriggerInteraction.Collide))
    //        {
    //            //Debug.Log("hits: " + hit.transform.name);
    //            if(hit.transform.gameObject.layer == 2) 
    //            { 
    //                //Debug.Log(name + "is not blocked by " + hit.transform.name + " " + hit.transform.gameObject.layer); 
    //                return false; 
    //            }
    //            if (hit.transform.gameObject.GetComponentInParent<BlockContainer>() == currentContainer) 
    //            {

    //                return false; 
    //            }
    //            //Debug.Log(name + "is not blocked by " + hit.transform.name);
    //        }
    //        //return false;
    //        //Debug.Log(name + " is blocked...");
    //        return true;
    //    }
    //    //Debug.Log(name + "is not blocked!!!");
    //    return false;
    //}
    public bool IsBlocked(Vector3 direction)
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction);
        for(int i=0; i<hits.Length; i++)
        {
            if(hits[i].transform.gameObject.layer == 2) { continue; }
            if(hits[i].transform.gameObject.GetComponentInParent<BlockContainer>() == currentContainer) { continue; }
            if(Vector3.Distance(hits[i].point, transform.position) > 1.2f) 
            { 
                continue; 
            }
            //Debug.Log(hits[i].transform.name + ", " + hits[i].transform.root.name + "is blocking " + name);
            return true;
        }
        return false;
    }
    public void Snap(Vector3 target)
    {
        //Debug.Log("Snap");
        transform.position = grid.WorldToCell(target);
        //meshRenderer.transform.localPosition = blockCollider.transform.localPosition = Vector3.zero;
        //groundCheck.transform.localPosition = Vector3.down * 0.5f;
        meshRenderer.transform.position = grid.WorldToCell(target);
        blockCollider.transform.position = grid.WorldToCell(target);
        //Debug.Log(transform.parent.name + " snaps to " + grid.WorldToCell(target));
        groundCheck.transform.position = grid.WorldToCell(target) + Vector3.down * 0.5f;
        //groundCheck.transform.position = transform.position + Vector3.down * 0.5f;
    }
}
