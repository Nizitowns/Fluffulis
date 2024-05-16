using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleGravity : MonoBehaviour
{
    GridObject gridObject;
    //[SerializeField] LayerMask Interactable = 1 << 6;
    //[SerializeField] LayerMask Ground = 1 << 3;
    private void Start()
    {
        gridObject = transform.parent.GetComponentInChildren<GridObject>();
        HandleStartFall();
    }
    private void OnTriggerExit(Collider other)
    {
        //Debug.Log(other.gameObject.name + " was exited from " + transform.parent.name);
        HandleFall();
    }

    public void HandleFall()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            //Debug.Log(transform.parent.name + "'s raycast hit below");
            if (Vector3.Magnitude(transform.position - hit.point) > .9f)
            {
                //Debug.Log(transform.parent.name + "'s distance between hit: " + Vector3.Distance(transform.position, hit.point));
                gridObject.enableGravity = true;
                gridObject.gravity = 1 / Vector3.Magnitude(transform.position - hit.point);
                gridObject.StartGravity(hit.point);
            }
        }
    }
    public void HandleStartFall()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            //Debug.Log(transform.parent.name + "'s raycast hit below");
            if (Vector3.Magnitude(transform.position - hit.point) > 2)
            {
                //Debug.Log(transform.parent.name + "'s distance between hit: " + Vector3.Distance(transform.position, hit.point));
                gridObject.enableGravity = true;
                gridObject.gravity = 1 / Vector3.Magnitude(transform.position - hit.point);
                gridObject.StartGravity(hit.point);
            }
        }
    }
}
