using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    private bool isSceneAActive = true; // Track which scene is currently active.

    private void Start()
    {
        // Determine initial state based on the active scene.
        var currentSceneName = SceneManager.GetActiveScene().name;
        isSceneAActive = currentSceneName == "SceneA"; // Replace "SceneA" with your actual scene name.
    }

    public void ToggleScene()
    {
        StartCoroutine(TransitionToOtherScene());
    }

    private IEnumerator TransitionToOtherScene()
    {
        // Determine which scene to load based on current flag state.
        string sceneToLoad = isSceneAActive ? "SceneB" : "SceneA"; // Replace with actual scene names.

        // Unload the current scene.
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

        // Load the new scene in single mode.
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);

        // Flip the flag state.
        isSceneAActive = !isSceneAActive;
    }
}
