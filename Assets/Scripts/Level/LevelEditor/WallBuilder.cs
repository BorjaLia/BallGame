using System.Collections.Generic;
using UnityEngine;

public enum WallTileType
{
    None,
    Normal,
    Entry,
    Exit,
    Breakable
}

public class WallBuilder : GridBuilderBase<WallTileType>
{
    protected override string ContainerName => "Generated_Wall_Tiles";

    public void GenerateSurroundingWalls(Vector2Int gridCount, WallTileType wallType = WallTileType.Normal)
    {
        if (gridCount.x <= 0 || gridCount.y <= 0) return;

        SyncGridSize(gridCount);

        for (int z = 0; z < gridCount.y; z++)
        {
            for (int x = 0; x < gridCount.x; x++)
            {
                bool isEdge = x == 0 || x == gridCount.x - 1 || z == 0 || z == gridCount.y - 1;
                if (!isEdge) continue;

                if (gridRows[z].tiles[x] == defaultTileType)
                {
                    gridRows[z].tiles[x] = wallType;
                }
            }
        }
    }
}