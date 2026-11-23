public struct OnCoinCollected : IEvent
{
    public int Amount;
    public OnCoinCollected(int amount) => Amount = amount;
}

public struct CoinsChangedEvent : IEvent
{
    public int NewTotal;
    public CoinsChangedEvent(int newTotal) => NewTotal = newTotal;
}