using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class Character : MonoBehaviour
{

    public static Character Instance;

    CharacterController controller;
    Transform model;
    public Vector3 gravityVector;
    public Vector3 yVelocity;
    [SerializeField] Transform cameraAxis;
    [SerializeField] public float baseSpeed = 10f;
    [SerializeField] public float gravity = 9.81f;
    [SerializeField] int[] soundToPlay;

    private Transform cameraTransform;
    private Transform cameraFocusPoint;
    private Vector3 currentTarget;
    public Vector2 rawMove { get; private set; }

    /// <summary>
    /// Jump Variables
    /// </summary>
    public float jumpHeight = 1.2f;
    public float jumpSpeed = 1f;
    public float jumpDuration = 1f;
    bool startedJump = false;
    private float startJumpY;
    private float timeElapsed;
    private GroundCheck groundCheck;
    public bool enableJump = true;
    private void Awake()
    {
        Instance = this;
        controller = GetComponent<CharacterController>();
        cameraFocusPoint = GameObject.Find("CameraFocusPoint").transform;
        model = GameObject.Find("CharacterModel").transform;
        groundCheck = GetComponentInChildren<GroundCheck>();
    }
    private void Start()
    {
        GameManager.Instance.respawnPosition = transform.position;
        cameraTransform = Camera.main.transform;
        gravityVector = Vector3.down * gravity;
        currentTarget = model.forward;
        yVelocity = gravityVector;
        PlaySound();
    }
    private void Update()
    {
        Jumping();
        Move();
    }
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

    private void StartJump(CallbackContext ctx)
    {
        if (!enableJump) { return; }
        if (!groundCheck.Grounded() || startedJump) { return; }
        startedJump = true;
        startJumpY = transform.position.y;
        timeElapsed = 0;
    }
    private void Jumping()
    {
        if (!enableJump) { return; }
        if (!startedJump) { return; }
        timeElapsed += Time.smoothDeltaTime;
        timeElapsed = Mathf.MoveTowards(timeElapsed, jumpDuration, Time.smoothDeltaTime);
        //currJumpY = Mathf.Lerp(currJumpY, startJumpY + jumpHeight, timeElapsed * jumpSpeed);
        yVelocity = Vector3.up * jumpSpeed;
        if (transform.position.y >= startJumpY + jumpHeight)
        {
            if (timeElapsed < jumpDuration) { yVelocity = Vector3.zero; }
            else
            {
                //Debug.Log("timeElapsed: " + timeElapsed);
                yVelocity = gravityVector;
                startedJump = false;
            }

        }
    }
    private void FaceDirection(Vector3 direction)
    {
        if (Mathf.Approximately(direction.x, 0) && Mathf.Approximately(direction.z, 0)) { }
        else { currentTarget = direction; }
        model.LookAt(model.position + currentTarget);
    }
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
    private void SetMove(CallbackContext ctx)
    {
        rawMove = ctx.ReadValue<Vector2>();
    }
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

    private void PlaySound()
    {
        int i = Random.Range(0, soundToPlay.Length - 1);
        AudioManager.Instance.PlaySFX(soundToPlay[i]);
    }
}
