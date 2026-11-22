public enum WallTileType
{
    None,
    Normal,
    Exit,
    Breakable
}

public class WallBuilder : GridBuilderBase<WallTileType>
{
    protected override string ContainerName => "Generated_Wall_Tiles";
}