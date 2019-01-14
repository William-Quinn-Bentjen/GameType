using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
class CreateSkeletonScriptWindow : EditorWindow
{
    [MenuItem("Assets/Create/Skeleton Script/Create Skeleton Script", priority = 0)]
    public static void ShowWindow()
    {
        GetWindow(typeof(CreateSkeletonScriptWindow));
    }
    public static SkeletonScripts.SkeletonScript skeletonScript;
    public static SkeletonScripts.SkeletonScriptReplace skeletonScriptReplace;
    public static string path;
    public static string nameOfFile;
    public static bool displayPreview;
    public static string previewContent = "";
    public static string previewPath = "";
    public static string error = "";
    void OnGUI()
    {
        // The actual window code goes here
        skeletonScript = EditorGUILayout.ObjectField("Skeleton Script", skeletonScript, typeof(SkeletonScripts.SkeletonScript), false) as SkeletonScripts.SkeletonScript;
        skeletonScriptReplace = EditorGUILayout.ObjectField("Skeleton Script Replace", skeletonScriptReplace, typeof(SkeletonScripts.SkeletonScriptReplace), false) as SkeletonScripts.SkeletonScriptReplace;
        if (skeletonScript != null)
        {
            GUI.enabled = false;
            foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
            {
                path = AssetDatabase.GetAssetPath(obj);
                if (File.Exists(path))
                {
                    path = Path.GetDirectoryName(path);
                    break;
                }
            }
            EditorGUILayout.TextField(new GUIContent("Directory", "Select a file in the \"Project\" window to set Directory"), path);
            GUI.enabled = true;
            nameOfFile = EditorGUILayout.TextField("Name", nameOfFile);
            //if (path != null && path[path.Length - 1] == '\\') path.Remove(path.Length - 1);
            //path += "\\";
            previewPath = path + "\\" + nameOfFile + skeletonScript.extention;
            if (path != null && nameOfFile != null && !File.Exists(previewPath))
            {
                displayPreview = EditorGUILayout.ToggleLeft("Display Preview", displayPreview);
                if (PathValidCheck(previewPath))
                {

                    error = "";
                }
                else
                {
                    error = "Can't save because of invalid path or name";
                }
                if (error == "")
                {
                    if (GUILayout.Button("Create Skeleton Script"))
                    {
                        skeletonScript.CreateFile(path, skeletonScriptReplace, nameOfFile);
                        AssetDatabase.Refresh();
                    }
                }
                else
                {
                    GUI.enabled = false;
                    EditorGUILayout.TextField(error);
                    GUI.enabled = true;
                }

                if (displayPreview)
                {
                    if (GUILayout.Button("Refresh Preview"))
                    {
                        previewContent = skeletonScript.GetPreview(skeletonScriptReplace, nameOfFile);
                        previewPath = path + "\\" + nameOfFile + skeletonScript.extention;
                    }
                    GUI.enabled = false;
                    EditorGUILayout.LabelField(new GUIContent(previewPath, "Where the Script will save to"));
                    EditorGUILayout.TextArea(previewContent, GUILayout.Height(position.height - 30));
                    //EditorGUILayout.EndScrollView();
                    GUI.enabled = true;
                }
            }
        }
        
    }
    public bool PathValidCheck(string path)
    {
        string file = path + ".tmp";
        try
        {
            if (File.Exists(path)) return false;
            string fileName = Path.GetFileNameWithoutExtension(path);
            if (fileName != null && fileName.Length > 0)
            {
                char c = fileName[0];
                if (/*c == '_' ||*/ (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
                {
                    using (File.Create(file)) { }
                    File.Delete(file);
                    return true;
                }
            }
        }
        catch
        {
            return false;
        }
        return false;
    }
}
