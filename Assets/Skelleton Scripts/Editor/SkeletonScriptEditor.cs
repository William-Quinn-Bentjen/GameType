using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEditor;
    [CustomEditor(typeof(SkeletonScripts.SkeletonScript), true)]
    public class SkeletonScriptEditor : Editor
    {
        public static Vector2 scrollPosition;
        public static string lines;
        // Draw the property inside the given rect
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            SkeletonScripts.SkeletonScript skeletonScript = (serializedObject.targetObject as SkeletonScripts.SkeletonScript);
            Undo.RecordObject(skeletonScript, "Skeleton script change properties");
            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("rawFile"), new GUIContent("Raw File", "this gets the extention and lines from a file"));
                if (skeletonScript.rawFile == null)
                {
                    GUI.enabled = false;
                }
                if (GUILayout.Button(GUI.enabled ? "Update Info From File" : "Missing Raw File to Update From"))
                {
                    skeletonScript.UpdateInfo();
                    EditorUtility.SetDirty(skeletonScript);
                }
            }
            GUI.enabled = true;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("extention"), new GUIContent("Extention", "the extention the new files will be (like .cs or .cg)"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("lines"), GUILayout.ExpandHeight(true));

            if (GUILayout.Button("Use in New Script"))
            {
                CreateSkeletonScriptWindow.skeletonScript = skeletonScript;
                CreateSkeletonScriptWindow.ShowWindow();
            }
            serializedObject.ApplyModifiedProperties();
        }
    }