using System.Collections;
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

    private void Start()
    {
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
        MenuStateMachine.ChangeState(new LoadingState(this));

        StartCoroutine(LoadSceneAsync(levelIndex));
    }

    private IEnumerator LoadSceneAsync(int levelIndex)
    {
        Application.backgroundLoadingPriority = ThreadPriority.BelowNormal;

        AsyncOperation operation = SceneManager.LoadSceneAsync(levelIndex);

        //operation.allowSceneActivation = false; 

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            if (progressBar != null)
            {
                progressBar.value = progress;
            }
            yield return null;
        }
    }
}
