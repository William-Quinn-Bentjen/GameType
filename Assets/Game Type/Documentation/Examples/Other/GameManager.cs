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
    public WinUI winUI;
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
                if (GameType.BeginGame())
                {
                    GameType.StartGame();
                    Debug.Log("Slayer Game Started!");
                }
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
    public void SetWinnerText(Teams.Team winner)
    {
        if (winUI != null)
        {
            winUI.SetWinnerText(winner);
        }
    }
}
