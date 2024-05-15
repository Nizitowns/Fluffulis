using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] int value;
    //[SerializeField] GameObject pickupEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Instantiate(pickupEffect, transform.position, transform.rotation);
            GameManager.Instance.AddCoins(value);

            Destroy(gameObject);
        }
    }
}
