using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    // Start is called before the first frame update
    Grid grid;
    [SerializeField] LayerMask BlockLayers = ~0;
    //[SerializeField] LayerMask Interactable = 1 << 6;
    //[SerializeField] LayerMask Ground = 1 << 3;
    [SerializeField] public bool snapToGridOnStart = true;
    [SerializeField] public bool isPushable = true;
    private bool pushing = false;
    private Vector3 pushTarget;
    private float timeElapsed = 0;
    private float pushSpeed = 1f;
    public delegate void StartedPushed();
    public StartedPushed startedPush;
    public delegate void FinishedPush();
    public FinishedPush finishedPush;

    float duration = 1;
    //float currentLerpTime = 0;

    public bool enableGravity = false;
    //bool grounded = true;
    public float gravity = 1f;
    Vector3 gravityTarget;
    bool gravityStarted = false;

    BoxCollider boxCollider;
    SphereCollider groundCheck;
    private void Awake()
    {
        grid = GameObject.Find("GridManager").GetComponent<Grid>();
        boxCollider = transform.parent.GetComponentInChildren<BoxCollider>();
        groundCheck = transform.parent.GetComponentInChildren<SphereCollider>();
    }
    void Start()
    {
        if(snapToGridOnStart) { Snap(transform.position); }
    }
    private void Update()
    {
        Sliding();
        Gravity();
    }
    /// <summary>
    /// Sets up variables for gravity to act on object, only if enabledGravity is true.
    /// </summary>
    /// <param name="ground"></param>
    public void StartGravity(Vector3 ground)
    {
        //Debug.Log("Start fall");
        if(!enableGravity) { return; }        
        gravityTarget = grid.WorldToCell(ground) + Vector3.up;
        //Debug.Log(transform.parent.name + "'s gravityTarget: " + gravityTarget);
        gravityStarted = true;
        isPushable = false;
    }
    /// <summary>
    /// If gravity is enabled and started, start moving downward towards gravityTarget.
    /// Once close enough to gravityTarget, snap the object to grid.
    /// </summary>
    public void Gravity()
    {
        if (enableGravity && gravityStarted)
        {
            //Debug.Log("falling");
            timeElapsed += Time.smoothDeltaTime;
            timeElapsed = Mathf.MoveTowards(timeElapsed, duration, Time.smoothDeltaTime);
            transform.position = Vector3.Lerp(transform.position, gravityTarget, timeElapsed * gravity);
            //transform.position = Vector3.Lerp(transform.position, gravityTarget, timeElapsed * pushSpeed);
            if (Vector3.Distance(transform.position, gravityTarget) < 0.1f)
            //if (transform.position == gravityTarget)
            {
                //Debug.Log(name + " is in position!");
                enableGravity = false;
                gravityStarted = false;
                isPushable = true;
                Snap(gravityTarget);
            }
        }
    }
    /// <summary>
    /// snaps grid object to nearest grid point
    /// </summary>
    /// <param name="target">position in space to convert to grid point</param>
    public void Snap(Vector3 target)
    {
        //Debug.Log("Snap");
        transform.position = grid.WorldToCell(target);
        boxCollider.transform.position = grid.WorldToCell(target);
        groundCheck.transform.position = transform.position + Vector3.down * 0.5f;
        //groundCheck.transform.position = grid.WorldToCell(target);
    }
    /// <summary>
    /// Called by the player's InteractionHandler OnControllerColliderHit.
    /// If not pushable, or is currently pushing, do nothing.
    /// Otherwise, checks which direction the object was pushed from, and whether there is space in the opposite direction.
    /// If there is space to push the object, set up variables for pushing it.
    /// </summary>
    public void CheckPush() 
    {
        if(!isPushable) { return; }
        if(pushing) { return; }
        Vector3[] directions = { transform.forward, -transform.forward, transform.right, -transform.right};
        foreach(Vector3 dir in directions)
        {
            if(IsPushDirection(dir))
            {
                Push(-dir);
                pushing = true;
                isPushable = false;
                break;
            }
        }
    }
    /// <summary>
    /// Sends a raycast to detect player. If the raycast hits the Player, and the opposite of direction is not Blocked, return true.
    /// Else, return false.
    /// </summary>
    /// <param name="direction">Direction of raycast that detects player.</param>
    /// <returns> Returns whether direction is the push direction, and can be pushed from that direction. </returns>
    private bool IsPushDirection(Vector3 direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit))
        {
            if(!hit.transform.CompareTag("Player") ){ return false; }
            if(IsBlocked(-direction)) { return false; }
            return true;
        }
        return false;
    }
    /// <summary>
    /// Sets up variables to push in direction, one grid unit.
    /// </summary>
    /// <param name="direction">The direction to push to.</param>
    private void Push(Vector3 direction) 
    {
        Vector3 newCellPosition = grid.WorldToCell(transform.position + direction);
        pushTarget = newCellPosition;
        timeElapsed = 0;
        startedPush?.Invoke();
        //StartCoroutine(DelayNewPush());
    }
    /// <summary>
    /// Slides the object towards pushTarget.
    /// When distance between pushTarget and transform is marginal, Finish pushing/sliding, and Snap to ensure object is on grid.
    /// </summary>
    private void Sliding() 
    {
        //Debug.Log("sliding but pushing");
        if(!pushing) { return; }
        timeElapsed += Time.smoothDeltaTime;
        timeElapsed = Mathf.MoveTowards(timeElapsed, duration, Time.smoothDeltaTime);
        transform.position = Vector3.Lerp(transform.position, pushTarget, timeElapsed * pushSpeed);
        //Debug.Log("sliding");
        if (Vector3.Distance(transform.position, pushTarget) < 0.1f)
        {
            //Debug.Log("finish sliding");
            finishedPush?.Invoke(); 
            Snap(pushTarget); 
            isPushable = true;
            pushing = false;
        }
    }
    /// <summary>
    /// Fires a raycast in direction, at a specific length. If there's a hit, then that direction is blocked. 
    /// Otherwise, it is not blocked.
    /// </summary>
    /// <param name="direction"> direction to fire raycast </param>
    /// <returns> true if blocked, false if not blocked </returns>
    private bool IsBlocked(Vector3 direction) 
    {
        //Debug.Log("is blocked");
        Debug.DrawRay(transform.position, direction * 1.2f, Color.red);
        if (Physics.Raycast(transform.position, direction, 1.2f, BlockLayers, QueryTriggerInteraction.Collide)) { return true; }
        return false;
    }
    private void CheckMatch() 
    { 
        //Debug.Log("Checking match"); 
    }
    //private void OnEnable()
    //{
    //    startedPush += () => Debug.Log(name + " was pushed");
    //}
    //private void OnDisable()
    //{
    //    startedPush -= () => Debug.Log(name + " was pushed");
    //}
}
