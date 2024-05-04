using UnityEngine;

public class BlockTrigger : MonoBehaviour
{
    private BlockManager manager;

    void Start()
    {
        // Find and assign the manager in the scene
        manager = FindObjectOfType<BlockManager>();
    }

    void Update()
    {
        // Trigger check when the block is clicked or interacted with
        manager.CheckAndDestroyBlocks(gameObject);
        Debug.Log("checking");
    }
}
