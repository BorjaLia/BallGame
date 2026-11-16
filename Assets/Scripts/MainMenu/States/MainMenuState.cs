using UnityEngine;

public class MainMenuState : MenuState
{
    public MainMenuState(MainMenuManager controller) : base(controller) { }

    public override void Enter()
    {
        Debug.Log("Entering Main Menu");
        controller.mainMenuPanel.SetActive(true);
    }

    public override void Exit()
    {
        controller.mainMenuPanel.SetActive(false);
    }
}