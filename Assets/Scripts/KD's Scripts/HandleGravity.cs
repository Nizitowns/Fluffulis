using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleGravity : MonoBehaviour
{
    GridObject gridObject;
    //[SerializeField] LayerMask Interactable = 1 << 6;
    //[SerializeField] LayerMask Ground = 1 << 3;
    //SphereCollider sphere;
    private void Start()
    {
        gridObject = transform.parent.GetComponentInChildren<GridObject>();
        HandleFall();
        
    }
    private void OnTriggerExit(Collider other)
    {
         HandleFall();
    }

    private void Update()
    {
        Debug.DrawRay(gridObject.transform.position, Vector3.down, Color.green);
    }
    /// <summary>
    /// Sends a raycast, checks the distance from object to ground
    /// </summary>
    public void HandleFall()
    {
        RaycastHit hit;
        if (Physics.Raycast(gridObject.transform.position, Vector3.down, out hit))
        {
            //Debug.Log(gridObject.transform.parent.name + "'s raycast hit below");
            //Debug.Log(gridObject.transform.parent.name + "'s distance between hit: " + Vector3.Distance(gridObject.transform.position, hit.point));
            //if (Vector3.Magnitude(gridObject.transform.position - hit.point) > .2f)
            //{
                FallTo(hit.point);
            //}
        }
        else
        {
            FallTo(transform.position + Vector3.down * 200);
        }
    }
    /// <summary>
    /// Object has gravity enabled
    /// </summary>
    /// <param name="point">point in space where object falls to</param>
    public void FallTo(Vector3 point)
    {
        //Debug.Log(gridObject.transform.parent.name + "'s distance between hit: " + Vector3.Distance(gridObject.transform.position, point));
        gridObject.enableGravity = true;
        if (Vector3.Magnitude(gridObject.transform.position - point) < 1) { gridObject.gravity = 1; }
        else { gridObject.gravity = 1 / Vector3.Magnitude(gridObject.transform.position - point); }
        gridObject.StartGravity(point);
    }
}
