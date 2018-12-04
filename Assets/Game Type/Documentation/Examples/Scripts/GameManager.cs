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
            if (GameType != null)
            {
                GameType.GameManager = this;
                GetPlayers();
                if (GameType is GameTypes.Interfaces.IPlayers) (GameType as GameTypes.Interfaces.IPlayers).SetPlayers(players);
                if (GameType.)
                if (GameType.BeginGame())
                {
                    GameType.StartGame();
                    Debug.Log(GameType.name + " Game Started!");
                }
            }
            else
            {
                foreach(Teams.Team team in Resources.FindObjectsOfTypeAll<Teams.Team>())
                {
                    team.KickAll();
                }
            }
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
    public void SetWinnerText(Teams.Team winner)
    {
        if (winUI != null)
        {
            winUI.SetWinnerText(winner);
        }
    }
    [ContextMenu("Get Players From Scene")]
    public void GetPlayers()
    {
        players = new List<Teams.TeamMember>(FindObjectsOfType<Teams.TeamMember>());
    }
}
