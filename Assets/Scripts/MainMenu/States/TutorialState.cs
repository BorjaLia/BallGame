using UnityEngine;
public class TutorialState : MenuState
{
    public TutorialState(MainMenuManager controller) : base(controller) { }

    public override void Enter()
    {
        Debug.Log("Entering Tutorial");
        controller.tutorialPanel.SetActive(true);
    }

    public override void Exit()
    {
        controller.tutorialPanel.SetActive(false);
    }
}