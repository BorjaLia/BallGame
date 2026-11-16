using UnityEngine;

public class LoadingState : MenuState
{
    public LoadingState(MainMenuManager controller) : base(controller) { }

    public override void Enter()
    {
        Debug.Log("Entering Loading Screen");
        controller.loadingPanel.SetActive(true);

        if (controller.progressBar != null)
        {
            controller.progressBar.value = 0f;
        }
    }

    public override void Exit()
    {
        controller.loadingPanel.SetActive(false);
    }
}