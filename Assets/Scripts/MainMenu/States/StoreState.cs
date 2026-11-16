using UnityEngine;
public class StoreState : MenuState
{
    public StoreState(MainMenuManager controller) : base(controller) { }

    public override void Enter()
    {
        Debug.Log("Entering Store");
        controller.storePanel.SetActive(true);
    }

    public override void Exit()
    {
        controller.storePanel.SetActive(false);
    }
}