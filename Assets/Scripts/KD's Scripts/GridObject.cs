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
    public void StartGravity(Vector3 ground)
    {
        //Debug.Log("Start fall");
        if(!enableGravity) { return; }        
        gravityTarget = grid.WorldToCell(ground) + Vector3.up;
        gravityStarted = true;
        isPushable = false;
    }
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
                Debug.Log(name + " is in position!");
                enableGravity = false;
                gravityStarted = false;
                isPushable = true;
                Snap(gravityTarget);
            }
        }
    }
    public void Snap(Vector3 target)
    {
        //Debug.Log("Snap");
        transform.position = grid.WorldToCell(target);
        boxCollider.transform.position = grid.WorldToCell(target);
        groundCheck.transform.position = transform.position + Vector3.down * 0.5f;
        //groundCheck.transform.position = grid.WorldToCell(target);
    }
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
    private void Push(Vector3 direction) 
    {
        Vector3 newCellPosition = grid.WorldToCell(transform.position + direction);
        pushTarget = newCellPosition;
        timeElapsed = 0;
        startedPush?.Invoke();
        //StartCoroutine(DelayNewPush());
    }
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
    private void OnEnable()
    {
        startedPush += () => Debug.Log(name + " was pushed");
    }
    private void OnDisable()
    {
        startedPush -= () => Debug.Log(name + " was pushed");
    }
}
