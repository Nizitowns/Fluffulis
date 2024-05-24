using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Vector3 respawnPosition;

    public int currentCoins;

    [SerializeField] GameObject deathEffect;
    private int levelCoins;

    //[SerializeField] int levelEndMusic;

    private void Awake()
    {
        // Ensure there's only one instance of GameManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        levelCoins = currentCoins;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;

        respawnPosition = Character.Instance.transform.position;

        AddCoins(0);
        UIManager.Instance.coinText.text = currentCoins.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadScene();
        }
        UIManager.Instance.coinText.text = currentCoins.ToString();
    }

    public void Respawn()
    {
        StartCoroutine(RespawnCoroutine());
    }

    public IEnumerator RespawnCoroutine()
    {
        Character.Instance.gameObject.SetActive(false);

        Instantiate(deathEffect, Character.Instance.transform.position + new Vector3(0f, 0.5f, 0f), Character.Instance.transform.rotation);

        UIManager.Instance.fadeToBlack = true;

        yield return new WaitForSeconds(1f);

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

        levelCoins = currentCoins;
        //AudioManager.Instance.PlayMusic(levelEndMusic);

        yield return new WaitForSeconds(2f);
        Debug.Log("load next scene in build settings");

        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_unlocked", 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void ReloadScene()
    {
        StartCoroutine(Reload());
    }
    public IEnumerator Reload()
    {
        // Get the name of the current active scene
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Load the current scene by its name
        SceneManager.LoadScene(currentSceneName);

        // Wait for the scene to load
        yield return new WaitForEndOfFrame();

        // Restore the current coins
        currentCoins = levelCoins;

        // Update the UI
        UIManager.Instance.coinText.text = currentCoins.ToString();
    }
}
