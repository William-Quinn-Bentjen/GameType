
using System.IO;
using UnityEngine;
using UnityEditor;

public class EditorSupport : EditorWindow
{
    /*
    static string[] files = new string[3] 
    {
        "82-C# Game Type Script-NewGameTypeScript.cs.txt",
        "82-C# Team Data Script-NewTeamDataScript.cs.txt",
        "82-C# Team Script-NewTeamScript.cs.txt"
    };
    static string path = "";//Path.GetDirectoryName(EditorApplication.applicationPath) + @"/Data/Resources/ScriptTemplates/";
    static bool installedSupport;
    static bool devMode;
    public Teams.Base.BaseTeam team;

    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/Game Type Editor Support")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        EditorSupport window = (EditorSupport)EditorWindow.GetWindow(typeof(EditorSupport));
        window.Show();
    }

    void OnGUI()
    {
        GUI.enabled = true;
        GUILayout.Label("Game Type Editor Support", EditorStyles.boldLabel);
        else
        {
            GUI.enabled = false;
            installedSupport = EditorGUILayout.ToggleLeft("Editor Support Enabled", AllFilesExist());
            GUI.enabled = (!installedSupport);
            if (GUILayout.Button("Install Support"))
            {
                Install();
            }
            GUI.enabled = installedSupport;
            if (GUILayout.Button("Uninstall Support"))
            {
                Uninstall();
            }
        }
        


    }
    bool AllFilesExist()
    {
        bool allFilesExist = true;
        for (int i = 0; i < files.Length && allFilesExist; i++)
        {
            if (File.Exists(path + files[i]) == false)
            {
                return false;
            }
        }
        return true;
    }

    void Install()
    {
       //place files at path
    }
    void Uninstall()
    {
        //remove files from path
    }
    */
}
