using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [Tooltip("Color for comparison when checking correct block. ColorAny by default.")]
    [SerializeField] public BlockColor color;
    [Tooltip("Button manager that manages button.")]
    [SerializeField] public ButtonManager buttonManager;
    [SerializeField] LayerMask Interactable = 1 << 6;
    [Tooltip("Don't change...")]
    [SerializeField] public BlockColor colorAny;

    //private void Awake()
    //{
    //    colorAny = Resources.Load<BlockColor>("BlockColor/ColorAny");
    //}
    private void Start()
    {
        //if (color == null) { color = Resources.Load<BlockColor>("ColorAny"); }
        if (buttonManager == null) { GameObject.Find("ButtonManager").TryGetComponent(out buttonManager); }
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((Interactable & (1 << other.gameObject.layer)) != 0)
        {
            BlockContainer bC = other.GetComponentInParent<BlockContainer>();
            if (bC != null) {Debug.Log("anyColor: " + (colorAny==null) + " color: " + (color==null) + " bC color:" +(bC.color==null)); if (bC.color.ID != color.ID && color.ID !=colorAny.ID) { return; } }
            //Debug.Log(other.name + " has hit button");
            buttonManager.buttonActivated?.Invoke();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if ((Interactable & (1 << other.gameObject.layer)) != 0)
        {
            BlockContainer bC = other.GetComponentInParent<BlockContainer>();
            if (bC != null) { if (bC.color.ID != color.ID && color.ID !=colorAny.ID) { return; } }
            Debug.Log(other.name + " exits button");
            buttonManager.buttonDeactivated?.Invoke();
        }
    }
}
