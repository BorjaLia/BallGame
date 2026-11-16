using UnityEngine;

public class CreditsState : MenuState
{
    public CreditsState(MainMenuManager controller) : base(controller) { }

    public override void Enter()
    {
        Debug.Log("Entering Credits");
        controller.creditsPanel.SetActive(true);
    }

    public override void Exit()
    {
        controller.creditsPanel.SetActive(false);
    }
}