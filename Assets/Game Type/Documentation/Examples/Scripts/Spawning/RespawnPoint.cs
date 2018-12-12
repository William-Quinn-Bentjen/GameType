using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Spawning
{
    public class RespawnPoint : SpawnPoint
    {
        public static Dictionary<Teams.Team, List<RespawnPoint>> RespawnPoints = new Dictionary<Teams.Team, List<RespawnPoint>>();
        public static List<RespawnPoint> NeutralRespawnPoints = new List<RespawnPoint>();
        public override void Activate()
        {
            base.Activate();
            if (neutral)
            {
                if (NeutralRespawnPoints.Contains(this) == false) NeutralRespawnPoints.Add(this);
            }
            else
            {
                foreach (Teams.Team team in whiteList)
                {
                    if (team != null)
                    {
                        if (RespawnPoints.ContainsKey(team))
                        {
                            RespawnPoints[team].Add(this);
                        }
                        else
                        {
                            RespawnPoints.Add(team, new List<RespawnPoint>(new RespawnPoint[1] { this }));
                        }
                    }
                }
            }
        }
        public override void Deactivate()
        {
            base.Deactivate();
            if (neutral)
            {
                if (NeutralRespawnPoints.Contains(this)) NeutralRespawnPoints.Remove(this);
            }
            else
            {
                foreach (Teams.Team team in whiteList)
                {
                    if (team != null)
                    {
                        if (RespawnPoints.ContainsKey(team))
                        {
                            RespawnPoints[team].Remove(this);
                        }
                        else
                        {
                            RespawnPoints.Add(team, new List<RespawnPoint>(new RespawnPoint[0]));
                        }
                    }
                }
            }
        }
        public static void ClearSpawnData()
        {
            RespawnPoints = new Dictionary<Teams.Team, List<RespawnPoint>>();
            NeutralRespawnPoints = new List<RespawnPoint>();
        }
        public static void GatherSpawnData()
        {
            ClearSpawnData();
            foreach (RespawnPoint spawn in FindObjectsOfType<RespawnPoint>())
            {
                if (spawn.active)
                {
                    spawn.Activate();
                }
            }
        }
        [ContextMenu("Clear RespawnPoint Data")]
        public void ClearRespawnPointsData()
        {
            ClearSpawnData();
        }
        [ContextMenu("Gather RespawnPoint Data")]
        public void GatherRespawnPointsData()
        {
            GatherSpawnData();
        }
        [ContextMenu("Debug RespawnPoint Data")]
        public void DebugSpawns()
        {
            Debug.Log("Debugging RespawnPoint Data");
            foreach (Teams.Team team in RespawnPoints.Keys)
            {
                if (RespawnPoints[team].Count > 0)
                {
                    Debug.Log(RespawnPoints[team].Count + " total # of RespawnPoints for team " + team.data.TeamName);
                }
            }
            Debug.Log(NeutralRespawnPoints.Count + " Neutral RespawnPoints");
        }
    }
}