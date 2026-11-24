public struct OnSpeedBoost : IEvent
{
    public float BoostForce;

    public OnSpeedBoost(float boostForce)
    {
        BoostForce = boostForce;
    }
}