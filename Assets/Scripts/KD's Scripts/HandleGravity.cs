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
        //if (other.gameObject.layer == Interactable)
        if((Interactable & (1 << other.gameObject.layer)) != 0)
        {
            //Debug.Log("exiter is Interactable");
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit))
            {
                //Debug.Log("raycast hit below");
                if (Vector3.Magnitude(transform.position - hit.point) > 0.6f)
                {
                    //Debug.Log("distance between hit and position > 1");
                    gridObject.enableGravity = true;
                    gridObject.StartGravity(hit.point);
                }
            }
        }
    }
}
