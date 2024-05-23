using UnityEngine;

public class LevelSelectResetPosition : MonoBehaviour
{
    public static LevelSelectResetPosition Instance;
    public Vector3 respawnPosition;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

        }
    }
}
