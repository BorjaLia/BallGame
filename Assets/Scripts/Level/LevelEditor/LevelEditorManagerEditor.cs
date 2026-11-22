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

        EditorGUILayout.Space(5);

        if (GUILayout.Button("Clear All"))
        {
            Undo.RegisterFullObjectHierarchyUndo(manager.TargetParent.gameObject, "Clear Level");
            manager.ClearAll();
        }
    }
}