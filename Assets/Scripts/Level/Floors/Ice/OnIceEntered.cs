
public struct IceTileEnteredEvent : IEvent
{
    public float ControlMultiplier;

    public IceTileEnteredEvent(float controlMultiplier)
    {
        ControlMultiplier = controlMultiplier;
    }
}