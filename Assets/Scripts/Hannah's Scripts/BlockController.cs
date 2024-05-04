using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public string blockTag = "PinkBlock"; // Tag assigned to each block
    public int groupSizeToDisappear = 3;  // Minimum number of adjacent blocks required
    private HashSet<GameObject> visitedBlocks = new HashSet<GameObject>();
    public float detectionRadius = 1.0f;
    public Color gizmoColor = Color.red;
    public void CheckAndDestroyBlocks(GameObject startBlock)
    {
        visitedBlocks.Clear(); // Clear visited blocks for a new check
        List<GameObject> connectedBlocks = FindConnectedBlocks(startBlock);
        Debug.Log("checking2" + connectedBlocks.Count);
        if (connectedBlocks.Count >= groupSizeToDisappear)
        {
            
            // Destroy all connected blocks
            foreach (var block in connectedBlocks)
            {
                Debug.Log("destroyingblock");
                Destroy(block);
            }
        }
    }

    private List<GameObject> FindConnectedBlocks(GameObject startBlock)
    {
        List<GameObject> connected = new List<GameObject>();
        Queue<GameObject> queue = new Queue<GameObject>();

        queue.Enqueue(startBlock);
        visitedBlocks.Add(startBlock);

        while (queue.Count > 0)
        {
        
            GameObject current = queue.Dequeue();
            connected.Add(current);

            // Get adjacent blocks by checking all six directions
            Vector3[] directions = {
                Vector3.forward, Vector3.back, Vector3.left,
                Vector3.right //Vector3.up, Vector3.down
            };

            foreach (var direction in directions)
            {
                Vector3 neighborPos = current.transform.position + direction;
                Collider[] neighbors = Physics.OverlapSphere(neighborPos, 0.9f);

                foreach (var neighbor in neighbors)
                {
                    
                    if (neighbor.CompareTag(blockTag) && !visitedBlocks.Contains(neighbor.gameObject) )
                    {
                         
                        queue.Enqueue(neighbor.gameObject);
                        visitedBlocks.Add(neighbor.gameObject);
                    }
                }
            }
        }

        return connected;
    }


    // This will draw the gizmo sphere when selected in the scene view
    void OnDrawGizmos()
    {
        // Set the gizmo color
        Gizmos.color = gizmoColor;

        // Draw a wireframe sphere at the position of the block
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    // This will draw the gizmo sphere only when selected in the inspector
    void OnDrawGizmosSelected()
    {
        // Optional: Draw a more prominent sphere when selected
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    void Update()
    {
        // Example of using Physics.OverlapSphere
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider hitCollider in hitColliders)
        {
            Debug.Log("Detected object: " + hitCollider.name);
        }
    }
}
