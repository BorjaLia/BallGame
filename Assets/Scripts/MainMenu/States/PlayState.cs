using UnityEngine;

public class PlayState : MenuState
{
    public PlayState(MainMenuManager controller) : base(controller) { }

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