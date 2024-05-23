using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectEntry : MonoBehaviour
{

    [SerializeField] string levelName, levelToCheck, displayName;

    private bool canLoadLevel, levelUnlocked;

    [SerializeField] GameObject mapPointActive, mapPointInactive;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt(levelToCheck + "_unlocked") == 1 || levelToCheck == "")
        {
            mapPointActive.SetActive(true);
            mapPointInactive.SetActive(false);
            levelUnlocked = true;
        }
        else
        {
            mapPointActive.SetActive(false);
            mapPointInactive.SetActive(true);
            levelUnlocked = false;
        }

        if (PlayerPrefs.GetString("CurrentLevel") == levelName)
        {
            Player.Instance.transform.position = transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && canLoadLevel && levelUnlocked)
        {
            SceneManager.LoadScene(levelName);
            PlayerPrefs.SetString("CurrentLevel", levelName);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canLoadLevel = true;

            LevelSelectUIManager.Instance.levelNamePanel.SetActive(true);
            LevelSelectUIManager.Instance.levelNameText.text = displayName;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canLoadLevel = false;

            LevelSelectUIManager.Instance.levelNamePanel.SetActive(false);
        }
    }
}
