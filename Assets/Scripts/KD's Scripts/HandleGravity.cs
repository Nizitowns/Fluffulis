using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleGravity : MonoBehaviour
{
    GridObject gridObject;
    [SerializeField] LayerMask Interactable = 1 << 6;
    [SerializeField] LayerMask Ground = 1 << 3;
    private void Start()
    {
        gridObject = transform.parent.GetComponentInChildren<GridObject>();
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.gameObject.name + " was exited from " + transform.parent.name);
        if (other.gameObject.layer == Interactable || other.gameObject.layer == Ground)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit))
            {

                if (Vector3.Magnitude(transform.position - hit.point) > 1f)
                {
                    gridObject.enableGravity = true;
                    gridObject.StartGravity(hit.point);
                }
            }
        }
    }
    //private void OnTriggerExit(Collision collision)
    //{
    //    Debug.Log(name + " was exited");
    //    if (collision.gameObject.layer == Interactable || collision.gameObject.layer == Ground)
    //    {
    //        RaycastHit hit;
    //        if (Physics.Raycast(transform.position, Vector3.down, out hit))
    //        {

    //            if (Vector3.Magnitude(transform.position - hit.point) > 1f)
    //            {
    //                gridObject.enableGravity = true;
    //                gridObject.StartGravity(hit.point);
    //            }
    //        }
    //    }
    //}
}
