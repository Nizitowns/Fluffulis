using TMPro;
using UnityEngine;

public class LevelSelectUIManager : MonoBehaviour
{
    public static LevelSelectUIManager Instance;

    public TMP_Text levelNameText;
    public GameObject levelNamePanel;

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
}
