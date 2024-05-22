using UnityEngine;

public class Button : MonoBehaviour
{
    [Tooltip("Color for comparison when checking correct block. ColorAny by default.")]
    [SerializeField] public BlockColor color;
    //[Tooltip("Button manager that manages button.")]
    //[SerializeField] public ButtonManager buttonManager;
    private ButtonManager buttonManager;
    [SerializeField] LayerMask Interactable = 1 << 6;
    [Tooltip("Don't change...")]
    [SerializeField] public BlockColor colorAny;
    public delegate void ButtonActivated();
    public ButtonActivated buttonActivated;
    public delegate void ButtonDeactivated();
    public ButtonDeactivated buttonDeactivated;
    [SerializeField] GameObject effect;
    [SerializeField] int activationEffect;

    //private void Awake()
    //{
    //    colorAny = Resources.Load<BlockColor>("BlockColor/ColorAny");
    //}
    //private void Start()
    //{
    //    //if (color == null) { color = Resources.Load<BlockColor>("ColorAny"); }
    //    if (buttonManager == null) { GameObject.Find("ButtonManager").TryGetComponent(out buttonManager); }
    //}
    private void OnTriggerEnter(Collider other)
    {
        if ((Interactable & (1 << other.gameObject.layer)) != 0)
        {
            BlockContainer bC = other.GetComponentInParent<BlockContainer>();
            Debug.Log("block container: " + bC.name);
            if (bC != null && bC.color.ID != color.ID && color.ID != colorAny.ID) { return; }
            //Debug.Log(other.name + " has hit button");
            //buttonManager.buttonActivated?.Invoke();
            buttonActivated?.Invoke();

            Instantiate(effect, transform.position, effect.transform.rotation);
            AudioManager.Instance.PlaySFX(activationEffect);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if ((Interactable & (1 << other.gameObject.layer)) != 0)
        {
            BlockContainer bC = other.GetComponentInParent<BlockContainer>();
            if (bC != null) { if (bC.color.ID != color.ID && color.ID != colorAny.ID) { return; } }
            //Debug.Log(other.name + " exits button");
            //buttonManager.buttonDeactivated?.Invoke();
            buttonDeactivated?.Invoke();
        }
    }
    //public void SetButtonManager(ButtonManager bM) { buttonManager = bM; }
}
