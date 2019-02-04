using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

[CreateAssetMenu(fileName = "Map", menuName = "GameType/Example/Map")]
public class Map : ScriptableObject
{
    // Filled out in inspector
    public string path = "";
    // Updated later
#if UNITY_EDITOR
    public SceneAsset sceneAsset = null;
#endif
    public int buildIndex = -1;
    public string sceneName = "";
    public delegate void OnLoadedDelegate();
    public OnLoadedDelegate onLoaded;
    public bool InBuild
    {
        get
        {
            UpdateInfo();
            return (buildIndex != -1);
        }
    }
    public void UpdateInfo()
    {
#if UNITY_EDITOR
        sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
#endif
        // Get scenes in build
        
        buildIndex = -1;
        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            if (path == scenePath)
            {
                buildIndex = i;
                int lastSlash = scenePath.LastIndexOf("/");
                sceneName = scenePath.Substring(lastSlash + 1, scenePath.LastIndexOf(".") - lastSlash - 1);
            }
        }
    }
    public void AddToBuild()
    {
#if UNITY_EDITOR
        // Get scenes in build
        List<string> scenesInBuild = new List<string>();
        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            scenesInBuild.Add(scenePath);
        }
        // Check if in build
        if (scenesInBuild.Contains(path) == false)
        {
            // Add to build
            List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes)
            {
                new EditorBuildSettingsScene(path, true)
            };
            EditorBuildSettings.scenes = scenes.ToArray();
            UpdateInfo();
        }
#endif
    }
    public void Load()
    {
        if (!InBuild && path.Length > 1)
        {
            AddToBuild();
        }
        GameManager.Instance.StartCoroutine(LoadSceneAsync());
    }
    IEnumerator LoadSceneAsync()
    {
        Debug.Log("Loading Map");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(buildIndex));
        Debug.Log("Map Loaded!");
        onLoaded?.Invoke();
    }
    public void Unload()
    {
        int sceneCount = SceneManager.sceneCount;
        for(int i = 0; i < sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name == "Lobby")
            {
                SceneManager.SetActiveScene(scene);
            }
            else if (scene.buildIndex == buildIndex)
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }
    }
}
