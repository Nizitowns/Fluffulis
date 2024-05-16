using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    Button[] buttons;
    public int numButtonsActivated = 0;
    public delegate void ButtonActivated();
    public static ButtonActivated buttonActivated;
    public delegate void ButtonDeactivated();
    public static ButtonDeactivated buttonDeactivated;
    private void Awake()
    {
        buttons = FindObjectsOfType<Button>();
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
        LevelExit.enableExit();
    }
    public void LockNextLevel()
    {
        //Debug.Log("Level locked");
        LevelExit.disableExit();
    }

}
