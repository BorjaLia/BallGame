public abstract class MenuState : IState
{
    protected MainMenuManager controller;

    public MenuState(MainMenuManager controller)
    {
        this.controller = controller;
    }

    public abstract void Enter();

    public virtual void Execute() { }

    public abstract void Exit();
}