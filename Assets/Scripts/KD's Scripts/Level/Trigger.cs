using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An abstract class for a generic trigger, with activate and deactivate functionality.
/// </summary>
public abstract class Trigger: MonoBehaviour
{

    /// <summary>
    /// Called when the trigger is activated.
    /// </summary>
    public abstract void Activate();

    /// <summary>
    /// Called when the trigger is deactivated.
    /// </summary>
    public abstract void DeActivate();
}
