using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Vector3 respawnPosition;

    public int currentCoins;

    [SerializeField] int levelEndMusic;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        respawnPosition = Character.Instance.transform.position;

        AddCoins(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }

    public void Respawn()
    {
        StartCoroutine(RespawnCoroutine());
    }

    public IEnumerator RespawnCoroutine()
    {
        Character.Instance.gameObject.SetActive(false);

        UIManager.Instance.fadeToBlack = true;

        yield return new WaitForSeconds(2f);

        UIManager.Instance.fadeFromBlack = true;

        Character.Instance.transform.position = respawnPosition;
        Character.Instance.gameObject.SetActive(true);
    }

    public void AddCoins(int coinsToAdd)
    {
        currentCoins += coinsToAdd;
        UIManager.Instance.coinText.text = currentCoins.ToString();
    }

    public void PauseUnpause()
    {
        if (UIManager.Instance.pauseScreen.activeInHierarchy)
        {
            UIManager.Instance.pauseScreen.SetActive(false);
            Time.timeScale = 1f;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            UIManager.Instance.pauseScreen.SetActive(true);
            UIManager.Instance.CloseOptions();
            Time.timeScale = 0f;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public IEnumerator LevelEndCoroutine()
    {
        //AudioManager.Instance.PlayMusic(levelEndMusic);

        yield return new WaitForSeconds(2f);

    }
}
