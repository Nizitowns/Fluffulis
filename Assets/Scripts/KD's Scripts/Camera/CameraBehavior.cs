using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

/// <summary>
/// Handles custom camera controls outside of CinemachineFreeLook component,
/// allowing camera rotation of 45 degrees clockwise or counter-clockwise along the y-axis depending on input.
/// </summary>
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

    /// <summary>
    /// Gets reference to the CinemachineFreeLook and CinemachineInputProvider components,
    /// and sets default values for properties.
    /// </summary>
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
    /// <summary>
    /// Sets up properties to prepare smooth 45 degree rotation based on input,
    /// while disabling the CinemachineInputProvider input (to ignore default camera control inputs).
    /// </summary>
    /// <param name="ctx"> Allows access to player input value. </param>
    void StartRotateOverrideFreeLook(CallbackContext ctx)
    {
        rotationDirection = (int) ctx.ReadValue<float>();
        timeElapsed = 0;
        vcamInput.enabled = false;
        targetAngle = vcam.m_XAxis.Value + rotationDirection * rotateAmount;
        
    }
    /// <summary>
    /// Handles smooth rotation towards targetAngle calculated from input and current angle. 
    /// </summary>
    void RotateOverrideFreeLook() 
    {
        if(vcamInput.enabled == true) { return; }
        timeElapsed += Time.smoothDeltaTime;
        // Adjust target for wrapping around (between 180, -180).
        if(targetAngle > 180 && vcam.m_XAxis.Value < 0) 
        { 
            targetAngle = -180 + (targetAngle - 180); 
        }
        else if(targetAngle < -180 && vcam.m_XAxis.Value > 0) 
        { 
            targetAngle = 180 + (targetAngle + 180); 
        }
        // Rotate to target.
        vcam.m_XAxis.Value = Mathf.Lerp(vcam.m_XAxis.Value, targetAngle, timeElapsed * rotateSpeed);

        // Enable free camera look after rotation is finished.
        if(Mathf.Approximately(targetAngle, vcam.m_XAxis.Value)) { vcamInput.enabled = true; }
    }

    /// <summary>
    /// Checks for rotation input and rotates towards the angle each frame until reaching the target.
    /// </summary>
    private void Update()
    {
        RotateOverrideFreeLook();
    }

    /// <summary>
    /// Subscribes StartRotateOverrideFreeLook to the custom cameraRotation input.
    /// </summary>
    private void OnEnable()
    {
        if(character.TryGetComponent(out PlayerInput input))
        {
            InputAction cameraRotation = input.actions["CameraRotate"];
            if(cameraRotation != null) 
            {
                cameraRotation.performed += StartRotateOverrideFreeLook;
            }
        }
    }

    /// <summary>
    /// Unsubscribes StartRotateOverrideFreeLook from the custom cameraRotation input.
    /// </summary>
    private void OnDisable()
    {
        if (character.TryGetComponent(out PlayerInput input))
        {
            InputAction cameraRotation = input.actions["CameraRotate"];
            if (cameraRotation != null)
            {
                Debug.Log("disable startrotateoverridefreelook");
                cameraRotation.performed -= StartRotateOverrideFreeLook;
            }
        }
    }
}
