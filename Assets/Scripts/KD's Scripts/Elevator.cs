using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Elevator : Trigger
{
    [Tooltip("The position that the Elevator moves towards. On activate, move towards target.")]
    [SerializeField] public Transform target;
    [Tooltip("The position the Elevator starts at. Default is its own transform. On Deactivate, move towards start.")]
    [SerializeField] public Transform start = null;
    private Vector3 targetPos;
    private Vector3 startPos;


    private bool activated = false;
    private float timeElapsed = 0;
    private Vector3 currentTarget;
    private float duration = 1f;
    [SerializeField] public float speed = 0.05f;
    private Grid grid;
    private Dictionary<Transform, Transform> onTheElevator = new Dictionary<Transform, Transform>();
    private bool snap = false;
    private List<BlockContainer> blocks;
    private bool hasTriggerDetect;
    private void Start()
    {
        grid = GameObject.Find("GridManager").GetComponent<Grid>();
        if (start == null)
        {
            start = transform;
        }
        Snap(transform);
        Snap(target);
        targetPos = target.position;
        startPos = transform.position;
        TriggerDetect tD = GetComponentInChildren<TriggerDetect>();
        if(tD == null) { hasTriggerDetect = false; }
        else { hasTriggerDetect = true; }
    }
    public override void Activate()
    {
        //Debug.Log("Activate elevator");
        if(transform.position != startPos) { Debug.Log(name + " is not at startPos."); return; }
        InitializeElevator(targetPos);
        activated = true;
        snap = false;
    }

    public override void DeActivate()
    {
        //Debug.Log("Deactivate elevator");
        if (transform.position != targetPos) { Debug.Log(name + " is not at targetPos."); return; }
        InitializeElevator(startPos);
        activated = false;
        snap = false;
    }

    private void InitializeElevator(Vector3 pos)
    {
        timeElapsed = 0f;
        currentTarget = pos;
        //Debug.Log("current target: " + pos);
        TogglePushableBlocks(false);
    }
    private void TogglePushableBlocks(bool val)
    {
        //Debug.Log("Blocks can be pushed: " + val);
        blocks = GetComponentsInChildren<BlockContainer>().ToList();
        for(int i=0; i<blocks.Count; i++)
        {
            blocks[i].enablePush = val;
        }
    }
    private void Update()
    {
        if (activated && transform.position != targetPos && !snap) 
        {
            //Debug.Log("moving towards targetPos");
            //timeElapsed += Time.smoothDeltaTime;
            timeElapsed = Mathf.MoveTowards(timeElapsed, duration, Time.smoothDeltaTime * speed);
            transform.position = Vector3.Lerp(transform.position, targetPos, timeElapsed);
            if (Vector3.Distance(transform.position, targetPos) > 0.1f) { return; }
            if (snap) { return; }
            //Debug.Log("snap going towards target");
            Snap();
            TogglePushableBlocks(true);
        }
        else if (!activated && transform.position != startPos && !snap)
        {
            //Debug.Log("moving towards startPos");
            //timeElapsed += Time.smoothDeltaTime;
            timeElapsed = Mathf.MoveTowards(timeElapsed, duration, Time.smoothDeltaTime * speed);
            transform.position = Vector3.Lerp(transform.position, startPos, timeElapsed);
            if (Vector3.Distance(transform.position, startPos) > 0.1f) { return; }
            if(snap) { return; }
            //Debug.Log("snap going towards start");
            Snap();
            TogglePushableBlocks(true);
        }
    }
    public void Snap()
    {
        //Debug.Log("Snap");
        snap = true;
        transform.position = grid.WorldToCell(currentTarget);
    }
    public void Snap(Transform t)
    {
        t.position = grid.WorldToCell(t.position);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(!hasTriggerDetect) { return; }
        //Debug.Log("trigger enter: " + name);
        if (other.transform.name == "GroundCheck") { return; }
        if (onTheElevator.ContainsKey(other.transform)) { return; }
        //Debug.Log("elevator adds: " + other.transform.root.name);
        onTheElevator.Add(other.transform, other.transform.root);
        other.transform.root.parent = transform;
    }
    private void OnTriggerExit(Collider other)
    {
        if (!hasTriggerDetect) { return; }
        //Debug.Log("trigger exit: " + name);
        Elevator elevator;
        if (!TryGetComponent(out elevator)) { return; }
        if (!onTheElevator.ContainsKey(other.transform)) { return; }
        if (other.transform.name == "GroundCheck") { return; }
        //Debug.Log("elevator removes: " + onTheElevator[other.transform].name);
        onTheElevator[other.transform].parent = null;
        onTheElevator.Remove(other.transform);


    }
}
