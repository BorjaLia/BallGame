using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelEditorManager))]
public class LevelEditorManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LevelEditorManager manager = (LevelEditorManager)target;

        EditorGUILayout.Space(10);

        if (GUILayout.Button("Sync Grid Lists"))
        {
            if (manager.floorBuilder != null) Undo.RecordObject(manager.floorBuilder, "Sync Floor Grid");
            if (manager.wallBuilder != null) Undo.RecordObject(manager.wallBuilder, "Sync Wall Grid");

            manager.SyncGridLayouts();

            if (manager.floorBuilder != null) EditorUtility.SetDirty(manager.floorBuilder);
            if (manager.wallBuilder != null) EditorUtility.SetDirty(manager.wallBuilder);
        }

        if (GUILayout.Button("Rebuild All", GUILayout.Height(30)))
        {
            Undo.RegisterFullObjectHierarchyUndo(manager.TargetParent.gameObject, "Rebuild Level");
            manager.RebuildAll();
        }

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Rebuild Floor"))
        {
            Undo.RegisterFullObjectHierarchyUndo(manager.TargetParent.gameObject, "Rebuild Floor");
            manager.RebuildFloor();
        }
        if (GUILayout.Button("Rebuild Walls"))
        {
            Undo.RegisterFullObjectHierarchyUndo(manager.TargetParent.gameObject, "Rebuild Walls");
            manager.RebuildWalls();
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Generate Surrounding Walls"))
        {
            if (manager.wallBuilder != null)
            {
                Undo.RecordObject(manager.wallBuilder, "Generate Surrounding Walls");
                Undo.RegisterFullObjectHierarchyUndo(manager.TargetParent.gameObject, "Generate Surrounding Walls");
                manager.GenerateSurroundingWalls();
                EditorUtility.SetDirty(manager.wallBuilder);
            }
        }

        EditorGUILayout.Space(5);

        if (GUILayout.Button("Clear All"))
        {
            Undo.RegisterFullObjectHierarchyUndo(manager.TargetParent.gameObject, "Clear Level");
            manager.ClearAll();
        }
    }
}