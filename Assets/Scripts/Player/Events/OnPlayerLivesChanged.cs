public struct PlayerLivesChangedEvent : IEvent
{
    public int LivesRemaining;
    public PlayerLivesChangedEvent(int livesRemaining) => LivesRemaining = livesRemaining;
}