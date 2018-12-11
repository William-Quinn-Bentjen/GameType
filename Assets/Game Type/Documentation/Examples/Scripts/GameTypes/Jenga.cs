using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Jenga", menuName = "GameType/Example/Jenga")]
public class Jenga : ExtendedWithRoundsGameType {

    public float totalTeamPreferenceWeight = 0;
    //teams
    [System.Serializable]
    public struct JengaTeams
    {
        public Teams.Team Survivors;
        public Teams.Team Demolisher;
        public Teams.Team Spectators;
    }
    //players and data
    [Header("Team Settings")]
    public JengaTeams teams = new JengaTeams();
    public List<PlayerInfo.PlayerData> playerData = new List<PlayerInfo.PlayerData>();
    public List<JengaPlayer> players = new List<JengaPlayer>();
    //spawn settings
    [Header("Spawn Settings")]
    public TeamWeights teamPrefenceWeights = new TeamWeights(1, 3, 5);
    public JengaPlayer defaultJengaPlayer;
    public Vector3 spawnOffset = Vector3.zero;
    [System.Serializable]
    public struct TeamWeights
    {
        [Range(0,100)]
        public float Neutral;
        [Range(0,100)]
        public float Survivors;
        [Range(0, 100)]
        public float Demolisher;
        public TeamWeights(float n, float s, float d)
        {
            Neutral = n;
            Survivors = s;
            Demolisher = d;
        }
    }
    // win conditions
    [Header("Game End Conditions")]
    [Tooltip("0 for no limit")]
    public float scoreToWin = 0; // 0 for no limit
    [Tooltip("0 for no limit")]
    [Range(0,999999)]
    public int maxRounds = 0; // 0 for no limit
    [Header("Score Settings")]
    // score 
    [Tooltip("Multiplier for being eliminated as a survivor (this * # of players currently eliminated)")]
    public float eliminatedMultiplier = 1;
    [Tooltip("Points for eliminating a survivor")]
    public float elimination = 1;
    public Dictionary<JengaPlayer, float> score = new Dictionary<JengaPlayer, float>();
    // bonus
    [Tooltip("Points for surviving a round")]
    public float survivedRoundBonus = 5;
    [Tooltip("Points for killing all survivors in a round")]
    public float acedBonus = 5;



