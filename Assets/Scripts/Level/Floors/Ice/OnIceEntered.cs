
public struct IceTileEnteredEvent : IEvent
{
    public float DampingMultiplier;
    public float ControlMultiplier;

    public IceTileEnteredEvent(float dampingMultiplier, float controlMultiplier)
    {
        DampingMultiplier = dampingMultiplier;
        ControlMultiplier = controlMultiplier;
    }
}