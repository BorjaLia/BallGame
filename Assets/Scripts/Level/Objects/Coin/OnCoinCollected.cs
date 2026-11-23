public struct OnCoinCollected : IEvent 
{
    int value;

    public OnCoinCollected(int value)
    {
        this.value = value;

        if(value <= 0)
        {
            this.value = 1;
        }
    }
};