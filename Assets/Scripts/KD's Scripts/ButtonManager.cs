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
    private void Start()
    {
        if(buttons.Length == 0) { buttons = FindObjectsOfType<Button>(); }
        if(trigger == null) { GameObject.Find("Exit").TryGetComponent(out trigger); }
        foreach(Button b in buttons) 
        { 
            b.SetButtonManager(this);
            b.buttonActivated += HandleButtonPress;
            b.buttonDeactivated += HandleButtonRelease;
        }
    }
    public void HandleButtonPress()
    {
        numButtonsActivated++;
        Debug.Log(numButtonsActivated);
        if (numButtonsActivated >= buttons.Length) 
        { 
            trigger.Activate();
        }
    }

    public void HandleButtonRelease()
    {
        numButtonsActivated--;
        if(numButtonsActivated < buttons.Length)
        {
            trigger.DeActivate();
        } 
    }

    private void OnDestroy()
    {
        foreach (Button b in buttons)
        {
            b.SetButtonManager(this);
            b.buttonActivated -= HandleButtonPress;
            b.buttonDeactivated -= HandleButtonRelease;
        }
    }
}
