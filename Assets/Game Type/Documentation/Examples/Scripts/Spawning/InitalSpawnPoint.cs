using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Spawning
{
    public class InitalSpawnPoint : SpawnPoint
    {
        public static Dictionary<Teams.Team, List<InitalSpawnPoint>> InitalSpawnPoints = new Dictionary<Teams.Team, List<InitalSpawnPoint>>();
        public static List<InitalSpawnPoint> NeutralInitalSpawnPoints = new List<InitalSpawnPoint>();
        public override void Activate()
        {
            base.Activate();
            if (neutral)
            {
                if (NeutralInitalSpawnPoints.Contains(this) == false) NeutralInitalSpawnPoints.Add(this);
            }
            else
            {
                foreach (Teams.Team team in whiteList)
                {
                    if (team != null)
                    {
                        if (InitalSpawnPoints.ContainsKey(team))
                        {
                            InitalSpawnPoints[team].Add(this);
                        }
                        else
                        {
                            InitalSpawnPoints.Add(team, new List<InitalSpawnPoint>(new InitalSpawnPoint[1] { this }));
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
                if (NeutralInitalSpawnPoints.Contains(this)) NeutralInitalSpawnPoints.Remove(this);
            }
            else
            {
                foreach (Teams.Team team in whiteList)
                {
                    if (team != null)
                    {
                        if (InitalSpawnPoints.ContainsKey(team))
                        {
                            InitalSpawnPoints[team].Remove(this);
                        }
                        else
                        {
                            InitalSpawnPoints.Add(team, new List<InitalSpawnPoint>(new InitalSpawnPoint[0]));
                        }
                    }
                }
            }
        }
        public static void ClearSpawnData()
        {
            InitalSpawnPoints = new Dictionary<Teams.Team, List<InitalSpawnPoint>>();
            NeutralInitalSpawnPoints = new List<InitalSpawnPoint>();
        }
        public static void GatherSpawnData()
        {
            ClearSpawnData();
            foreach (InitalSpawnPoint spawn in FindObjectsOfType<InitalSpawnPoint>())
            {
                if (spawn.active)
                {
                    spawn.Activate();
                }
            }
        }
        [ContextMenu("Clear InitalSpawnPoint Data")]
        public void ClearRespawnPointsData()
        {
            ClearSpawnData();
        }

        [ContextMenu("Gather InialSpawnPoint Data")]
        public void GatherInitalSpawnPointsData()
        {
            GatherSpawnData();
        }
        [ContextMenu("Debug InitalSpawnPoint Data")]
        public void DebugSpawns()
        {
            Debug.Log("Debugging InitalSpawnPoint Data");
            foreach (Teams.Team team in InitalSpawnPoints.Keys)
            {
                if (InitalSpawnPoints[team].Count > 0)
                {
                    Debug.Log(InitalSpawnPoints[team].Count + " total # of InitalSpawnPoints for team " + team.data.TeamName);
                }
            }
            Debug.Log(NeutralInitalSpawnPoints.Count + " Neutral InitalSpawnPoints");
        }
    }
}