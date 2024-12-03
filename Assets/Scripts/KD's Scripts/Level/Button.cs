using UnityEngine;

/// <summary>
/// Handles block - button activation through block color checks.
/// </summary>
public class Button : MonoBehaviour
{
    [Tooltip("Color for comparison when checking correct block. ColorAny by default.")]
    [SerializeField] public BlockColor color;
    private ButtonManager buttonManager;
    [SerializeField] LayerMask Interactable = 1 << 6;
    [Tooltip("Don't change...")]
    [SerializeField] public BlockColor colorAny;

    /// <summary>
    /// Events invoked when buttons are activated/deactivated.
    /// </summary>
    public delegate void ButtonActivated();
    public ButtonActivated buttonActivated;
    public delegate void ButtonDeactivated();
    public ButtonDeactivated buttonDeactivated;

    [SerializeField] GameObject effect;
    [SerializeField] int activationEffect;

    /// <summary>
    /// Activates the button if it's triggered by the correct block.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        // checks if the collider is interactable
        if ((Interactable & (1 << other.gameObject.layer)) != 0)
        {
            // checks if collider is a BlockContainer and matches the button color property.
            BlockContainer bC = other.GetComponentInParent<BlockContainer>();
            if (bC != null && bC.color.ID != color.ID && color.ID != colorAny.ID) { return; }
            
            // activates button
            buttonActivated?.Invoke();
            Instantiate(effect, transform.position, effect.transform.rotation);
            AudioManager.Instance.PlaySFX(activationEffect);
        }
    }
    /// <summary>
    /// Deactivates the button if the correct block is removed.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        // checks if collider is interactable
        if ((Interactable & (1 << other.gameObject.layer)) != 0)
        {
            // checks if collider is a BlockContainer and matches the button color property.
            BlockContainer bC = other.GetComponentInParent<BlockContainer>();
            if (bC != null) { if (bC.color.ID != color.ID && color.ID != colorAny.ID) { return; } }

            // deactivates button
            buttonDeactivated?.Invoke();
        }
    }
}
