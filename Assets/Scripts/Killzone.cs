using UnityEngine;

/// <summary>
/// Handles interactions that result in the player's death and respawn.
/// </summary>
public class Killzone : MonoBehaviour
{
    /// <summary>
    /// Called when another collider enters the trigger collider attached to this object.
    /// </summary>
    /// <param name="other">The collider that triggered the event.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerPlayerRespawn();
        }
    }

    /// <summary>
    /// Triggers the respawn process for the player through the GameManager.
    /// </summary>
    private void TriggerPlayerRespawn()
    {
        GameManager.Instance.Respawn();
    }
}
