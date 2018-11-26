using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleGameTypeIntegration : ExtendedGameType
{
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
    public virtual void EnsureExistance(Teams.Team team)
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
}
