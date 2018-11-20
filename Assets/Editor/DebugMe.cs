using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class DebugMe : MonoBehaviour {
    public string[] files = new string[3] { "82-C# Game Type Script-NewGameTypeScript.cs.txt", "82-C# Team Data Script-NewTeamDataScript.cs.txt", "82-C# Team Script-NewTeamScript.cs.txt" };
    // Use this for initialization
    void Start () {
        string path = Path.GetDirectoryName(EditorApplication.applicationPath) + @"/Data/Resources/ScriptTemplates/";
        bool allFilesExist = true;
        for (int i = 0; i < files.Length && allFilesExist; i++)
        {
            if (File.Exists(path + files[i]) == false)
            {
                allFilesExist = false;
            }
        }
        Debug.Log(Path.GetDirectoryName(EditorApplication.applicationPath) + @"/Data/Resources/ScriptTemplates/");
        if (allFilesExist)
        {
            Debug.Log("All files are here");
        }
        else
        {
            Debug.Log("FILE MISSING");
        }
        
        

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
