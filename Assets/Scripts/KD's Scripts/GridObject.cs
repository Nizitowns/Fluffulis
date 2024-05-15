using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    // Start is called before the first frame update
    Grid grid;
    [SerializeField] public bool snapToGridOnStart = true;

    [SerializeField] public bool isPushable = true;
    private bool pushing = false;
    private Vector3 pushTarget;
    private float timeElapsed = 0;
    private float pushSpeed = 1f;
    private void Awake()
    {
        grid = transform.parent.GetComponentInChildren<Grid>();
    }
    void Start()
    {
        if(snapToGridOnStart) { transform.position = grid.WorldToCell(transform.position); }
        
    }
    private void Update()
    {
        Sliding();
    }
    public void CheckPush() 
    {
        if(pushing) { return; }
        RaycastHit forwardHit;
        RaycastHit backwardHit;
        RaycastHit rightHit;
        RaycastHit leftHit;

        if(Physics.Raycast(transform.position, transform.forward, out forwardHit)) 
        {
            //Debug.Log("forwardHit");
            //Debug.DrawRay(transform.position, transform.forward, Color.green);
            Push(-transform.forward);
            pushing = true;
        }
        if (Physics.Raycast(transform.position, -transform.forward, out backwardHit)) 
        {
            //Debug.Log("backwardHit");
            //Debug.DrawRay(transform.position, -transform.forward, Color.cyan);
            Push(transform.forward);
            pushing = true;
        }
        if (Physics.Raycast(transform.position, transform.right, out rightHit)) 
        {
            //Debug.Log("rightHit");
            //Debug.DrawRay(transform.position, transform.right, Color.red);
            Push(-transform.right);
            pushing = true;
        }
        if (Physics.Raycast(transform.position, -transform.right, out leftHit)) 
        {
            //Debug.Log("leftHit");
            //Debug.DrawRay(transform.position, -transform.right, Color.blue);
            Push(transform.right);
            pushing = true;
        }
    }
    private void Push(Vector3 direction) 
    {
        Vector3 newCellPosition = grid.WorldToCell(transform.position + direction);
        //transform.position = newCellPosition;

        pushTarget = newCellPosition;
        timeElapsed = 0;
        Sliding();
        StartCoroutine(DelayNewPush());
    }
    private void Sliding() 
    {
        if(!pushing) { return; }
        timeElapsed += Time.smoothDeltaTime;
        transform.position = Vector3.Lerp(transform.position, pushTarget, timeElapsed * pushSpeed);
    }
    IEnumerator DelayNewPush() 
    {
        yield return new WaitForSeconds(0.3f);
        pushing = false;
    }
    private void isBlocked() { }
}
