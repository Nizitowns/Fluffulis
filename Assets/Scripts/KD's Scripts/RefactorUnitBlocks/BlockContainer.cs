using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockContainer : MonoBehaviour
{
    [Tooltip("Color for comparison when checking correct button. ColorAny by default.")]
    [SerializeField] public BlockColor color;
    public Grid grid;
    List<UnitBlock> blocks;
    List<BoxCollider> blockColliders;
    List<MeshRenderer> meshRenderers;
    public delegate void StartPush();
    public StartPush startPush;

    public delegate void StartGravity();
    public StartGravity startGravity;

    /// <summary>
    /// Push Variables
    /// </summary>
    private float pushSpeed = 1f;
    private float duration = 1f;
    /// <summary>
    /// Push state
    /// </summary>
    public bool isPushable = true;
    private bool pushing = false;
    private float timeElapsed = 0f;
    private List<Vector3> pushTargets = new List<Vector3>();
    //private Vector3 pushTarget;

    /// <summary>
    /// Gravity Variables
    /// </summary>
    private float gravity = 1f;
    /// <summary>
    /// Gravity State
    /// </summary>
    private bool enableGravity = false;
    //private Vector3 gravityTarget;
    private List<Vector3> gravityTargets = new List<Vector3>();
    private void Awake()
    {
        blocks = GetComponentsInChildren<UnitBlock>().ToList();
        meshRenderers = GetComponentsInChildren<MeshRenderer>().ToList();
        grid = GameObject.Find("GridManager").GetComponent<Grid>();
        Snap(transform.position);
    }

    private void OnEnable()
    {
        ReceiveGravity();
        foreach(UnitBlock u in blocks) 
        { 
            u.receivePush += ReceivePush;
            u.GetComponentInChildren<UnitBlockGravity>().receiveGravity += ReceiveGravity;
        }
    }
    private void OnDisable()
    {
        foreach (UnitBlock u in blocks) 
        { 
            u.receivePush -= ReceivePush;
            u.GetComponentInChildren<UnitBlockGravity>().receiveGravity += ReceiveGravity;
        }
    }

    private void Update()
    {
        Falling();
        Sliding();
    }
    /// <summary>
    /// Receives notification from a unit block to enable gravity.
    /// If all other blocks can be affected by gravity, begin falling to the closest position.
    /// </summary>
    /// <param name="block"></param>
    public void ReceiveGravity()
    {
        if (enableGravity) { return; }
        float shortestDistance = Mathf.Infinity;
        UnitBlock block = blocks[0];
        Vector3 point = transform.position;
        foreach (UnitBlock u in blocks)
        {
            RaycastHit hit;
            if (Physics.Raycast(u.transform.position, Vector3.down, out hit))
            {
                if (hit.transform.gameObject.GetComponentInParent<BlockContainer>() == this) { continue; }
                float distance = Vector3.Distance(u.transform.position, hit.point);
                if(distance < shortestDistance && distance > 0.2f) { block = u; shortestDistance = distance; point = hit.point; }
            }
            else { /* no hit */}
        }
        enableGravity = true;
        isPushable = false;
        timeElapsed = 0f;
        if (shortestDistance == Mathf.Infinity)
        {
            // fall to void
            //gravityTarget = grid.WorldToCell(transform.position + Vector3.down * 200f);
            foreach(MeshRenderer m in meshRenderers) { gravityTargets.Add(m.transform.position + Vector3.down * 200f); }
            gravity = 1/200f;
        }
        else
        {
            foreach (MeshRenderer m in meshRenderers) { gravityTargets.Add(grid.WorldToCell(m.transform.position + Vector3.down * shortestDistance) + Vector3.up) ; }
            if (shortestDistance > 1) { gravity = 1 / shortestDistance; }
            else { gravity = 1; }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public void Falling() 
    {
        if(!enableGravity) { return; }
        timeElapsed += Time.smoothDeltaTime;
        timeElapsed = Mathf.MoveTowards(timeElapsed, duration, Time.smoothDeltaTime);
        for(int i=0; i<meshRenderers.Count; i++)
        {
            meshRenderers[i].transform.position = Vector3.Lerp(meshRenderers[i].transform.position, gravityTargets[i], timeElapsed * gravity);
        }
        for (int i = 0; i < meshRenderers.Count; i++)
        {
            if(Vector3.Distance(meshRenderers[i].transform.position, gravityTargets[i]) > 0.1f) { return; }
            //Debug.Log(name + " is in position: " + grid.WorldToCell(transform.position));
        }            
        enableGravity = false;
        isPushable = true;
        for (int i = 0; i < blocks.Count; i++) { blocks[i].Snap(gravityTargets[i]); }
        gravityTargets = new List<Vector3>();
    }
    /// <summary>
    /// Receive notification of a push, starts pushing if all unit blocks can be pushed in the push direction.
    /// </summary>
    /// <param name="block">The original unit block that was pushed. </param>
    public void ReceivePush(UnitBlock block)
    {
        //Debug.Log("Container ReceivePush");
        if(pushing) { return; }
        if(!isPushable) { return; }
        Vector3 pushDirection = block.GetPushDirection();
        if(pushDirection == Vector3.zero) { return; }
        foreach(UnitBlock u in blocks) { if (u.IsBlocked(pushDirection)) { return; } }
        PushBlockContainer(pushDirection);
    }
    /// <summary>
    /// Initializes push state variables
    /// </summary>
    /// <param name="dir"> The direction to push in </param>
    public void PushBlockContainer(Vector3 dir)
    {
        timeElapsed = 0f;
        pushing = true;
        isPushable = false;
        foreach(MeshRenderer m in meshRenderers) { pushTargets.Add(grid.WorldToCell(m.transform.position + dir)); }
        //pushTarget = transform.position + dir;
    }
    /// <summary>
    /// Lerp towards push target, and reset push state variables when finished lerping.
    /// </summary>
    private void Sliding()
    {
        //Debug.Log("sliding but pushing");
        if (!pushing) { return; }
        timeElapsed += Time.smoothDeltaTime;
        timeElapsed = Mathf.MoveTowards(timeElapsed, duration, Time.smoothDeltaTime);
        //transform.position = Vector3.Lerp(transform.position, pushTarget, timeElapsed * pushSpeed);
        //Debug.Log("sliding");
        for (int i = 0; i < meshRenderers.Count; i++)
        {
            meshRenderers[i].transform.position = Vector3.Lerp(meshRenderers[i].transform.position, pushTargets[i], timeElapsed * gravity);
        }
        for (int i = 0; i < meshRenderers.Count; i++)
        {
            if (Vector3.Distance(meshRenderers[i].transform.position, pushTargets[i]) > 0.1f) { return; }
            //Debug.Log(name + " is in position: " + grid.WorldToCell(transform.position));
        }
        for (int i = 0; i < blocks.Count; i++) { blocks[i].Snap(pushTargets[i]); }
        pushing = false;
        isPushable = true;
        pushTargets = new List<Vector3>();
    }
    /// <summary>
    /// Snaps container to nearest point on grid.
    /// </summary>
    /// <param name="target"> Target should be the nearest point on grid. </param>
    public void Snap(Vector3 target)
    {
        //Debug.Log("Snap");
        transform.position = grid.WorldToCell(target);
    }
}
