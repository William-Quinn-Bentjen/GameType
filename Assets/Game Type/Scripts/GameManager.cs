using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
#if UNITY_EDITOR
            //find
            if (FindObjectOfType<GameManager>() != null)
            {
                Debug.LogWarning("GameManager exists in scene but not hooked up as instance");
                instance = FindObjectOfType<GameManager>();
            }
            else
            {
                //create new
                GameObject newInstance = new GameObject("GameManager");
                instance = newInstance.AddComponent<GameManager>();
            }
#else
            //create new
            GameObject newInstance = new GameObject("GameManager");
            instance = newInstance.AddComponent<GameManager>();
#endif
        }
    }
    public GameType GameType;
    // Use this for initialization
    void Awake () {
        if (instance == null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            if (GameType != null)
            {
                GameType.GameBegin();
            }
        }
	}
    private void OnDestroy()
    {
        if (GameType != null)
        {
            GameType.GameOver();
            GameType = null;
        }
        
    }
}
