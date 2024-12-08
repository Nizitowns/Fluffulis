using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBlock : MonoBehaviour
{
    public Grid grid;
    public BlockContainer currentContainer;
    private BoxCollider blockCollider;
    private MeshRenderer meshRenderer;
    private Transform groundCheck;

    /// <summary>
    /// Initializes properties by finding necessary components.
    /// </summary>
    private void Awake()
    {
        currentContainer = transform.GetComponentInParent<BlockContainer>();
        grid = GameObject.Find("GridManager").GetComponent<Grid>();
        blockCollider = GetComponentInChildren<BoxCollider>();
        groundCheck = GetComponentInChildren<Rigidbody>().transform;
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        Snap(transform.position);

        
    }
    /// <summary>
    /// Handles push behavior for a BlockContainer, consisting of one or  more UnitBlocks.
    /// </summary>
    public void Push() { currentContainer.ReceivePush(this); }
    /// <summary>
    /// Determines the direction the UnitBlock is being pushed towards.
    /// </summary>
    /// <returns> Returns the direction the UnitBlock is being pushed towards. </returns>
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
    /// <summary>
    /// Verifies push direction by checking whether a raycast in direction hits the player.
    /// </summary>
    /// <param name="direction"> The direction to send the raycast, which checks for player hit. </param>
    /// <returns> True if player is hit, false otherwise. </returns>
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
    /// <summary>
    /// Checks if the block is being blocked in the position where the block should move after the push.
    /// </summary>
    /// <param name="direction"></param>
    /// <returns> True if the block is blocked, false otherwise. </returns>
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
    /// <summary>
    /// Snaps the block to a grid cell.
    /// </summary>
    /// <param name="target"> The position of the block. </param>
    public void Snap(Vector3 target)
    {
        transform.position = grid.WorldToCell(target);
        meshRenderer.transform.position = grid.WorldToCell(target);
        blockCollider.transform.position = grid.WorldToCell(target);
        groundCheck.transform.position = grid.WorldToCell(target) + Vector3.down * 0.5f;
    }
}
