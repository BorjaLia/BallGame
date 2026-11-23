public struct SpeedBoostTriggeredEvent : IEvent
{
    public float BoostForce;

    public SpeedBoostTriggeredEvent(float boostForce)
    {
        BoostForce = boostForce;
    }
}