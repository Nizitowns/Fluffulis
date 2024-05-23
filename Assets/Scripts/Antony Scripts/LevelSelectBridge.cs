using UnityEngine;

public class LevelSelectBridge : MonoBehaviour
{

    [SerializeField] string levelToUnlock;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt(levelToUnlock + "_unlocked") == 0)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
