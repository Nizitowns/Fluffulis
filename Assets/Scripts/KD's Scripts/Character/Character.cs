using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

/// <summary>
/// Handles basic character controls, including movement, look, and jump in relation to the camera.
/// </summary>
public class Character : MonoBehaviour
{
    public static Character Instance;

    private CharacterController controller;
    private Transform model;
    public Vector3 gravityVector;
    public Vector3 yVelocity;
    [SerializeField] private Transform cameraAxis;
    [SerializeField] public float baseSpeed = 10f;
    [SerializeField] public float gravity = 9.81f;
    [SerializeField] private int[] soundToPlay;

    private Transform cameraTransform;
    private Transform cameraFocusPoint;
    private Vector3 currentTarget;
    public Vector2 rawMove { get; private set; }

    // Jump Variables
    public float jumpHeight = 1.2f;
    public float jumpSpeed = 1f;
    public float jumpDuration = 1f;
    private bool startedJump = false;
    private float startJumpY;
    private float timeElapsed;
    private GroundCheck groundCheck;
    public bool enableJump = true;

    /// <summary>
    /// Finds necessary components and sets them to properties.
    /// Sets this component as a referencable Instance in the scene.
    /// </summary>
    private void Awake()
    {
        Instance = this;
        controller = GetComponent<CharacterController>();
        cameraFocusPoint = GameObject.Find("CameraFocusPoint").transform;
        model = GameObject.Find("CharacterModel").transform;
        groundCheck = GetComponentInChildren<GroundCheck>();
    }
    /// <summary>
    /// Sets property values to default, plays sound at start.
    /// </summary>
    private void Start()
    {
        GameManager.Instance.respawnPosition = transform.position;
        cameraTransform = Camera.main.transform;
        gravityVector = Vector3.down * gravity;
        currentTarget = model.forward;
        yVelocity = gravityVector;
        PlaySound();
    }
    /// <summary>
    /// Handles jump and movement behavior every frame.
    /// </summary>
    private void Update()
    {
        Jumping();
        Move();
    }
    /// <summary>
    /// Gets player to move and look in direction GetDirection(),
    /// at velocity rate (includes constant gravity).
    /// </summary>
    private void Move()
    {
        // direction and apply speed
        Vector3 direction = GetDirection();
        Vector3 velocity = (baseSpeed * direction + yVelocity);
        // move
        controller.Move(velocity * Time.deltaTime);
        // move character model
        model.position = transform.position;
        // face in moving direction
        FaceDirection(direction);
    }

    /// <summary>
    /// If jump is enabled, the player is grounded, and has not started jumping, begin jumping.
    /// </summary>
    /// <param name="ctx"> Necessary parameter for subscribing to inputAction events. </param>
    private void StartJump(CallbackContext ctx)
    {
        if (!enableJump) { return; }
        if (!groundCheck.Grounded() || startedJump) { return; }
        startedJump = true;
        startJumpY = transform.position.y;
        timeElapsed = 0;
    }

    /// <summary>
    /// Moves towards jump height at a constant rate. When reaching jump height, begin falling 
    /// at a constant rate.
    /// </summary>
    private void Jumping()
    {
        if (!enableJump) { return; }
        if (!startedJump) { return; }
        timeElapsed += Time.smoothDeltaTime;
        timeElapsed = Mathf.MoveTowards(timeElapsed, jumpDuration, Time.smoothDeltaTime);
        yVelocity = Vector3.up * jumpSpeed;
        if (transform.position.y >= startJumpY + jumpHeight)
        {
            if (timeElapsed < jumpDuration) { yVelocity = Vector3.zero; }
            else
            {
                yVelocity = gravityVector;
                startedJump = false;
            }

        }
    }
    /// <summary>
    /// Looks in direction player is moving. If no movement, do nothing (looks at previous look target).
    /// </summary>
    /// <param name="direction"></param>
    private void FaceDirection(Vector3 direction)
    {
        if (Mathf.Approximately(direction.x, 0) && Mathf.Approximately(direction.z, 0)) { }
        else { currentTarget = direction; }
        model.LookAt(model.position + currentTarget);
    }
    /// <summary>
    /// Gets the direction on the horizontal plane (x,z) from player to camera and relates it
    /// to the raw input values from the rawMove Vector2.
    /// </summary>
    /// <returns> Returns the movement/look direction in relation to the direction from player to camera. </returns>
    private Vector3 GetDirection()
    {
        // get vector from camera to focus point
        Vector3 camToPoint = (cameraFocusPoint.position) - (cameraTransform.position);
        // extract x and z values from vector
        Vector3 yDirection = (new Vector3(camToPoint.x, 0, camToPoint.z)).normalized;
        Vector3 xDirection = (Quaternion.Euler(new Vector3(0, 90, 0)) * yDirection).normalized;
        Debug.DrawRay(cameraFocusPoint.position, yDirection * rawMove.y * 5, Color.cyan);
        Debug.DrawRay(cameraFocusPoint.position, xDirection * rawMove.x * 5, Color.green);
        return xDirection * rawMove.x + yDirection * rawMove.y;
    }

    /// <summary>
    /// Sets rawMove to the value of the Move action input.
    /// Other methods will reference rawMove to determine movement and direction.
    /// </summary>
    /// <param name="ctx"></param>
    private void SetMove(CallbackContext ctx)
    {
        rawMove = ctx.ReadValue<Vector2>();
    }

    /// <summary>
    /// Subscribes SetMove() to the moveAction event and StartJump() to the jumpAction event.
    /// </summary>
    private void OnEnable()
    {
        if (TryGetComponent(out PlayerInput playerInput))
        {
            InputAction moveAction = playerInput.actions["Move"];
            if (moveAction != null)
            {
                moveAction.performed += SetMove;
                moveAction.canceled += SetMove;
            }
            InputAction jumpAction = playerInput.actions["Jump"];
            if (jumpAction != null)
            {
                jumpAction.started += StartJump;
                jumpAction.canceled -= StartJump;
            }
        }
    }

    /// <summary>
    /// Unsubscribes SetMove() from the moveAction event and StartJump() from the jumpAction event.
    /// </summary>
    private void OnDisable()
    {
        rawMove = Vector2.zero;
        if (TryGetComponent(out PlayerInput playerInput))
        {
            InputAction moveAction = playerInput.actions["Move"];
            if (moveAction != null)
            {
                moveAction.performed -= SetMove;
                moveAction.canceled -= SetMove;
            }
            InputAction jumpAction = playerInput.actions["Jump"];
            if (jumpAction != null)
            {
                jumpAction.started -= StartJump;
                jumpAction.canceled -= StartJump;
            }
        }
    }

    /// <summary>
    /// Plays a sound from an array of sounds in AudioManager.
    /// </summary>
    private void PlaySound()
    {
        int i = Random.Range(0, soundToPlay.Length - 1);
        AudioManager.Instance.PlaySFX(soundToPlay[i]);
    }
}
