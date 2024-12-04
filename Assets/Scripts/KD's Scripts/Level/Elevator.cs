using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/// <summary>
/// Elevator is a component for handling triggerable movement to and from a start position and target position.
/// </summary>
public class Elevator : Trigger
{
    [Tooltip("The position that the Elevator moves towards. On activate, move towards target.")]
    [SerializeField] public Transform target;
    [Tooltip("The position the Elevator starts at. Default is its own transform. On Deactivate, move towards start.")]
    [SerializeField] public Transform start = null;
    private Vector3 targetPos;
    private Vector3 startPos;
    int soundToPlay = 8;

    private bool activated = false;
    private float timeElapsed = 0;
    private Vector3 currentTarget;
    private float duration = 1f;
    [SerializeField] public float speed = 0.05f;
    private Grid grid;
    private bool snap = false;
    private List<BlockContainer> blocks;
    /// <summary>
    /// Initializes properties of Elevator, ensures the start and target transforms are snapped to the grid.
    /// </summary>
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
    }
    /// <summary>
    /// Begins moving the elevator towards the target.
    /// </summary>
    public override void Activate()
    {
        if(transform.position != startPos) { return; }
        InitializeElevator(targetPos);
        activated = true;
        snap = false;
    }
    /// <summary>
    /// Begins moving the elevator towards the start.
    /// </summary>
    public override void DeActivate()
    {
        if (transform.position != targetPos) { return; }
        InitializeElevator(startPos);
        activated = false;
        snap = false;
    }
    /// <summary>
    /// Resets state properties before moving the elevator.
    /// </summary>
    /// <param name="pos"></param>
    private void InitializeElevator(Vector3 pos)
    {
        timeElapsed = 0f;
        currentTarget = pos;
        TogglePushableBlocks(false);
        PlayElevatorMoveSound();
    }
    /// <summary>
    /// Toggles whether blocks in the level can be pushed. 
    /// Blocks should not be pushable when the elevator is moving.
    /// </summary>
    /// <param name="val"></param>
    private void TogglePushableBlocks(bool val)
    {
        blocks = GetComponentsInChildren<BlockContainer>().ToList();
        for(int i=0; i<blocks.Count; i++)
        {
            blocks[i].enablePush = val;
        }
    }
    /// <summary>
    /// Handles movement of elevator at each frame, depending on whether the trigger was activated or deactivated.
    /// </summary>
    private void Update()
    {
        if (activated && transform.position != targetPos && !snap) 
        {
            timeElapsed = Mathf.MoveTowards(timeElapsed, duration, Time.smoothDeltaTime * speed);
            transform.position = Vector3.Lerp(transform.position, targetPos, timeElapsed * Time.timeScale);
            if (Vector3.Distance(transform.position, targetPos) > 0.1f) { return; }
            if (snap) { return; }
            Snap();
            TogglePushableBlocks(true);
        }
        else if (!activated && transform.position != startPos && !snap)
        {
            timeElapsed = Mathf.MoveTowards(timeElapsed, duration, Time.smoothDeltaTime * speed * Time.timeScale);
            transform.position = Vector3.Lerp(transform.position, startPos, timeElapsed);
            if (Vector3.Distance(transform.position, startPos) > 0.1f) { return; }
            if(snap) { return; }
            Snap();
            TogglePushableBlocks(true);
        }
    }
    /// <summary>
    /// Snaps the elevator to the grid.
    /// </summary>
    public void Snap()
    {
        snap = true;
        transform.position = grid.WorldToCell(currentTarget);
    }
    /// <summary>
    /// Snaps the Transform parameter to the grid.
    /// </summary>
    /// <param name="t"> The Transform being snapped. </param>
    public void Snap(Transform t)
    {
        t.position = grid.WorldToCell(t.position);
    }
    /// <summary>
    /// Plays an elevator sound (when elevator begins moving).
    /// </summary>
    public void PlayElevatorMoveSound()
    {
        AudioManager.Instance.PlaySFX(soundToPlay);
    }
}
