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
    public GameType GameType;
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
            if (GameType != null)
            {
                GameType.GameManager = this;
                GameType.BeginGame();
            }
        }
    }
    private void OnDestroy()
    {
        if (GameType != null)
        {
            GameType.EndGame();
            GameType = null;
        }

    }
}
