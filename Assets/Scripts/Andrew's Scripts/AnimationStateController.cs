using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;


public class AnimationStateController : MonoBehaviour
{

    public Animator animator;


    // Start is called before the first frame update
    void Start()
    {
    
        Debug.Log(animator);
       
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void stoping(CallbackContext ctx)
    {
        animator.SetBool("isWalking", false);
        //Debug.Log("stoping");
    }

    private void walking(CallbackContext ctx)
    {
        animator.SetBool("isWalking", true);
        //Debug.Log("walking");
    }

    private void OnEnable()
    {
        if (TryGetComponent(out PlayerInput playerInput))
        {
            InputAction moveAction = playerInput.actions["Move"];
            if (moveAction != null)
            {
                moveAction.performed += walking;
                moveAction.canceled += stoping;
            }
       
        }
    }
    private void OnDisable()
    {
        
        if (TryGetComponent(out PlayerInput playerInput))
        {
            InputAction moveAction = playerInput.actions["Move"];
            if (moveAction != null)
            {
                moveAction.performed -= walking;
                moveAction.canceled -= stoping;
            }

            
        }
    }


}
