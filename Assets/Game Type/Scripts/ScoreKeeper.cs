using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointKeeper : ScriptableObject {
    /// <summary>
    /// the dictionary that actually holds all the team and point info
    /// </summary>
    private Dictionary<Teams.Base.BaseTeam, float> pointKeeper = new Dictionary<Teams.Base.BaseTeam, float>();
    /// <summary>
    /// Used to get all the teams the point keeper knows about
    /// </summary>
    /// <returns>A list of all teams the point keeper know about</returns>
    public virtual List<Teams.Base.BaseTeam> GetAllTeams()
    {
        List<Teams.Base.BaseTeam> retVal = new List<Teams.Base.BaseTeam>();
        foreach(Teams.Base.BaseTeam team in pointKeeper.Keys)
        {
            retVal.Add(team);
        }
        return retVal;
    }
    public virtual void ClearAllTeams()
    {
        pointKeeper.Clear();
    }
    /// <summary>
    /// Removes a team and their point from the pointkeeper
    /// </summary>
    /// <param name="team">Team to be removed</param>
    public virtual void ClearTeam(Teams.Base.BaseTeam team)
    {
        if (pointKeeper.ContainsKey(team))
        {
            pointKeeper.Remove(team);
        }
    }
    /// <summary>
    /// Sets a team's points (if a team doesn't exist it will be created)
    /// </summary>
    /// <param name="team">Target team</param>
    /// <param name="point">Points to set</param>
    public virtual void SetPoints(Teams.Base.BaseTeam team, float point = 0)
    {
        if (pointKeeper.ContainsKey(team))
        {
            pointKeeper[team] = point;
        }
        else
        {
            pointKeeper.Add(team, point);
        }
    }
    /// <summary>
    /// Used to add points to a team (if a team doesn't exist it will be created)
    /// </summary>
    /// <param name="teamNumber">the team that pointd</param>
    /// <param name="pointToAdd">the point that should be added to the team</param>
    public virtual void AddPoints(Teams.Base.BaseTeam team, float pointToAdd)
    {
        if (pointKeeper.ContainsKey(team))
        {
            //add to value
            pointKeeper[team] += pointToAdd;
        }
        else
        {
            //create key
            pointKeeper.Add(team, pointToAdd);
        }
    }
    /// <summary>
    /// Check if a team exists within the pointkeeper
    /// </summary>
    /// <param name="teamNumber">the team number to check</param>
    /// <returns>true if the team exists false if it does not</returns>
    public virtual bool ContainsTeam(Teams.Base.BaseTeam team)
    {
        if (team == null)
        {
            return false;
        };
        return pointKeeper.ContainsKey(team);
    }
    /// <summary>
    /// used to create a new team
    /// </summary>
    /// <returns>returns the new team's number</returns>
    public virtual float CheckPoints(Teams.Base.BaseTeam team)
    {
        if (ContainsTeam(team))
        {
            return pointKeeper[team];
        }
        return 0;
    }
    /// <summary>
    /// Used to see who is currently at the top of the pointboard
    /// </summary>
    /// <returns>the team with the most points</returns>
    //public static List<Teams.Base.BaseTeam> Leader()
    //{
    //    List<Teams.Base.BaseTeam> retVal = new List<Teams.Base.BaseTeam>();
    //    int teamsCount = pointKeeper.Keys.Count;
    //    switch (teamsCount)
    //    {
    //        case 0:
    //            return retVal;
    //        case 1:
    //            foreach (Teams.Base.BaseTeam team in pointKeeper.Keys)
    //            {
    //                retVal.Add(team);
    //            }
    //            return retVal;
    //        default:
    //            float max = 0;
    //            foreach (Teams.Base.BaseTeam team in pointKeeper.Keys)
    //            {
    //                if (max < Mathf.Max(max, pointKeeper[team]))
    //                {
    //                    max = pointKeeper[team];
    //                    maxTeam = team;
    //                }
    //            }
    //            break;
    //    }


    //    float max = 0;
    //    Teams.Base.BaseTeam maxTeam = null;
    //    foreach (Teams.Base.BaseTeam team in pointKeeper.Keys)
    //    {
    //        if (max < Mathf.Max(max, pointKeeper[team]))
    //        {
    //            max = pointKeeper[team];
    //            maxTeam = team;
    //        }
    //    }
    //    // if we didn't find a team
    //    if (maxTeam == null)
    //    {
    //        Teams.Base.BaseTeam Tie = new Teams.Base.BaseTeam();
    //        Tie.teamData = new Teams.Base.BaseTeamData();
    //        Tie.teamData.teamName = "Tie";
    //        SetPoints(Tie, CheckPoints(maxTeam));
    //        maxTeam = Tie;
    //    }
    //    return maxTeam;
    //}
}
