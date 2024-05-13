using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float climbSpeed = 3f;
  //  public float jumpForce = 5f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;
    public LayerMask interactableLayer;
    public LayerMask ladderLayer;
    public Camera mainCamera; // Reference to the main camera

    private Rigidbody rb;
    private bool isGrounded;
    private bool isClimbing;
    private Vector3 movement;

    private enum State { Idle, Walking, Jumping, Interacting, Climbing }
    private State currentState;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentState = State.Idle;
    }

    void Update()
    {
        CheckGroundStatus();
        ProcessInputs();

        switch (currentState)
        {
            case State.Idle:
                HandleIdleState();
                break;
            case State.Walking:
                HandleWalkingState();
                break;
            //case State.Jumping:
            //    HandleJumpingState();
            //    break;
            case State.Interacting:
                HandleInteractingState();
                break;
            case State.Climbing:
                HandleClimbingState();
                break;
        }
    }

    private void ProcessInputs()
    {
        // Calculate the direction relative to the camera's rotation
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // Use camera relative directions to move the player
        movement = (forward * Input.GetAxis("Vertical") + right * Input.GetAxis("Horizontal")).normalized;

        if (movement.magnitude > 0.1f)
        {
            if (currentState != State.Climbing)
                currentState = State.Walking;
        }
        else if (currentState != State.Climbing)
        {
            currentState = State.Idle;
        }

        //if (Input.GetKeyDown(KeyCode.Space) && isGrounded && currentState != State.Climbing)
        //{
        //    currentState = State.Jumping;
        //}

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (currentState == State.Climbing)
            {
                currentState = State.Idle; // Allow dismounting the ladder with the same interact key
            }
            else
            {
                currentState = State.Interacting;
            }
        }
    }

    private void CheckGroundStatus()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void HandleIdleState() 
    {
        rb.useGravity = true; // enable gravity while not climbing
                              //
     }

        private void HandleWalkingState()
    {
        // Rotate the player to face the movement direction
        Quaternion targetRotation = Quaternion.LookRotation(movement);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10);
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
    }

    //private void HandleJumpingState()
    //{
    //    if (isGrounded)
    //    {
    //        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    //        currentState = State.Idle;
    //    }
    //}

    private void HandleInteractingState()
    {
        // Detect and interact with ladder
        Collider[] ladders = Physics.OverlapSphere(transform.position, 1.0f, ladderLayer);
        if (ladders.Length > 0)
        {
            currentState = State.Climbing; // Transition to climbing state
            rb.useGravity = false; // Disable gravity while climbing
        }
        else
        {
            currentState = State.Idle; // No ladder found, return to idle
            rb.useGravity = true; // enable gravity while not climbing
        }   
    }

    private void HandleClimbingState()
    {
        rb.velocity = new Vector3(0, Input.GetAxis("Vertical") * climbSpeed, 0);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    public static implicit operator PlayerController(Player v)
    {
        throw new NotImplementedException();
    }
}
