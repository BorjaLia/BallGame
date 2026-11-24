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
    public GameObject tutorialPanel;

    [Header("Loading Screen")]
    public GameObject loadingPanel;
    public Slider progressBar;

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
    public void GoToPlugin() => loader.LoadScene(SceneLoader.SceneNames.Plugin);
    public void GoToTutorial() => MenuStateMachine.ChangeState(new TutorialState(this));

    public void ShowAchivements()
    {
        GPGSManager.Instance.ShowAchievements();
    }
    public void LoadLevel(int levelIndex)
    {
        if (levelIndex < 1)
        {
            Debug.LogError("Cant load negative levels!");
            return;
        }

        if (levelIndex > (int)SceneLoader.SceneNames.LevelThree)
        {
            Debug.LogError("Level out of index");
            return;
        }

        MenuStateMachine.ChangeState(new LoadingState(this));

        loader.LoadScene((SceneLoader.SceneNames)(levelIndex-1));
    }
}
