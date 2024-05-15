using Cinemachine;
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

    [SerializeField] public float rotateAmount = 15;
    [SerializeField] public Transform cameraAxis;
    [SerializeField] public float rotateSpeed = 1f;

    private float timeElapsed = 0f;
    private float targetAngle;
    private CinemachineFreeLook vcam;
    private CinemachineInputProvider vcamInput;
    private GameObject character;

    private void Awake()
    {
        character = GameObject.Find("Character");
        vcam = GetComponent<CinemachineFreeLook>();
        vcamInput = GetComponent<CinemachineInputProvider>();
        cameraAxis = transform.parent;
        transform.parent = cameraAxis.transform;
        rotationDirection = 0;
        targetAngle = cameraAxis.eulerAngles.y;
    }
    void StartRotateOverrideFreeLook(CallbackContext ctx)
    {
        rotationDirection = (int) ctx.ReadValue<float>();
        timeElapsed = 0;
        vcamInput.enabled = false;
        targetAngle = vcam.m_XAxis.Value + rotationDirection * rotateAmount;
    }
    void RotateOverrideFreeLook() 
    {
        if(vcamInput.enabled == true) { return; }
        timeElapsed += Time.smoothDeltaTime;
        // Adjust target for wrapping around (btw 180, -180)
        if(targetAngle > 180 && vcam.m_XAxis.Value < 0) 
        { 
            targetAngle = -180 + (targetAngle - 180); 
        }
        else if(targetAngle < -180 && vcam.m_XAxis.Value > 0) 
        { 
            targetAngle = 180 + (targetAngle + 180); 
        }
        // rotate to target
        vcam.m_XAxis.Value = Mathf.Lerp(vcam.m_XAxis.Value, targetAngle, timeElapsed * rotateSpeed);
        // enable free camera look after rotation should be done
        if(Mathf.Approximately(targetAngle, vcam.m_XAxis.Value)) { vcamInput.enabled = true; }
    }
    private void Update()
    {
        RotateOverrideFreeLook();
    }

    private void OnEnable()
    {
        if(character.TryGetComponent(out PlayerInput input))
        {
            InputAction cameraRotation = input.actions["CameraRotate"];
            if(cameraRotation != null) 
            {
                cameraRotation.performed += ctx => StartRotateOverrideFreeLook(ctx);
            }
        }
    }
    private void OnDisable()
    {
        if (character.TryGetComponent(out PlayerInput input))
        {
            InputAction cameraRotation = input.actions["CameraRotate"];
            if (cameraRotation != null)
            {
                cameraRotation.performed -= ctx => StartRotateOverrideFreeLook(ctx);
            }
        }
    }
}
