using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Map), true)]
public class ScenePickerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var picker = target as Map;
        var oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(picker.path);

        serializedObject.Update();

        EditorGUI.BeginChangeCheck();
        var newScene = EditorGUILayout.ObjectField("Scene", oldScene, typeof(SceneAsset), false) as SceneAsset;

        if (EditorGUI.EndChangeCheck())
        {
            var newPath = AssetDatabase.GetAssetPath(newScene);
            var scenePathProperty = serializedObject.FindProperty("path");
            scenePathProperty.stringValue = newPath;
            picker.UpdateInfo();
        }
        GUI.enabled = false;
        var inBuild = EditorGUILayout.ToggleLeft("In Build", picker.InBuild);
        if (inBuild)
        {
            EditorGUILayout.LabelField("Scene name: " + picker.sceneName);
            EditorGUILayout.IntField("Build Index", picker.buildIndex);
            EditorGUILayout.LabelField("Path: " + picker.path);
        }
        else
        {
            GUI.enabled = true;
            if (GUILayout.Button("Add To Build"))
            {
                picker.AddToBuild();
            }
        }
        GUI.enabled = true;
        serializedObject.ApplyModifiedProperties();
    }
}