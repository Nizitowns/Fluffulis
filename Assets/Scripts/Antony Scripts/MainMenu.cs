using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string firstLevel, levelSelect;

    [SerializeField] private GameObject continueButton;

    [SerializeField] int[] soundToPlay;

    public string[] levelNames;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Continue"))
        {
            //continueButton.SetActive(true);
        }
        else
        {
            ResetProgress();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NewGame()
    {
        AudioManager.Instance.PlaySFX(soundToPlay[2]);
        SceneManager.LoadScene(firstLevel);

        PlayerPrefs.SetInt("Continue", 0);
        PlayerPrefs.SetString("CurrentLevel", firstLevel);

        ResetProgress();
    }

    public void Continue()
    {
        AudioManager.Instance.PlaySFX(soundToPlay[0]);
        //SceneManager.LoadScene(levelSelect);
    }

    public void QuitGame()
    {
        AudioManager.Instance.PlaySFX(soundToPlay[0]);
        Application.Quit();
    }

    public void ResetProgress()
    {
        for (int i = 0; i < levelNames.Length; i++)
        {
            PlayerPrefs.SetInt(levelNames[i] + "_unlocked", 0);
        }
    }

    public void PlayOnSettingsClickSound()
    {
        AudioManager.Instance.PlaySFX(soundToPlay[3]);
    }
    public void PlayOnHoverSound()
    {
        AudioManager.Instance.PlaySFX(soundToPlay[1]);
    }
}
