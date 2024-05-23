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
                //Push(-dir);
                //pushing = true;
                //isPushable = false;
                return -dir;
            }
        }
        return Vector3.zero;
    }
    private bool IsPushDirection(Vector3 direction)
    {
        RaycastHit hit;
        //Debug.DrawRay(transform.position, direction, Color.red);
        //Debug.Log("checking is push direction: " + direction);
        if(Mathf.Approximately(direction.x, 0))
        {
            
            if ( (Physics.Raycast(transform.position, direction, out hit) && hit.transform.CompareTag("Player"))
            || (Physics.Raycast(transform.position + transform.right * 0.33f, direction, out hit) && hit.transform.CompareTag("Player"))
            || (Physics.Raycast(transform.position + transform.right * -0.33f, direction, out hit) && hit.transform.CompareTag("Player"))
            )
            {
                if (IsBlocked(-direction)) { return false; }
                //Debug.Log(hit.transform.parent.name + " is player");
                return true;
            }
            return false;

        }
        else
        {

            if ( (Physics.Raycast(transform.position, direction, out hit) && hit.transform.CompareTag("Player"))
                || (Physics.Raycast(transform.position + transform.forward * 0.33f, direction, out hit) && hit.transform.CompareTag("Player"))
                || (Physics.Raycast(transform.position + transform.forward * -0.33f, direction, out hit) && hit.transform.CompareTag("Player"))
                )
            {
                if (IsBlocked(-direction)) { return false; }
                return true;
            }
            return false;
        }
        //if (Physics.Raycast(transform.position, direction, out hit))
        //{
        //    if (!hit.transform.CompareTag("Player")) { return false; }
        //    if (IsBlocked(-direction)) { return false; }
        //    return true;
        //}
        //return false;
    }

    public bool IsBlocked(Vector3 direction)
    {
        //Debug.Log("is blocked");
        Debug.DrawRay(transform.position, direction * 1.2f, Color.red);
        
        if (Physics.Raycast(transform.position, direction, 1.2f, BlockLayers, QueryTriggerInteraction.Collide)) 
        { 
            foreach(RaycastHit hit in Physics.RaycastAll(transform.position, direction, 1.2f, BlockLayers, QueryTriggerInteraction.Collide))
            {
                Debug.Log("hits: " + hit.transform.name);
                if(hit.transform.gameObject.layer == 2) { return false; }
                if (hit.transform.gameObject.GetComponentInParent<BlockContainer>() == currentContainer) { return false; }
            }
            //return false;
            Debug.Log(name + " is blocked...");
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
