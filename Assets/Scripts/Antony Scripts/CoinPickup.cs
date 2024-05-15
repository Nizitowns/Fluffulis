using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] int value;
    [SerializeField] GameObject pickupEffect;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

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
