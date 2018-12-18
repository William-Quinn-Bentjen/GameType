using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public Map map;
    public GameType GameType;
    public WinUI winUI;
    public List<Teams.TeamMember> players = new List<Teams.TeamMember>(); 
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
        }
    }
    private void OnDestroy()
    {
        if (GameType != null)
        {
            GameType.EndGame();
            if (GameType is Infection)
            {
                Infection inf = (GameType as Infection);
                inf.InfectedTeam.KickAll();
                inf.SurvivorTeam.KickAll();
            }
            if (GameType is TeamSlayer)
            {
                TeamSlayer slayer = (GameType as TeamSlayer);
                foreach (Teams.Team team in slayer.score.Keys)
                {
                    if (team != null)
                    {
                        team.KickAll();
                    }
                }
                slayer.score.Clear();
            }
            //GameType = null;
        }

    }
}
