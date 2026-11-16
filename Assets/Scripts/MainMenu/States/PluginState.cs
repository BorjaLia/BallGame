using UnityEngine;

public class PluginState : MenuState
{
    public PluginState(MainMenuManager controller) : base(controller) { }

    public override void Enter()
    {
        Debug.Log("Entering Play");
        controller.logsPanel.SetActive(true);
    }

    public override void Exit()
    {
        controller.logsPanel.SetActive(false);
    }
}