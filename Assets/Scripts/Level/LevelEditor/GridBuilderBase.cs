using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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

    [FormerlySerializedAs("prefab")]
    public GameObject centerPrefab;

    public GameObject edgePrefab;

    public GameObject cornerPrefab;

    public GameObject internalCornerPrefab;

    public bool allowInternalCornerOverride;
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

    [Header("Grid Layout")]
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

        var entryMap = new Dictionary<TTileType, TilePrefabEntry<TTileType>>();
        foreach (var entry in tilePrefabs)
        {
            if (!entryMap.ContainsKey(entry.type))
            {
                entryMap.Add(entry.type, entry);
            }
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

                if (!entryMap.TryGetValue(currentType, out TilePrefabEntry<TTileType> entry) || entry.centerPrefab == null) continue;

                ResolveTileShape(x, z, currentType, entry, entryMap, settings.gridCount, out GameObject prefab, out float yRotation);

                Vector3 localPos = startOffset + new Vector3(x * tileWidth, 0f, z * tileDepth);
                GameObject tile = SpawnPrefab(prefab, columnObj.transform);

                tile.name = $"Tile_{x}_{z}_{currentType}";
                tile.transform.localPosition = localPos;
                tile.transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);

                if (settings.autoScaleTiles)
                {
                    bool rotated90 = Mathf.Approximately(yRotation, 90f) || Mathf.Approximately(yRotation, 270f);
                    tile.transform.localScale = new Vector3(
                        rotated90 ? tileDepth : tileWidth,
                        tile.transform.localScale.y,
                        rotated90 ? tileWidth : tileDepth
                    );
                }
            }
        }
    }

    protected virtual bool IsConnected(int x, int z, TTileType targetType, Vector2Int gridCount)
    {
        if (x < 0 || x >= gridCount.x || z < 0 || z >= gridCount.y) return false;

        return EqualityComparer<TTileType>.Default.Equals(gridRows[z].tiles[x], targetType);
    }

    private void ResolveTileShape(
    int x, int z,
    TTileType type,
    TilePrefabEntry<TTileType> entry,
    Dictionary<TTileType, TilePrefabEntry<TTileType>> entryMap,
    Vector2Int gridCount,
    out GameObject prefab,
    out float yRotation)
    {
        // Default fallback
        prefab = entry.centerPrefab;
        yRotation = 0f;

        // 0. If this tile (e.g., Hole) can be overwritten, check if it sits inside another tile's L-bend
        if (entry.allowInternalCornerOverride &&
            TryResolveInternalCornerOverride(x, z, type, entryMap, gridCount, out GameObject overridePrefab, out float overrideRot))
        {
            prefab = overridePrefab;
            yRotation = overrideRot;
            return;
        }

        if (entry.edgePrefab == null && entry.cornerPrefab == null)
            return;

        bool n = IsConnected(x, z + 1, type, gridCount);
        bool s = IsConnected(x, z - 1, type, gridCount);
        bool e = IsConnected(x + 1, z, type, gridCount);
        bool w = IsConnected(x - 1, z, type, gridCount);

        if (entry.cornerPrefab != null)
        {
            if (!n && !w && s && e) { prefab = entry.cornerPrefab; yRotation = 0f; return; }
            if (!n && !e && s && w) { prefab = entry.cornerPrefab; yRotation = 90f; return; }
            if (!s && !e && n && w) { prefab = entry.cornerPrefab; yRotation = 180f; return; }
            if (!s && !w && n && e) { prefab = entry.cornerPrefab; yRotation = 270f; return; }
        }

        if (entry.edgePrefab != null)
        {
            if (!n && s && e && w) { prefab = entry.edgePrefab; yRotation = 0f; return; }
            if (!e && n && s && w) { prefab = entry.edgePrefab; yRotation = 90f; return; }
            if (!s && n && e && w) { prefab = entry.edgePrefab; yRotation = 180f; return; }
            if (!w && n && s && e) { prefab = entry.edgePrefab; yRotation = 270f; return; }
        }
    }

    private bool TryResolveInternalCornerOverride(
        int x, int z,
        TTileType currentType,
        Dictionary<TTileType, TilePrefabEntry<TTileType>> entryMap,
        Vector2Int gridCount,
        out GameObject prefab,
        out float yRotation)
    {
        prefab = null;
        yRotation = 0f;

        if (CheckBend(x, z, 0, -1, 1, 0, currentType, entryMap, gridCount, out prefab)) { yRotation = 0f; return true; } // Solid S + E (Open TL)
        if (CheckBend(x, z, 0, -1, -1, 0, currentType, entryMap, gridCount, out prefab)) { yRotation = 90f; return true; } // Solid S + W (Open TR)
        if (CheckBend(x, z, 0, 1, -1, 0, currentType, entryMap, gridCount, out prefab)) { yRotation = 180f; return true; } // Solid N + W (Open BR)
        if (CheckBend(x, z, 0, 1, 1, 0, currentType, entryMap, gridCount, out prefab)) { yRotation = 270f; return true; } // Solid N + E (Open BL)

        return false;
    }

    private bool CheckBend(
        int x, int z,
        int dx1, int dz1,
        int dx2, int dz2,
        TTileType currentType,
        Dictionary<TTileType, TilePrefabEntry<TTileType>> entryMap,
        Vector2Int gridCount,
        out GameObject internalPrefab)
    {
        internalPrefab = null;

        int x1 = x + dx1, z1 = z + dz1;
        int x2 = x + dx2, z2 = z + dz2;
        int xd = x + dx1 + dx2, zd = z + dz1 + dz2;

        if (!InBounds(x1, z1, gridCount) || !InBounds(x2, z2, gridCount) || !InBounds(xd, zd, gridCount))
            return false;

        TTileType neighborType = gridRows[z1].tiles[x1];

        if (EqualityComparer<TTileType>.Default.Equals(neighborType, currentType))
            return false;

        if (!entryMap.TryGetValue(neighborType, out var neighborEntry) || neighborEntry.internalCornerPrefab == null)
            return false;

        if (!IsConnected(x2, z2, neighborType, gridCount) || !IsConnected(xd, zd, neighborType, gridCount))
            return false;

        if (IsConnected(x - dx1, z - dz1, neighborType, gridCount) || IsConnected(x - dx2, z - dz2, neighborType, gridCount))
            return false;

        internalPrefab = neighborEntry.internalCornerPrefab;
        return true;
    }

    private bool InBounds(int x, int z, Vector2Int gridCount)
    {
        return x >= 0 && x < gridCount.x && z >= 0 && z < gridCount.y;
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