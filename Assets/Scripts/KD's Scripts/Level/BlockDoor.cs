using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A door that opens when activated. It is closed by default and cannot be closed once opened.
/// </summary>
public class BlockDoor : Trigger
{
    List<UnitBlock> blocks;
    
    /// <summary>
    /// Gets a list of blocks that consist of the entire door. 
    /// </summary>
    private void Start()
    {
        blocks = GetComponentInChildren<BlockContainer>().blocks;
    }

    /// <summary>
    /// Opens the door.
    /// </summary>
    public override void Activate()
    {
        Debug.Log("Activate BlockDoor!");
        StartCoroutine(DelayDestroy());
    }

    /// <summary>
    /// Does nothing. The door cannot be closed once opened.
    /// </summary>
    public override void DeActivate()
    {
        
    }

    /// <summary>
    /// Sets each block that makes up the door inactive, allowing entry.
    /// </summary>
    /// <returns></returns>
    public IEnumerator DelayDestroy()
    {
        for(int i=blocks.Count-1; i >= 0; i--)
        {
            yield return new WaitForSeconds(0.1f);
            blocks[i].gameObject.SetActive(false);
        }

    }
}
