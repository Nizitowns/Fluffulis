using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
    
public class CameraBehavior : MonoBehaviour
{
    public delegate void BeginPosRotate();
    public static BeginPosRotate beginPosRotate;
    public delegate void BeginNegRotate();
    public static BeginNegRotate beginNegRotate;
    public int rotationDirection { get; private set; }

    [SerializeField] public Transform cameraAxis;
    [SerializeField] public float rotateSpeed = 1f;

    private float timeElapsed = 0f;
    private float targetAngle;


    
    private void Awake()
    {
        cameraAxis = transform.parent;
        transform.parent = cameraAxis.transform;
        rotationDirection = 0;
        targetAngle = cameraAxis.eulerAngles.y;
    }
    void StartRotate(CallbackContext ctx) 
    {
        //Debug.Log("start rotate");
        rotationDirection = (int) ctx.ReadValue<float>();
        Debug.Log("rotationDir=" + rotationDirection);
        timeElapsed = 0;
        targetAngle += rotationDirection * 90;
        if(rotationDirection < 0) { beginNegRotate?.Invoke(); }
        else if(rotationDirection > 0) { beginPosRotate?.Invoke(); }
        
    }
    void Rotate()
    {
        timeElapsed += Time.deltaTime;
        cameraAxis.rotation = Quaternion.Lerp(
            cameraAxis.rotation,
            Quaternion.Euler(cameraAxis.eulerAngles.x, targetAngle, cameraAxis.eulerAngles.z),
            timeElapsed * rotateSpeed
            );

    }
    private void Update()
    {
        Rotate();
    }

    private void OnEnable()
    {
        if(GameObject.Find("Character").TryGetComponent(out PlayerInput input))
        {
            InputAction cameraRotation = input.actions["CameraRotate"];
            if(cameraRotation != null) 
            {
                cameraRotation.performed += ctx => StartRotate(ctx);
            }
        }
    }
    private void OnDisable()
    {
        if (GameObject.Find("Character").TryGetComponent(out PlayerInput input))
        {
            InputAction cameraRotation = input.actions["CameraRotate"];
            if (cameraRotation != null)
            {
                cameraRotation.performed -= ctx => StartRotate(ctx);
            }
        }
    }
}
