using UnityEngine;

public class LevelEditorManager : MonoBehaviour
{
    [Header("Hierarchy Setup")]
    [Tooltip("Parent transform for all generated objects. Defaults to this object if left empty.")]
    [SerializeField] private Transform gridParent;

    [Header("General Grid Settings")]
    [Tooltip("Total physical size of the floor in Unity units (X = Width, Y = Depth/Z).")]
    public Vector2 totalFloorSize = new Vector2(10f, 10f);

    [Tooltip("Number of tiles along X and Z.")]
    public Vector2Int gridCount = new Vector2Int(5, 5);

    [Tooltip("Scale spawned tiles to fit the calculated cell size (assumes 1x1 unit prefabs).")]
    public bool autoScaleTiles = true;

    [Header("Builders")]
    public FloorBuilder floorBuilder;
    public WallBuilder wallBuilder;

    public Transform TargetParent => gridParent != null ? gridParent : transform;

    public GridSettings CurrentSettings => new GridSettings
    {
        targetParent = TargetParent,
        totalFloorSize = totalFloorSize,
        gridCount = gridCount,
        autoScaleTiles = autoScaleTiles
    };

    [ContextMenu("Rebuild All")]
    public void RebuildAll()
    {
        RebuildFloor();
        RebuildWalls();
    }

    [ContextMenu("Clear All")]
    public void ClearAll()
    {
        ClearFloor();
        ClearWalls();
    }

    [ContextMenu("Generate Surrounding Walls")]
    public void GenerateSurroundingWalls()
    {
        if (wallBuilder == null) return;

        wallBuilder.GenerateSurroundingWalls(gridCount);
        RebuildWalls();
    }

    [ContextMenu("Sync Grid Lists")]
    public void SyncGridLayouts()
    {
        if (floorBuilder != null)
        {
            floorBuilder.SyncGridSize(gridCount);
        }

        if (wallBuilder != null)
        {
            wallBuilder.SyncGridSize(gridCount);
        }
    }

    public void RebuildFloor()
    {
        if (floorBuilder != null)
        {
            floorBuilder.Rebuild(CurrentSettings);
        }
    }

    public void RebuildWalls()
    {
        if (wallBuilder != null)
        {
            wallBuilder.Rebuild(CurrentSettings);
        }
    }

    public void ClearFloor()
    {
        if (floorBuilder != null)
        {
            floorBuilder.Clear(TargetParent);
        }
    }

    public void ClearWalls()
    {
        if (wallBuilder != null)
        {
            wallBuilder.Clear(TargetParent);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (gridCount.x <= 0 || gridCount.y <= 0) return;

        Gizmos.color = Color.cyan;
        float tileWidth = totalFloorSize.x / gridCount.x;
        float tileDepth = totalFloorSize.y / gridCount.y;

        Vector3 startOffset = new Vector3(
            -totalFloorSize.x * 0.5f + tileWidth * 0.5f,
            0f,
            -totalFloorSize.y * 0.5f + tileDepth * 0.5f
        );

        for (int x = 0; x < gridCount.x; x++)
        {
            for (int z = 0; z < gridCount.y; z++)
            {
                Vector3 center = TargetParent.TransformPoint(startOffset + new Vector3(x * tileWidth, 0f, z * tileDepth));
                Gizmos.DrawWireCube(center, new Vector3(tileWidth, 0.05f, tileDepth));
            }
        }
    }
}