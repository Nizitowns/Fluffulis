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
    private Transform cameraFocusPoint;
    private Vector3 currentTarget;
    public Vector2 rawMove { get; private set; }
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        cameraFocusPoint = transform.parent.parent.Find("CameraFocusPoint");
        model = GameObject.Find("CharacterModel").transform;
       
    }
    private void Start()
    {
        cameraTransform = Camera.main.transform;
        gravityVector = Vector3.down * gravity;
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
        if(Mathf.Approximately(direction.x, 0) && Mathf.Approximately(direction.z, 0)) {  }
        else { currentTarget = direction; }
        model.LookAt(model.position + currentTarget);
        //Debug.DrawLine(model.position, model.position + currentTarget * 5);

    }
    private Vector3 GetDirection()
    {
        Vector3 camToChar = (cameraFocusPoint.position) - (cameraTransform.position); 
        Vector3 yDirection = (new Vector3(camToChar.x, 0, camToChar.z)).normalized;
        Vector3 xDirection = (Quaternion.Euler(new Vector3(0,90,0)) * yDirection).normalized;
        Debug.DrawRay(cameraFocusPoint.position, yDirection * rawMove.y * 5, Color.cyan);
        Debug.DrawRay(cameraFocusPoint.position, xDirection * rawMove.x * 5, Color.green);
        return xDirection * rawMove.x + yDirection * rawMove.y;
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
