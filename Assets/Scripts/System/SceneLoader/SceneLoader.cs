using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneLoader : MonoBehaviour
{
    public enum SceneNames
    { 
        MainMenu,
        LevelOne,
        LevelTwo,
        LevelThree,
        Plugin
    }

    private float loadProgress;
    public float LoadProgress { get { return loadProgress; } private set {} }

    public readonly Dictionary<SceneNames,string> availableScenes = new Dictionary<SceneNames, string>();

    private void Awake()
    {
        availableScenes.Add(SceneNames.MainMenu,"MainMenu");
        availableScenes.Add(SceneNames.LevelOne,"LevelOne");
        availableScenes.Add(SceneNames.LevelTwo,"LevelTwo");
        availableScenes.Add(SceneNames.LevelThree,"LevelThree");
        availableScenes.Add(SceneNames.Plugin,"Plugin");
    }

    public void LoadScene(SceneNames scene)
    {
        string value;

        availableScenes.TryGetValue(scene, out value);

        if (value != null) 
        {
            LoadScene(value);
        }
        else
        {
            Debug.LogError("Coulnt find scene!");
        }
    }

    public void LoadScene(string name)
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
