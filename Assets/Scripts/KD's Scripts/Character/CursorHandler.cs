using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A component that locks and hides the cursor on start.
/// </summary>
public class CursorHandler : MonoBehaviour
{
    // Locks and hides the cursor on start if the current scene is not the first scene (main menu).
    void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0) { return; }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

}
