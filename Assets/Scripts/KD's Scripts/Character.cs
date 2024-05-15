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
        // direction and apply speed
        Vector3 direction = GetDirection();
        Vector3 velocity = (baseSpeed * direction + gravityVector);
        // move
        controller.Move(velocity * Time.deltaTime);
        // move character model
        model.position = transform.position;
        // face in moving direction
        FaceDirection(direction);
    }

    private void FaceDirection(Vector3 direction)
    {
        if(Mathf.Approximately(direction.x, 0) && Mathf.Approximately(direction.z, 0)) {  }
        else { currentTarget = direction; }
        model.LookAt(model.position + currentTarget);
    }
    private Vector3 GetDirection()
    {
        // get vector from camera to focus point
        Vector3 camToPoint = (cameraFocusPoint.position) - (cameraTransform.position); 
        // extract x and z values from vector
        Vector3 yDirection = (new Vector3(camToPoint.x, 0, camToPoint.z)).normalized;
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
