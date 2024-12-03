using UnityEngine;

/// <summary>
/// ButtonManager is responsible for managing an array of Buttons. 
/// When all buttons are activated, it will activate a trigger. 
/// If there is at least one button that is not activated, it will deactivate the trigger.
/// </summary>
public class ButtonManager : MonoBehaviour
{
    [Tooltip("The set of buttons this component manages. If not set, all buttons must be activated to activate the trigger.")]
    [SerializeField] Button[] buttons;
    [Tooltip("The trigger/condition that activates when all buttons are activated. i.e. LevelExit.")]
    [SerializeField] public Trigger trigger;
    private int numButtonsActivated = 0;

    /// <summary>
    /// Handles some default ButtonManager behavior if the buttons and trigger properties are not specified.
    /// If no buttons are specified, ButtonManager will require all buttons in the scene to be activated for the trigger to activate.
    /// If no trigger is specified, ButtonManager will try tp use the Exit gameObject as the trigger.
    /// Subscribes HandleButtonPress and HandleButtonRelease to buttonActivated and buttonDeactivated for each button in buttons.
    /// </summary>
    private void Start()
    {
        if (buttons.Length == 0) { buttons = FindObjectsOfType<Button>(); Debug.Log(name + ""); }
        if (trigger == null) { GameObject.Find("Exit").TryGetComponent(out trigger); }
        foreach (Button b in buttons)
        {
            b.buttonActivated += HandleButtonPress;
            b.buttonDeactivated += HandleButtonRelease;
        }
    }
    /// <summary>
    /// Updates the number of activated buttons and activates the trigger if all of them are activated.
    /// </summary>
    public void HandleButtonPress()
    {
        numButtonsActivated++;
        if (numButtonsActivated >= buttons.Length)
        {
            trigger.Activate();
        }
    }
    /// <summary>
    /// Updates the number of activated buttons and deactivates the trigger if at least one of them is not activated.
    /// </summary>
    public void HandleButtonRelease()
    {
        numButtonsActivated--;
        if (numButtonsActivated < buttons.Length)
        {
            trigger.DeActivate();
        }
    }
    /// <summary>
    /// Unsubscribes HandleButtonPress and HandleButtonRelease from buttonActivated and buttonDeactivated for each button in buttons.
    /// </summary>
    private void OnDestroy()
    {
        foreach (Button b in buttons)
        {
            b.buttonActivated -= HandleButtonPress;
            b.buttonDeactivated -= HandleButtonRelease;
        }
    }
}
