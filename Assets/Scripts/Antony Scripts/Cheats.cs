using UnityEngine;
using UnityEngine.SceneManagement;

public class Cheats : MonoBehaviour
{
    void Update()
    {
        CheckForSceneChangeInput();
    }

    private void CheckForSceneChangeInput()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            LoadNextScene();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            LoadPreviousScene();
        }
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Check if the next scene index exceeds available scenes
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No next scene to load, current scene is the last one.");
        }
    }

    private void LoadPreviousScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int previousSceneIndex = currentSceneIndex - 1;

        // Check if the previous scene index is valid
        if (previousSceneIndex >= 0)
        {
            SceneManager.LoadScene(previousSceneIndex);
        }
        else
        {
            Debug.LogWarning("No previous scene to load, current scene is the first one.");
        }
    }
}