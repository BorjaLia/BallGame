using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(LevelLayoutSerializer))]
public class LevelLayoutSerializerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LevelLayoutSerializer serializer = (LevelLayoutSerializer)target;
        LevelEditorManager manager = serializer.levelManager;

        EditorGUILayout.Space(10);

        GUI.enabled = manager != null && serializer.layoutAsset != null;

        if (GUILayout.Button("Load Layout", GUILayout.Height(28)))
        {
            RecordUndoForLoad(manager);
            serializer.LoadFromAsset(serializer.layoutAsset);
            MarkDirtyForLoad(manager);
        }

        if (GUILayout.Button("Save (Overwrite Assigned Asset)", GUILayout.Height(28)))
        {
            Undo.RecordObject(serializer.layoutAsset, "Overwrite Level Layout");
            serializer.SaveToAsset(serializer.layoutAsset);
        }

        GUI.enabled = manager != null;

        EditorGUILayout.Space(5);

        if (GUILayout.Button("Save As New Asset..."))
        {
            string path = EditorUtility.SaveFilePanelInProject(
                "Save Level Layout",
                "NewLevelLayout",
                "asset",
                "Choose a location to save the level layout."
            );

            if (!string.IsNullOrEmpty(path))
            {
                LevelLayoutData newAsset = ScriptableObject.CreateInstance<LevelLayoutData>();
                AssetDatabase.CreateAsset(newAsset, path);

                serializer.SaveToAsset(newAsset);

                Undo.RecordObject(serializer, "Assign New Layout Asset");
                serializer.layoutAsset = newAsset;
                EditorUtility.SetDirty(serializer);
                EditorGUIUtility.PingObject(newAsset);
            }
        }

        GUI.enabled = true;
    }

    private void RecordUndoForLoad(LevelEditorManager manager)
    {
        Undo.RecordObject(manager, "Load Level Layout");
        if (manager.floorBuilder != null) Undo.RecordObject(manager.floorBuilder, "Load Floor Layout");
        if (manager.wallBuilder != null) Undo.RecordObject(manager.wallBuilder, "Load Wall Layout");
        Undo.RegisterFullObjectHierarchyUndo(manager.TargetParent.gameObject, "Load Level Layout");
    }

    private void MarkDirtyForLoad(LevelEditorManager manager)
    {
        EditorUtility.SetDirty(manager);
        if (manager.floorBuilder != null) EditorUtility.SetDirty(manager.floorBuilder);
        if (manager.wallBuilder != null) EditorUtility.SetDirty(manager.wallBuilder);
    }
}
#endif