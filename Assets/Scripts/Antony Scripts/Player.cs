using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 180f;

    private CharacterController characterController;
    private Vector3 moveDirection;
    private Camera mainCamera;

    [SerializeField] private Animator animator;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        HandleMovement();
        //UpdateAnimator();
    }

    private void HandleMovement()
    {
        // Calculate movement direction
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        moveDirection = (forward * Input.GetAxis("Vertical") + right * Input.GetAxis("Horizontal")).normalized;

        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

        // Rotate player model to face moving direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotateSpeed * Time.deltaTime);
        }
    }

    //private void UpdateAnimator()
    //{
    //    animator.SetFloat("Speed", moveDirection.magnitude);
    //}
}
