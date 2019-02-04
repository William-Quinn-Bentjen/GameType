using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExampleGameTypeWithRoundsIntegration : ExtendedWithRoundsGameType
{
    [Header("Player Settings")]
    public JengaPlayer defaultPlayer;
    public List<PlayerInfo.PlayerData> playerData = new List<PlayerInfo.PlayerData>();
    public List<JengaPlayer> players = new List<JengaPlayer>();
    [Header("Win Conditions")]
    [Tooltip("0 for no limit")]
    [Range(0, 999999)]
    public int maxRounds = 0; // 0 for no limit
    public virtual void EvaluateDeath(ExampleGameTypeIntegration.DeathInfo deathInfo) { }
    public virtual void CreatePlayers()
    {
        if (defaultPlayer != null)
        {
            foreach (PlayerInfo.PlayerData data in playerData)
            {
                //instantiate player
                JengaPlayer player = Instantiate(defaultPlayer);
                data.SetPlayerFromData(player);
                players.Add(player);
            }
        }
    }
    public override bool CanStart()
    {
        return playerData.Count > 1;
    }
    public override void EnterMap()
    {
        base.EnterMap();
        Spawning.SpawnManager.ClearSpawnData();
        Spawning.SpawnManager.GatherSpawnData();
        CreatePlayers();
        StartGame();
    }
    public override void EndGame()
    {
        base.EndGame();
        LeaveMap();
    }

    public override void StartRound()
    {
        Spawning.SpawnManager.InitalSpawn(players);
        base.StartRound();
    }
    public override void EndRound()
    {
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
}
