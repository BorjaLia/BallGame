using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public struct GridSettings
{
    public Transform targetParent;
    public Vector2 totalFloorSize;
    public Vector2Int gridCount;
    public bool autoScaleTiles;
}

[Serializable]
public struct TilePrefabEntry<TTileType> where TTileType : Enum
{
    public TTileType type;
    public GameObject prefab;
}

[Serializable]
public class GridRow<TTileType> where TTileType : Enum
{
    [HideInInspector] public string rowLabel;
    public List<TTileType> tiles = new List<TTileType>();
}

public abstract class GridBuilderBase<TTileType> : MonoBehaviour where TTileType : Enum
{
    [Header("Tile Prefabs")]
    public float verticalOffset;
    public TTileType defaultTileType;
    public List<TilePrefabEntry<TTileType>> tilePrefabs = new List<TilePrefabEntry<TTileType>>();

    [Header("Grid Layout (Rows = Z, Elements = X)")]
    public List<GridRow<TTileType>> gridRows = new List<GridRow<TTileType>>();

    protected abstract string ContainerName { get; }

    public void SyncGridSize(Vector2Int gridCount)
    {
        if (gridCount.x <= 0 || gridCount.y <= 0) return;

        while (gridRows.Count < gridCount.y)
        {
            gridRows.Add(new GridRow<TTileType>());
        }

        if (gridRows.Count > gridCount.y)
        {
            gridRows.RemoveRange(gridCount.y, gridRows.Count - gridCount.y);
        }

        for (int z = 0; z < gridCount.y; z++)
        {
            gridRows[z].rowLabel = $"Row {z} (Z={z})";
            List<TTileType> rowTiles = gridRows[z].tiles;

            while (rowTiles.Count < gridCount.x)
            {
                rowTiles.Add(defaultTileType);
            }

            if (rowTiles.Count > gridCount.x)
            {
                rowTiles.RemoveRange(gridCount.x, rowTiles.Count - gridCount.x);
            }
        }
    }

    public virtual void Rebuild(GridSettings settings)
    {
        Clear(settings.targetParent);

        if (settings.gridCount.x <= 0 || settings.gridCount.y <= 0)
        {
            Debug.LogWarning($"[{GetType().Name}] Grid count must be greater than 0 on both axes.");
            return;
        }

        SyncGridSize(settings.gridCount);

        var prefabMap = new Dictionary<TTileType, GameObject>();
        foreach (var entry in tilePrefabs)
        {
            if (!prefabMap.ContainsKey(entry.type))
                prefabMap.Add(entry.type, entry.prefab);
        }

        GameObject container = new GameObject(ContainerName);
        container.transform.SetParent(settings.targetParent, false);

        Vector3 currentPos = container.transform.position;
        currentPos.y += verticalOffset;
        container.transform.position = currentPos;

        float tileWidth = settings.totalFloorSize.x / settings.gridCount.x;
        float tileDepth = settings.totalFloorSize.y / settings.gridCount.y;

        Vector3 startOffset = new Vector3(
            -settings.totalFloorSize.x * 0.5f + tileWidth * 0.5f,
            0f,
            -settings.totalFloorSize.y * 0.5f + tileDepth * 0.5f
        );

        for (int x = 0; x < settings.gridCount.x; x++)
        {
            GameObject columnObj = new GameObject($"Column_{x}");
            columnObj.transform.SetParent(container.transform, false);

            for (int z = 0; z < settings.gridCount.y; z++)
            {
                TTileType currentType = gridRows[z].tiles[x];

                if (!prefabMap.TryGetValue(currentType, out GameObject prefab) || prefab == null)
                    continue;

                Vector3 localPos = startOffset + new Vector3(x * tileWidth, 0f, z * tileDepth);
                GameObject tile = SpawnPrefab(prefab, columnObj.transform);

                tile.name = $"Tile_{x}_{z}_{currentType}";
                tile.transform.localPosition = localPos;

                if (settings.autoScaleTiles)
                {
                    tile.transform.localScale = new Vector3(
                        tileWidth,
                        tile.transform.localScale.y,
                        tileDepth
                    );
                }
            }
        }
    }

    public virtual void Clear(Transform targetParent)
    {
        if (targetParent == null) return;

        Transform existing = targetParent.Find(ContainerName);
        if (existing != null)
        {
            if (Application.isPlaying)
                Destroy(existing.gameObject);
            else
                DestroyImmediate(existing.gameObject);
        }
    }

    protected GameObject SpawnPrefab(GameObject prefab, Transform parent)
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parent);
            return instance != null ? instance : Instantiate(prefab, parent);
        }
#endif
        return Instantiate(prefab, parent);
    }
}