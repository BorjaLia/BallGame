public enum FloorTileType
{
    Normal,
    Hole,
    Speed,
    Ice
}

public class FloorBuilder : GridBuilderBase<FloorTileType>
{
    protected override string ContainerName => "Generated_Floor_Tiles";
}