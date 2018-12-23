using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerData
{
    void SetPlayerData(List<PlayerInfo.PlayerData> newPlayerData);
    List<PlayerInfo.PlayerData> GetPlayerData();
}
public class ExampleGameTypeIntegration : ExtendedGameType, IPlayerData
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
        public DeathInfo(JengaPlayer victim, Collision collision)
        {
            Victim = victim;
            Collision = collision;
            Other = Collision.collider;
            Killer = Collision.gameObject;
            Weapon = collision.gameObject;
            ExampleBullet bullet = Collision.gameObject.GetComponent<ExampleBullet>();
            if (bullet != null)
            {
                Weapon = bullet.Gun;
                Killer = bullet.Killer.gameObject;
            }
        }
    }
    public virtual void EvaluateDeath(DeathInfo deathInfo) { }
    public virtual void CreatePlayers()
    {
        if (defaultPlayer != null)
        {
            players = new List<JengaPlayer>();
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
    public override bool BeginGame()
    {
        //map tells gametype when the map is loaded to call EnterMap() once it's the active scene
        return CanStart();
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
        Spawning.SpawnManager.InitalSpawn(players);
        StartGame();
    }
    public override void EndGame()
    {
        base.EndGame();
        LeaveMap();
    }

    public void SetPlayerData(List<PlayerInfo.PlayerData> newPlayerData)
    {
        playerData = newPlayerData;
    }

    public List<PlayerInfo.PlayerData> GetPlayerData()
    {
        return playerData;
    }
}
