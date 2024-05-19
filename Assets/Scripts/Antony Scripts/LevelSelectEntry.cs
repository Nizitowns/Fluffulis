using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectEntry : MonoBehaviour
{

    [SerializeField] string levelName, levelToCheck;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && canLoadLevel && levelUnlocked)
        {
            SceneManager.LoadScene(levelName);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canLoadLevel = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canLoadLevel = false;
        }
    }
}
