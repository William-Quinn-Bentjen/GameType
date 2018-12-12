using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JengaGameManager : MonoBehaviour {

    static JengaGameManager instance = null;
    public static JengaGameManager Instance
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
            JengaGameManager gm = FindObjectOfType<JengaGameManager>();
            if (gm != null)
            {
                instance = gm;
            }
            else
            {
                //create new
                GameObject newInstance = new GameObject("JengaGameManager");
                instance = newInstance.AddComponent<JengaGameManager>();
            }
        }
    }
    public Jenga GameType;
    private void Awake()
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
                    Debug.Log(GameType.name + " Game Started!");
                }
            }
        }
    }
}
