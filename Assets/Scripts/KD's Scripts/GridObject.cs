using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    // Start is called before the first frame update
    Grid grid;
    [SerializeField] LayerMask BlockLayers = ~0;
    [SerializeField] LayerMask Interactable = 1 << 6;
    [SerializeField] LayerMask Ground = 1 << 3;
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

    bool enableGravity = false;
    bool grounded = true;
    float gravity = 1f;
    Vector3 gravityTarget;

    BoxCollider boxCollider;
    private void Awake()
    {
        grid = GameObject.Find("GridManager").GetComponent<Grid>();
        boxCollider = transform.parent.GetComponentInChildren<BoxCollider>();
    }
    void Start()
    {
        if(snapToGridOnStart) { Snap(); }
    }
    private void Update()
    {
        Sliding();
        Gravity();
    }

    void Gravity()
    {

        // end gravity
        Debug.DrawRay(transform.position, Vector3.down * 1f, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            if (!enableGravity && Vector3.Magnitude(transform.position - hit.point) > 1f)
            {
                gravityTarget = grid.WorldToCell(hit.point) + Vector3.up;
                //gravityTarget = hit.point;
                Debug.Log(name + "'s gravityTarget: " + gravityTarget);
                enableGravity = true;
                isPushable = false;
                timeElapsed = 0f;
                boxCollider.transform.position = gravityTarget;
                
            }
        }

        if (enableGravity) 
        {
            timeElapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, gravityTarget, timeElapsed * pushSpeed);
        }
        if(transform.position == gravityTarget) 
        {
            Debug.Log("in position!");
            enableGravity = false;
            isPushable = true;
        }
    }
    public void Snap()
    {
        //Debug.Log("Snap");
        transform.position = grid.WorldToCell(transform.position);
        boxCollider.transform.position = transform.position;
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
        StartCoroutine(DelayNewPush());
    }
    private void Sliding() 
    {
        if(!pushing) { return; }
        timeElapsed += Time.smoothDeltaTime;
        transform.position = Vector3.Lerp(transform.position, pushTarget, timeElapsed * pushSpeed);
        if(transform.position == pushTarget) 
        { 
            finishedPush?.Invoke(); 
            Snap(); 
            isPushable = true;
        }
    }
    IEnumerator DelayNewPush() 
    {
        yield return new WaitForSeconds(0.3f);
        pushing = false;
    }
    private bool IsBlocked(Vector3 direction) 
    {
        Debug.Log("is blocked");
        Debug.DrawRay(transform.position, direction * 1.2f, Color.red);
        if (Physics.Raycast(transform.position, direction, 1.2f, BlockLayers, QueryTriggerInteraction.Collide)) { return true; }
        return false;
    }
    private void CheckMatch() 
    { 
        Debug.Log("Checking match"); 
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
