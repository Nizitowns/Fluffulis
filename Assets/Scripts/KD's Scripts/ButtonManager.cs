using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    [Tooltip("The set of buttons this component manages. If not set, all buttons must be activated to activate the trigger.")]
    [SerializeField] Button[] buttons;    
    [Tooltip("The trigger/condition that activates when all buttons are activated. i.e. LevelExit.")]
    [SerializeField] public Trigger trigger;
    public int numButtonsActivated = 0;
    public delegate void ButtonActivated();
    public ButtonActivated buttonActivated;
    public delegate void ButtonDeactivated();
    public ButtonDeactivated buttonDeactivated;

    
    private void Start()
    {
        if(buttons.Length == 0) { buttons = FindObjectsOfType<Button>(); }
        if(trigger == null) { GameObject.Find("Exit").TryGetComponent(out trigger); }
    }
    private void OnEnable()
    {
        buttonActivated += HandleButtonPress;
        buttonDeactivated += HandleButtonRelease;
    }
    private void OnDisable()
    {
        buttonActivated -=  HandleButtonPress;
        buttonDeactivated -= HandleButtonRelease;
    }

    public void HandleButtonPress()
    {
        numButtonsActivated++;
        Debug.Log(numButtonsActivated);
        if (numButtonsActivated >= buttons.Length) 
        { 
            UnlockNextLevel(); 
        }
    }

    public void HandleButtonRelease()
    {
        numButtonsActivated--;
        if(numButtonsActivated < buttons.Length)
        {
            LockNextLevel();
        } 
    }

    public void UnlockNextLevel() 
    {
        //Debug.Log("Unlock next level!");
        trigger.Activate();
    }
    public void LockNextLevel()
    {
        //Debug.Log("Level locked");
        trigger.DeActivate();
    }

}
