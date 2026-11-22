using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public StateMachine MenuStateMachine { get; private set; } = new StateMachine();

    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject storePanel;
    public GameObject creditsPanel;
    public GameObject playPanel;
    public GameObject logsPanel;

    [Header("Loading Screen")]
    public GameObject loadingPanel;
    public Slider progressBar;

    [Header("Scenes List")]
    public List<string> sceneNames = new List<string>();

    private SceneLoader loader;

    private void Start()
    {
        loader = ServiceLocator.Instance.Get<SceneLoader>();

        mainMenuPanel.SetActive(false);
        storePanel.SetActive(false);
        creditsPanel.SetActive(false);
        playPanel.SetActive(false);
        logsPanel.SetActive(false);

        MenuStateMachine.ChangeState(new MainMenuState(this));
    }

    private void Update()
    {
        MenuStateMachine.Update();
    }

    public void GoToMainMenu() => MenuStateMachine.ChangeState(new MainMenuState(this));
    public void GoToStore() => MenuStateMachine.ChangeState(new StoreState(this));
    public void GoToCredits() => MenuStateMachine.ChangeState(new CreditsState(this));
    public void GoToPlay() => MenuStateMachine.ChangeState(new PlayState(this));
    public void GoToPlugin() => MenuStateMachine.ChangeState(new PluginState(this));

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex < 1)
        {
            Debug.LogError("Cant load negative levels!");
            return;
        }

        if (levelIndex > sceneNames.Count)
        {
            Debug.LogError("Level out of index");
            return;
        }

        MenuStateMachine.ChangeState(new LoadingState(this));

        loader.LoadLevel(sceneNames[levelIndex-1]);
    }
}
