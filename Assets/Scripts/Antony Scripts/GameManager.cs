using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Vector3 respawnPosition;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        respawnPosition = Player.Instance.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Respawn()
    {
        StartCoroutine(RespawnCoroutine());
    }

    public IEnumerator RespawnCoroutine()
    {
        Player.Instance.gameObject.SetActive(false);

        yield return new WaitForSeconds(2f);

        Player.Instance.transform.position = respawnPosition;
        Player.Instance.gameObject.SetActive(true);
    }
}
