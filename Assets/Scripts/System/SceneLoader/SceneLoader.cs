using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Globalization;

public class SceneLoader : MonoBehaviour
{
    private float loadProgress;
    public float LoadProgress { get { return loadProgress; } private set {} }

    private string mainMenuName = "MainMenu";

    public void LoadMainMenu()
    {
        LoadLevel(mainMenuName);
    }

    public void LoadLevel(string name)
    {
        StartCoroutine(LoadSceneAsync(name));
    }

    private IEnumerator LoadSceneAsync(string name)
    {
        Application.backgroundLoadingPriority = ThreadPriority.BelowNormal;

        AsyncOperation operation = SceneManager.LoadSceneAsync(name);

        //operation.allowSceneActivation = false; 

        while (!operation.isDone)
        {
            loadProgress = Mathf.Clamp01(operation.progress / 0.9f);

            yield return null;
        }
    }
}
