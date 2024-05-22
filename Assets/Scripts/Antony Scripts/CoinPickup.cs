using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] int value;
    [SerializeField] int soundToPlay;
    [SerializeField] GameObject pickupEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(pickupEffect, transform.position, pickupEffect.transform.rotation);
            GameManager.Instance.AddCoins(value);

            Destroy(gameObject);
            AudioManager.Instance.PlaySFX(soundToPlay);
        }
    }
}
