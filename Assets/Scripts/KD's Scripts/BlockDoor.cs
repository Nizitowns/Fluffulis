using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDoor : Trigger
{
    List<UnitBlock> blocks;
    private void Awake()
    {

    }
    private void Start()
    {
        blocks = GetComponentInChildren<BlockContainer>().blocks;
    }
    public override void Activate()
    {
        Debug.Log("Activate BlockDoor!");
        StartCoroutine(DelayDestroy());
    }

    public override void DeActivate()
    {
        
    }

    public IEnumerator DelayDestroy()
    {
        for(int i=blocks.Count-1; i >= 0; i--)
        {
            yield return new WaitForSeconds(0.1f);
            blocks[i].gameObject.SetActive(false);
            Debug.Log("destroy " + blocks[i].name);
        }

    }
}
