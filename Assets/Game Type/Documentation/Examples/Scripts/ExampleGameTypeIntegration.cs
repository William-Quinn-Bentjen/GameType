using System.Collections;
using System.Collections.Generic;
using Teams.Base;
using UnityEngine;

public class ExampleGameTypeIntegration : ExtendedGameType
{
    public override bool AttemptJoin(BaseTeam team, BaseTeamMember member)
    {
        if (base.AttemptJoin(team, member))
        {
            EnsureExistance(team);
            MemberJoinEffect(member);
            return true;
        }
        return false;
    }
    public virtual void MemberJoinEffect(Teams.Base.BaseTeamMember member)
    {
        ExampleMember exampleMember = member.GetComponent<ExampleMember>();
        if (exampleMember != null)
        {
            exampleMember.OnDeath = null;
            exampleMember.OnDeath += EvaluateDeath;
        }
    }
    public virtual void EnsureExistance(Teams.Base.BaseTeam team)
    {
        if (team != null)
        {
            team.OnSuccessfulJoin += MemberJoinEffect;
        }
    }
    public virtual void EvaluateDeath(Teams.Base.BaseTeamMember dead, Teams.Base.BaseTeamMember killer)
    {
        //if (dead == killer && dead != null)
        //{
        //    //suicide
        //}
    }
    public virtual void EvaluateWinCondition(BaseTeam team)
    {

    }
}
