using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.Playables;

public class MainMenuManager : MonoBehaviour
{
    public StateMachine MenuStateMachine { get; private set; } = new StateMachine();

    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject storePanel;
    public GameObject creditsPanel;
    public GameObject playPanel;
    public GameObject logsPanel;

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
}
