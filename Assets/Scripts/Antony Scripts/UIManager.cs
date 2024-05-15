using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private Image blackScreen;
    [SerializeField] private float fadeSpeed;
    public bool fadeToBlack, fadeFromBlack;
    public TMP_Text coinText;
    public GameObject pauseScreen, optionsScreen;
    public Slider musicVolSlider, sfxVolSlider;


    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeToBlack)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 1f, fadeSpeed * Time.deltaTime));

            if (blackScreen.color.a == 1f)
            {
                fadeToBlack = false;
            }
        }

        if (fadeFromBlack)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 0f, fadeSpeed * Time.deltaTime));

            if (blackScreen.color.a == 0f)
            {
                fadeFromBlack = false;
            }
        }
    }

    public void Resume()
    {
        GameManager.Instance.PauseUnpause();
    }

    public void OpenOptions()
    {
        optionsScreen.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsScreen.SetActive(false);
    }

    public void LevelSelect()
    {

    }

    public void MainMenu()
    {

    }

    public void SetMusicLevel()
    {
        AudioManager.Instance.SetMusicLevel();
    }

    public void SetSFXLevel()
    {
        AudioManager.Instance.SetSFXLevel();
    }
}
