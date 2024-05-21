using UnityEngine;

public class LevelExit : Trigger
{
    // Start is called before the first frame update
    private bool exit = false;
    public delegate void SuccessExit();
    public static FailExit successExit;
    public delegate void FailExit();
    public static FailExit failExit;

    public void CanExit() { exit = true; /*Debug.Log("Exitable");*/ }
    public void CantExit() { exit = false; /*Debug.Log("Not Exitable");*/ }
    private void OnTriggerEnter(Collider other)
    {

        if(!exit)
        {
            //Debug.Log("player fails to exit");
            failExit?.Invoke();
            return;
        }
        if (other.CompareTag("Player"))
        {        
            //Debug.Log("player successfully exit");
            successExit?.Invoke();
            StartCoroutine(GameManager.Instance.LevelEndCoroutine());
        }
    }

    public override void Activate()
    {
        CanExit();
    }

    public override void DeActivate()
    {
        CantExit();
    }
}