    //utility
    [ContextMenu("Create Teams")]
    void GenerateTeams()
    {
        if (teams.Survivors == null || teams.Survivors.data == null)
        {
            teams.Survivors = CreateInstance<Teams.Team>();
            teams.Survivors.data = CreateInstance<Teams.TeamData>();
            teams.Survivors.data.TeamName = "Survivors";
            teams.Survivors.data.TeamColor = Color.blue;
        }
        if (teams.Spectators == null || teams.Spectators == null)
        {
            teams.Spectators = CreateInstance<Teams.Team>();
            teams.Spectators.data = CreateInstance<Teams.TeamData>();
            teams.Spectators.data.TeamName = "Spectators";
            teams.Spectators.data.TeamColor = Color.grey;
        }
        if (teams.Demolisher == null || teams.Demolisher.data == null)
        {
            teams.Demolisher = CreateInstance<Teams.Team>();
            teams.Demolisher.data = CreateInstance<Teams.TeamData>();
            teams.Demolisher.data.TeamName = "Demolisher";
            teams.Demolisher.data.TeamColor = Color.red;
        }
    }
    void CreatePlayers()
    {
        if (defaultJengaPlayer != null)
        {
            totalTeamPreferenceWeight = 0;
            foreach (PlayerInfo.PlayerData data in playerData)
            {
                //instantiate player
                JengaPlayer player = Instantiate(defaultJengaPlayer);
                data.SetPlayerFromData(player);
                players.Add(player);
                if (data.teamPreference == teams.Survivors)
                {
                    totalTeamPreferenceWeight += teamPrefenceWeights.Survivors;
                }
                else if (data.teamPreference == teams.Demolisher)
                {
                    totalTeamPreferenceWeight += teamPrefenceWeights.Demolisher;
                }
                else
                {
                    totalTeamPreferenceWeight += teamPrefenceWeights.Neutral;
                }
            }
        }
    }
    void AsignTeams()
    {
        float rand = Random.Range(0, totalTeamPreferenceWeight);
        bool demolisherSelected = false;
        for(int i = 0; i < players.Count; i++)
        {
            if (players[i] != null)
            {
                if (rand <= 0 && demolisherSelected == false)
                {
                    //demolisher joined
                    demolisherSelected = true;
                    teams.Demolisher.Join(players[i]);
                }
                else
                {
                    teams.Survivors.Join(players[i]);
                }
                if (players[i].team == teams.Demolisher)
                {
                    rand -= teamPrefenceWeights.Demolisher;
                }
                else if (players[i].team == teams.Survivors)
                {
                    rand -= teamPrefenceWeights.Survivors;
                }
                else
                {
                    rand -= teamPrefenceWeights.Neutral;
                }
            }
        }
    }
    void SetupScores()
    {
        score.Clear();
        foreach (JengaPlayer player in players)
        {
            score.Add(player, 0);
        }
    }
    void AddScore(JengaPlayer player, float scoreToAdd = 0)
    {
        if (score.ContainsKey(player) == false) score.Add(player, 0);
        score[player] += scoreToAdd;
        if (score[player] >= scoreToWin)
        {
            EndGame();
        }
    }
    public struct JengaDeathInfo
    {
        public Collision Collision;
        public Collider Other;
        public JengaPlayer Victim;
        public GameObject Weapon;
        public GameObject Killer;
    }
    public void EvaluateDeath(JengaDeathInfo deathInfo)
    {
        if (deathInfo.Victim.team == teams.Survivors)
        {
            teams.Spectators.Join(deathInfo.Victim);
            AddScore(deathInfo.Victim, teams.Spectators.members.Count * eliminatedMultiplier);

            if (teams.Demolisher.members.Count > 0)
            {
                JengaPlayer demolisher = teams.Demolisher.members[0] as JengaPlayer;
                if (demolisher != null)
                {
                    AddScore(demolisher, elimination);
                }
                else
                {
                    Debug.LogError("demolisher's null, aborting game!");
                    GameState.Key = ExampleGameState.Aborted;
                    LeaveMap();
                }
            }
            else
            {
                Debug.LogError("No demolishers, aborting game!");
                GameState.Key = ExampleGameState.Aborted;
                LeaveMap();
            }
        }
        else if (deathInfo.Victim.team == teams.Demolisher)
        {
            Debug.LogWarning("Demolisher should never die, ending round!");
            EndRound();
        }
    }
    

    //overrided 
    public override void OnEnable()
    {
        base.OnEnable();
        GenerateTeams();
    }
    //lobby         assign/join teams (create playerdata)                          
    public override bool BeginGame()
    {
        if (CanStart())
        {
            StartGame();
            return true;
        }
        return false;
    }
    public override bool CanStart()
    {
        return (playerData.Count > 1);
    }
    public override void EnterMap()
    {
        base.EnterMap();
        Spawning.SpawnManager.ClearSpawnData();
        Spawning.SpawnManager.GatherSpawnData();
        CreatePlayers();
        SetupScores();
    }
    public override void StartRound()
    {
        AsignTeams();
        Spawning.SpawnManager.InitalSpawn(players, spawnOffset);
        base.StartRound();
    }
    public override void EndRound()
    {
        if (teams.Survivors.members.Count > 0)
        {
            if (RoundTimer.TimeLimit >= 0 && RoundTimer.Time >= RoundTimer.TimeLimit)
            {
                foreach (Teams.TeamMember member in teams.Survivors.members)
                {
                    JengaPlayer survivor = member as JengaPlayer;
                    if (survivor != null)
                    {
                        AddScore(survivor, survivedRoundBonus);
                    }
                }
            }
        }
        else
        {
            JengaPlayer demolisher = teams.Demolisher.members[0] as JengaPlayer;
            if (demolisher != null)
            {
                AddScore(demolisher, elimination);
            }
        }
        base.EndRound();
        if (maxRounds > 0 && CurrentRound >= maxRounds)
        {
            EndGame();
        }
        else
        {
            if (GameState.Key != ExampleGameState.Ending) StartRound();
        }
    }
    public override void EndGame()
    {
        base.EndGame();
        LeaveMap();
    }
    public override void LeaveMap()
    {
        base.LeaveMap();
        //to lobby
    }
}
