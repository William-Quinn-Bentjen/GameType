using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExampleGameTypeIntegration : ExtendedGameType
{
    [Header("Player Settings")]
    public JengaPlayer defaultPlayer;
    public List<PlayerInfo.PlayerData> playerData = new List<PlayerInfo.PlayerData>();
    public List<JengaPlayer> players = new List<JengaPlayer>();
    public struct DeathInfo
    {
        public Collision Collision;
        public Collider Other;
        public JengaPlayer Victim;
        public GameObject Weapon;
        public GameObject Killer;
    }
    public virtual void EvaluateDeath(DeathInfo deathInfo) { }
    public virtual void CreatePlayers()
    {
        if (defaultPlayer != null)
        {
            foreach (PlayerInfo.PlayerData data in playerData)
            {
                //instantiate player
                JengaPlayer player = Instantiate(defaultPlayer);
                data.SetPlayerFromData(player);
                if (player.team != null) player.team.Join(player);
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
}
