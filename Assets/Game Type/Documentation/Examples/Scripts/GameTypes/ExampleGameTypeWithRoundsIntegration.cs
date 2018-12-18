using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExampleGameTypeWithRoundsIntegration : ExtendedWithRoundsGameType, ExampleInterface.IPlayerData
{
    public List<PlayerInfo.PlayerData> playerData = new List<PlayerInfo.PlayerData>();
    public List<Teams.TeamMember> players = new List<Teams.TeamMember>();

    // Override 
    public override bool BeginGame()
    {
        if (CanStart())
        {
            // Entermap when done loading
            GameManager.Instance.onLoadScene += EnterMap;
            return true;
        }
        return false;
    }
    public override bool CanStart()
    {
        return (playerData != null && players != null &&
            playerData.Count > 0 &&
            playerData.Count == players.Count);
    }
    public override void EnterMap()
    {
        GameManager.Instance.onLoadScene -= EnterMap;
        GameManager.Instance.lobby.isEnabled = false;
        base.EnterMap();
        Spawning.SpawnManager.ClearSpawnData();
        Spawning.SpawnManager.GatherSpawnData();
        StartRound();
    }
    public override void EndGame()
    {
        base.EndGame();
        LeaveMap();
    }
    public override void LeaveMap()
    {
        base.LeaveMap();
        GameManager.Instance.lobby.playersDisplay.RemoveAllPlayers();
        foreach (PlayerInfo.PlayerData data in playerData)
        {
            GameManager.Instance.lobby.playersDisplay.AddPlayer(data);
        }
        GameManager.Instance.lobby.isEnabled = true;
    }

    // Death
    public virtual void EvaluateDeath(ExampleGameTypeIntegration.DeathInfo deathInfo)
    {


    }

    // Player Interface
    public virtual List<Teams.TeamMember> GetPlayers()
    {
        return players;
    }
    public virtual void SetPlayers(List<Teams.TeamMember> playersList)
    {
        players = playersList;
    }

    // Player Data Interface
    public virtual List<PlayerInfo.PlayerData> GetPlayerData()
    {
        return playerData;
    }
    public virtual void SetPlayerData(List<PlayerInfo.PlayerData> newPlayerData)
    {
        playerData = newPlayerData;
    }
}
