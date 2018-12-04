using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExampleGameTypeIntegration : ExtendedGameType, GameTypes.Interfaces.IPlayers, GameTypes.Interfaces.IFFA
{
    public List<Teams.TeamMember> players = new List<Teams.TeamMember>();
    public override bool AttemptJoin(Teams.Team team, Teams.TeamMember member)
    {
        if (base.AttemptJoin(team, member))
        {
            EnsureExistance(team);
            MemberJoinEffect(member);
            return true;
        }
        return false;
    }
    public virtual void MemberJoinEffect(Teams.TeamMember member)
    {
        ExampleMember exampleMember = member.GetComponent<ExampleMember>();
        if (exampleMember != null)
        {
            exampleMember.OnDeath = null;
            exampleMember.OnDeath += EvaluateDeath;
        }
    }
    public virtual void EnsureExistance(Teams.Team team, Teams.TeamMember member = null)
    {
        if (team != null)
        {
            team.OnSuccessfulJoin += MemberJoinEffect;
        }
    }
    public virtual void EvaluateDeath(Teams.TeamMember dead, Teams.TeamMember killer)
    {
        //if (dead == killer && dead != null)
        //{
        //    //suicide
        //}
    }
    public virtual void EvaluateWinCondition(Teams.Team team)
    {

    }
    public virtual void SetWinnerText(Teams.Team team)
    {
        // could be better but it works
        GameManager gm = GameManager.GetComponent<GameManager>();
        if (gm != null)
        {
            gm.SetWinnerText(team);
        }
    }
    public virtual List<Teams.TeamMember> GetPlayers()
    {
        return players;
    }

    public virtual void SetPlayers(List<Teams.TeamMember> playersList)
    {
        players = playersList;
    }

    public virtual bool IsFFA()
    {
        return false;
    }
}
