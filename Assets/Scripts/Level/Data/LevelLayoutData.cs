using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelLayout", menuName = "Level Editor/Level Layout Data")]
public class LevelLayoutData : ScriptableObject
{
    [Header("General Grid Settings")]
    public Vector2 totalFloorSize = new Vector2(10f, 10f);
    public Vector2Int gridCount = new Vector2Int(5, 5);
    public bool autoScaleTiles = true;

    [Header("Floor Data")]
    public float floorVerticalOffset;
    public FloorTileType floorDefaultTile = FloorTileType.Normal;
    public List<GridRow<FloorTileType>> floorRows = new List<GridRow<FloorTileType>>();

    [Header("Wall Data")]
    public float wallVerticalOffset;
    public WallTileType wallDefaultTile = WallTileType.None;
    public List<GridRow<WallTileType>> wallRows = new List<GridRow<WallTileType>>();
}