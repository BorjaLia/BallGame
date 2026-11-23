public struct StatChangedEvent : IEvent
{
    public StatUpgradeData StatData;
    public int SelectedLevel;
    public int UnlockedLevel;

    public StatChangedEvent(StatUpgradeData statData, int selectedLevel, int unlockedLevel)
    {
        StatData = statData;
        SelectedLevel = selectedLevel;
        UnlockedLevel = unlockedLevel;
    }
}