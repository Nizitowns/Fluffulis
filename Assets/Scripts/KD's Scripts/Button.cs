using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] LayerMask Interactable = 1 << 6;
    private void OnTriggerEnter(Collider other)
    {
        if ((Interactable & (1 << other.gameObject.layer)) != 0)
        {
            //Debug.Log(other.name + "has hit button");
            ButtonManager.buttonActivated?.Invoke();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if ((Interactable & (1 << other.gameObject.layer)) != 0)
        {
            //Debug.Log("Interactable exits button");
            ButtonManager.buttonDeactivated?.Invoke();
        }
    }
}
