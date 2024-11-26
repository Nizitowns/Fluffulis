using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A ScriptableObject with an ID property used to distinguish between different types of blocks. 
/// </summary>
[CreateAssetMenu(fileName = "BlockColor", menuName = "ScriptableObjects/BlockColor", order = 1)]
public class BlockColor : ScriptableObject
{
    [SerializeField] public int ID;
}
