using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    // Start is called before the first frame update
    Grid grid;
    [SerializeField] public bool snapToGridOnStart = true;
    private void Awake()
    {
        grid = GetComponentInParent<Grid>();
    }
    void Start()
    {
        if(snapToGridOnStart) { transform.position = grid.WorldToCell(transform.position); }
        
    }
}
