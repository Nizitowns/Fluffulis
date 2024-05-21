using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{
    public float lifetime;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, lifetime);
    }
}
