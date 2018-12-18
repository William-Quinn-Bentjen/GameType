using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    
    static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            InstantiateSelf();
            return instance;
        }
        set
        {
            instance = value;
        }
    }
    static void InstantiateSelf()
    {
        if (instance == null)
        {
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
            {
                instance = gm;
            }
            else
            {
                //create new
                GameObject newInstance = new GameObject("GameManager");
                instance = newInstance.AddComponent<GameManager>();
            }
        }
    }
    public Lobby lobby;
    public GameType GameType;
    public delegate void SceneLoad();
    public SceneLoad onLoadScene;
    //public WinUI winUI;
    //public List<Teams.TeamMember> players = new List<Teams.TeamMember>(); 
    // Use this for initialization
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            UnityAction<Scene, LoadSceneMode> unityAction = new UnityAction<Scene, LoadSceneMode>(LoadSceneListener);
            SceneManager.sceneLoaded += LoadSceneListener;
        }
    }

    private void OnDestroy()
    {
        Debug.LogWarning("Game Manager destroyed");
    }

    public void LoadSceneListener(Scene scene, LoadSceneMode loadSceneMode)
    {
        onLoadScene?.Invoke();
    }
    public void LoadScene(Scene scene)
    {
        List<UnityEditor.EditorBuildSettingsScene> scenes = new List<UnityEditor.EditorBuildSettingsScene>();

        //have some sort of for loop to cycle through scenes...
        string pathToScene = UnityEditor.AssetDatabase.GetAssetPath(mySceneAssets[i]);
        UnityEditor.EditorBuildSettingsScene scene = new UnityEditor.EditorBuildSettingsScene(pathToScene, true);
        scenes.Add(scene);

        //later on...
        UnityEditor.EditorBuildSettings.scenes = scenes.ToArray();
    }
    IEnumerator LoadSceneAsync(int buildNumber)
    {
        Debug.Log("Loading Scene");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(buildNumber);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        Debug.Log("Scene Loaded!");
    }
}
