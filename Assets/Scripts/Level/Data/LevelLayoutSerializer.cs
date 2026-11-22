using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelLayoutSerializer : MonoBehaviour
{
    [Header("References")]
    public LevelEditorManager levelManager;

    [Header("Layout Asset")]
    [Tooltip("The ScriptableObject to load from or overwrite when saving.")]
    public LevelLayoutData layoutAsset;

    [Tooltip("Automatically rebuild the scene geometry after loading a layout.")]
    public bool rebuildOnLoad = true;

    private void Reset()
    {
        if (levelManager == null)
            levelManager = GetComponent<LevelEditorManager>();
    }

    public void SaveToAsset(LevelLayoutData target)
    {
        if (levelManager == null || target == null)
        {
            Debug.LogWarning("[LevelLayoutSerializer] Assign both LevelManager and LayoutAsset before saving.");
            return;
        }

        target.totalFloorSize = levelManager.totalFloorSize;
        target.gridCount = levelManager.gridCount;
        target.autoScaleTiles = levelManager.autoScaleTiles;

        if (levelManager.floorBuilder != null)
        {
            target.floorVerticalOffset = levelManager.floorBuilder.verticalOffset;
            target.floorDefaultTile = levelManager.floorBuilder.defaultTileType;
            target.floorRows = CloneRows(levelManager.floorBuilder.gridRows);
        }

        if (levelManager.wallBuilder != null)
        {
            target.wallVerticalOffset = levelManager.wallBuilder.verticalOffset;
            target.wallDefaultTile = levelManager.wallBuilder.defaultTileType;
            target.wallRows = CloneRows(levelManager.wallBuilder.gridRows);
        }

#if UNITY_EDITOR
        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();
#endif
        Debug.Log($"[LevelLayoutSerializer] Saved layout to '{target.name}'.");
    }

    public void LoadFromAsset(LevelLayoutData source)
    {
        if (levelManager == null || source == null)
        {
            Debug.LogWarning("[LevelLayoutSerializer] Assign both LevelManager and LayoutAsset before loading.");
            return;
        }

        levelManager.totalFloorSize = source.totalFloorSize;
        levelManager.gridCount = source.gridCount;
        levelManager.autoScaleTiles = source.autoScaleTiles;

        if (levelManager.floorBuilder != null)
        {
            levelManager.floorBuilder.verticalOffset = source.floorVerticalOffset;
            levelManager.floorBuilder.defaultTileType = source.floorDefaultTile;
            levelManager.floorBuilder.gridRows = CloneRows(source.floorRows);
        }

        if (levelManager.wallBuilder != null)
        {
            levelManager.wallBuilder.verticalOffset = source.wallVerticalOffset;
            levelManager.wallBuilder.defaultTileType = source.wallDefaultTile;
            levelManager.wallBuilder.gridRows = CloneRows(source.wallRows);
        }

        if (rebuildOnLoad)
        {
            levelManager.RebuildAll();
        }

        Debug.Log($"[LevelLayoutSerializer] Loaded layout from '{source.name}'.");
    }

    private List<GridRow<T>> CloneRows<T>(List<GridRow<T>> sourceRows) where T : Enum
    {
        var copy = new List<GridRow<T>>(sourceRows.Count);
        foreach (var row in sourceRows)
        {
            copy.Add(new GridRow<T>
            {
                rowLabel = row.rowLabel,
                tiles = new List<T>(row.tiles)
            });
        }
        return copy;
    }
}