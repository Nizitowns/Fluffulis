using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Grid grid;
    private void Awake()
    {
        grid = GetComponent<Grid>();
    }
}
