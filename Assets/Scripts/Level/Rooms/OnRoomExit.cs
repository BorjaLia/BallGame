public struct OnRoomExit : IEvent
{
    public ExitWallScript ExitWall;
    public OnRoomExit(ExitWallScript exitWall) => ExitWall = exitWall;
}