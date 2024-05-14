using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class Character : MonoBehaviour
{
    CharacterController controller;
    Transform model;
    Vector3 gravityVector;
    [SerializeField] Transform cameraAxis;
    [SerializeField] public float baseSpeed = 10f;
    [SerializeField] public float gravity = 9.81f;

    private Transform cameraTransform;
    private Vector3 currentLook;
    private Vector3 currentTarget;
    public Vector2 rawMove { get; private set; }
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        model = GameObject.Find("CharacterModel").transform;
       
    }
    private void Start()
    {
        cameraTransform = Camera.main.transform;
        gravityVector = Vector3.down * gravity;
        currentLook = transform.forward;
        currentTarget = model.forward;
    }    
    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 direction = GetDirection();
        Vector3 velocity = (baseSpeed * direction + gravityVector) * 0.001f;
        controller.Move(velocity);
        model.position = transform.position;
        FaceDirection(direction);
    }

    private void FaceDirection(Vector3 direction)
    {
        if(direction.x == 0 && direction.z == 0) {  }
        else { currentTarget = direction; }
        //float angle = Vector3.Angle(currentLook, currentTarget);
        ////Debug.Log("angle: " + angle);
        //currentLook = Vector3.Lerp(currentLook, currentTarget, Time.deltaTime);
        //model.LookAt(model.position + currentLook);
        model.LookAt(model.position + currentTarget);
        //model.LookAt(Vector3.Lerp(model.localRotation * model.forward + transform.position, target, Time.deltaTime));
        Debug.DrawRay(model.position, currentLook * 10f, Color.red);
        Debug.DrawRay(model.position, currentTarget * 5f, Color.blue);
    }
    private Vector3 GetDirection()
    {

        return cameraTransform.right * rawMove.x + cameraTransform.forward * rawMove.y;
    }
    private void OnEnable()
    {
        if(TryGetComponent(out PlayerInput playerInput))
        {
            InputAction moveAction = playerInput.actions["Move"];
            if(moveAction != null) 
            {
                moveAction.performed += ctx => SetMove(ctx);
                moveAction.canceled += ctx => SetMove(ctx);
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
                moveAction.performed -= ctx => SetMove(ctx);
                moveAction.canceled -= ctx => SetMove(ctx);
            }
        }
    }
    private void SetMove(CallbackContext ctx)
    {
        rawMove = ctx.ReadValue<Vector2>();
    }
}
