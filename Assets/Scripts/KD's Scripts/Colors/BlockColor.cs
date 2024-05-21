using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockColor", menuName = "ScriptableObjects/BlockColor", order = 1)]
public class BlockColor : ScriptableObject
{
    [SerializeField] public int ID;
}
