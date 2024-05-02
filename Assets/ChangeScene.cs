using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    private int currentSceneIndex;

    private void Start()
    {
        // Get the index of the current active scene
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void MoveToScene(int sceneID)
    {
        // Unload the current scene
        SceneManager.UnloadSceneAsync(currentSceneIndex);

        // Load the new scene with single mode
        SceneManager.LoadScene(sceneID, LoadSceneMode.Single);
    }
}
