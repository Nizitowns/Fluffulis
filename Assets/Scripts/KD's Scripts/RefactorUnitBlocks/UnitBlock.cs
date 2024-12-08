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
    public void Push() { currentContainer.ReceivePush(this); }
    public Vector3 GetPushDirection()
    {
        Vector3[] directions = { Vector3.forward, -Vector3.forward, Vector3.right, -Vector3.right };
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
        RaycastHit[][] hits;
        if (Mathf.Approximately(direction.x, 0))
        {
            Debug.DrawRay(transform.position, direction, Color.red, 3f);
            Debug.DrawRay(transform.position + Vector3.right * 0.33f, direction, Color.red, 3f);
            Debug.DrawRay(transform.position - Vector3.right * 0.33f, direction, Color.red, 3f);
            hits = new RaycastHit[3][] {
                Physics.RaycastAll(transform.position + Vector3.right * 0.33f, direction),
                Physics.RaycastAll(transform.position + Vector3.right * -0.33f, direction),
                Physics.RaycastAll(transform.position, direction),
                };
        }
        else {
            Debug.DrawRay(transform.position, direction, Color.green, 3f);
            Debug.DrawRay(transform.position + Vector3.forward * 0.33f, direction, Color.green, 3f);
            Debug.DrawRay(transform.position - Vector3.forward * 0.33f, direction, Color.green, 3f);
            hits = new RaycastHit[3][] {
                Physics.RaycastAll(transform.position + Vector3.forward * 0.33f, direction),
                Physics.RaycastAll(transform.position + Vector3.forward * -0.33f, direction),
                Physics.RaycastAll(transform.position, direction),
                };
        }
        for (int i=0; i<3; i++)
        {
            for(int j=0; j<hits[i].Length; j++)
            {
                if (hits[i][j].transform.CompareTag("Player")) 
                {
                    return true; 
                }
                
            }
        }
        return false;
    }
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
            return true;
        }
        return false;
    }
    public void Snap(Vector3 target)
    {
        transform.position = grid.WorldToCell(target);
        meshRenderer.transform.position = grid.WorldToCell(target);
        blockCollider.transform.position = grid.WorldToCell(target);
        groundCheck.transform.position = grid.WorldToCell(target) + Vector3.down * 0.5f;
    }
}
