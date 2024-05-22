using UnityEngine;

public class LevelExit : Trigger
{
    // Start is called before the first frame update
    [Tooltip("keep false if blocks need to stay on buttons to activated. Make true if you can exit without having blocks stay on buttons.")]
    [SerializeField] public bool exit = false;
    public delegate void SuccessExit();
    public static FailExit successExit;
    public delegate void FailExit();
    public static FailExit failExit;
    [SerializeField] int soundToPlay;
    [SerializeField] GameObject effect;

    public void CanExit() { exit = true; /*Debug.Log("Exitable");*/ }
    public void CantExit() { exit = false; /*Debug.Log("Not Exitable");*/ }
    private void OnTriggerEnter(Collider other)
    {

        if (!exit)
        {
            //Debug.Log("player fails to exit");
            failExit?.Invoke();
            return;
        }
        if (other.CompareTag("Player"))
        {
            //Debug.Log("player successfully exit");

            AudioManager.Instance.PlaySFX(soundToPlay);
            successExit?.Invoke();
            StartCoroutine(GameManager.Instance.LevelEndCoroutine());
        }
    }

    public override void Activate()
    {
        Instantiate(effect, transform.position, effect.transform.rotation);
        CanExit();
    }

    public override void DeActivate()
    {
        CantExit();
    }

    public void ActivateEffect()
    {

    }
}
