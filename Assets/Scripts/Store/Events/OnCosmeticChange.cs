public struct CosmeticChangedEvent : IEvent
{
    public CosmeticColorData SelectedColor;
    public CosmeticChangedEvent(CosmeticColorData selectedColor) => SelectedColor = selectedColor;
}