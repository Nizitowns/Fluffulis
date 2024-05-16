using UnityEngine;

public class LevelExit : MonoBehaviour
{
    // Start is called before the first frame update
    private bool exit = false;
    public delegate void EnableExit();
    public static EnableExit enableExit;
    public delegate void DisableExit();
    public static DisableExit disableExit;
    public delegate void SuccessExit();
    public static FailExit successExit;
    public delegate void FailExit();
    public static FailExit failExit;
    private void OnEnable()
    {
        enableExit += CanExit;
        disableExit += CantExit;
    }
    private void OnDisable()
    {
        enableExit -= CanExit;
        disableExit -= CantExit;
    }

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
}
