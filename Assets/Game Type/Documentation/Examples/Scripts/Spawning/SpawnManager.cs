using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Spawning
{
    public class SpawnManager
    {
        public static void GatherSpawnData()
        {
            InitalSpawnPoint.GatherSpawnData();
            RespawnPoint.GatherSpawnData();
        }
        public static void ClearSpawnData()
        {
            InitalSpawnPoint.ClearSpawnData();
            RespawnPoint.ClearSpawnData();
        }
        public static List<SpawnPoint> GetRespawnPoints(Teams.Team team = null)
        {
            List<SpawnPoint> possibleSpawns = new List<SpawnPoint>();
            if (team != null)
            {
                if (RespawnPoint.RespawnPoints.ContainsKey(team))
                {
                    possibleSpawns.AddRange(RespawnPoint.RespawnPoints[team]);
                }
                else
                {
                    RespawnPoint.RespawnPoints.Add(team, new List<RespawnPoint>());
                }
            }
            if (RespawnPoint.NeutralRespawnPoints.Count > 0)
            {
                possibleSpawns.AddRange(RespawnPoint.NeutralRespawnPoints);
            }
            return possibleSpawns;
        }
        public static SpawnPoint GetRespawnPoint(Teams.Team team = null)
        {
            List<SpawnPoint> spawnPoints = GetRespawnPoints(team);
            if (spawnPoints.Count > 0)
            {
                return spawnPoints[Random.Range(0, spawnPoints.Count - 1)];
            }
            else
            {
                return null;
            }
        }
        public static List<SpawnPoint> GetInitalSpawnPoints(Teams.Team team = null)
        {
            List<SpawnPoint> possibleSpawns = new List<SpawnPoint>();
            if (team != null)
            {
                if (InitalSpawnPoint.InitalSpawnPoints.ContainsKey(team))
                {
                    possibleSpawns.AddRange(InitalSpawnPoint.InitalSpawnPoints[team]);
                }
                else
                {
                    InitalSpawnPoint.InitalSpawnPoints.Add(team, new List<InitalSpawnPoint>());
                }
            }
            if (InitalSpawnPoint.NeutralInitalSpawnPoints.Count > 0)
            {
                possibleSpawns.AddRange(InitalSpawnPoint.NeutralInitalSpawnPoints);
            }
            return possibleSpawns;
        }
        public static SpawnPoint GetInitalSpawnPoint(Teams.Team team = null, bool allowRespawnIfNoInital = true)
        {
            List<SpawnPoint> spawnPoints = GetInitalSpawnPoints(team);
            if (spawnPoints.Count > 0)
            {
                return spawnPoints[Random.Range(0, spawnPoints.Count - 1)];
            }
            else if (allowRespawnIfNoInital)
            {
                spawnPoints = GetRespawnPoints();
                return spawnPoints[Random.Range(0, spawnPoints.Count - 1)];
            }
            else
            {
                return null;
            }
        }
    }
}