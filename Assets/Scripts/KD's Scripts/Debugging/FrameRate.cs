using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sets frame rate to a low value to debug frame rate related bugs.
/// </summary>
public class FrameRate : MonoBehaviour
{
    /// <summary>
    /// Sets frame rate to 5 at the start.
    /// </summary>
    void Start()
    {
        Application.targetFrameRate = 5;
    }

}
