using UnityEngine;

public class PluginState : MenuState
{
    public PluginState(MainMenuManager controller) : base(controller) { }

    public override void Enter()
    {
        Debug.Log("Entering Play");
        controller.playPanel.SetActive(true);
    }

    public override void Exit()
    {
        controller.playPanel.SetActive(false);
    }
}