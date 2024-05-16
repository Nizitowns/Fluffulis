using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleGravity : MonoBehaviour
{
    GridObject gridObject;
    [SerializeField] LayerMask Interactable = 1 << 6;
    [SerializeField] LayerMask Ground = 1 << 3;
    SphereCollider sphere;
    private void Start()
    {
        gridObject = transform.parent.GetComponentInChildren<GridObject>();
        sphere = GetComponent<SphereCollider>();
        HandleStartFall();
        
    }
    private void OnTriggerExit(Collider other)
    {
        //Debug.Log(other.gameObject.name + " was exited from " + gridObject.transform.parent.name);
        //if (((Interactable & (1 << other.gameObject.layer)) != 0) || ((Ground & (1 << other.gameObject.layer)) != 0))
        //{
           
        //}
         HandleFall();
       
    }

    private void Update()
    {
        Debug.DrawRay(gridObject.transform.position, Vector3.down, Color.green);
    }

    public void HandleFall()
    {
        //sphere.enabled = false;
        RaycastHit hit;
        if (Physics.Raycast(gridObject.transform.position, Vector3.down, out hit))
        {
            //Debug.Log(gridObject.transform.parent.name + "'s raycast hit below");
            //Debug.Log(gridObject.transform.parent.name + "'s distance between hit: " + Vector3.Distance(gridObject.transform.position, hit.point));
            if (Vector3.Magnitude(gridObject.transform.position - hit.point) > .2f)
            {
                Debug.Log(gridObject.transform.parent.name + "'s distance between hit: " + Vector3.Distance(gridObject.transform.position, hit.point) + " hit: " + hit.transform.gameObject.name);
                gridObject.enableGravity = true;
                if (Vector3.Magnitude(gridObject.transform.position - hit.point) < 1) { gridObject.gravity = 1; }
                else { gridObject.gravity = 1 / Vector3.Magnitude(gridObject.transform.position - hit.point); }
                gridObject.StartGravity(hit.point);
            }
        }
        //sphere.enabled = true;
    }
    public void HandleStartFall()
    {
        //sphere.enabled = false;
        RaycastHit hit;
        if (Physics.Raycast(gridObject.transform.position, Vector3.down, out hit))
        {
            //Debug.Log(gridObject.transform.parent.name + "'s raycast hit below");
            //Debug.Log(gridObject.transform.parent.name + "'s distance between hit: " + Vector3.Distance(gridObject.transform.position, hit.point));
            if (Vector3.Magnitude(gridObject.transform.position - hit.point) > .2f)
            {
                Debug.Log(gridObject.transform.parent.name + "'s distance between hit: " + Vector3.Distance(gridObject.transform.position, hit.point));
                gridObject.enableGravity = true;
                if (Vector3.Magnitude(gridObject.transform.position - hit.point) < 1) { gridObject.gravity = 1; }
                else { gridObject.gravity = 1 / Vector3.Magnitude(gridObject.transform.position - hit.point); }
                gridObject.StartGravity(hit.point);
            }
        }
        //sphere.enabled = true;
    }
}
