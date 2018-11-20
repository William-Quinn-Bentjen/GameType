using System.Collections;
using System.Collections.Generic;
using Teams.Base;
using UnityEngine;
// place in Unity\Editor\Data\Resources\ScriptTemplates
[CreateAssetMenu(fileName = "TeamSlayer", menuName = "GameType/TeamSlayer")]
public class TeamSlayer : GameType {
    public int killsToWin = 10;
    public int startingScore = 0;
    public int killWorth = 1;
    public int suicideWorth = -1;
    public bool forceTeamColor = true;
    public Dictionary<Teams.Base.BaseTeam, float> score;

    

	// Use this for initialization
    protected override void OnEnablePreform()
    {
        base.OnEnablePreform();
        score = new Dictionary<Teams.Base.BaseTeam, float>();
    }
	
    // Used to Attempt to start the game
    public override bool BeginGame()
    {
        return base.BeginGame();
    }
	
    // Things like minimum player checks should be done here to determine if the game can start
    public override bool CanStart()
    {
		return base.CanStart();
    }
	
    // Called at the begining of gameplay after the everthing is ready 
    public override void StartGame()
    { 
        base.StartGame();
    }
	
    // Called at the end of gameplay 
    // (things like score can be sent off or saved before players should load to the end screen)
    public override void EndGame()
    {
        base.EndGame();
    }
	
    // Called after the game has ended and is the very last thing the gamemode does
    public override void LeaveMap()
    {
		
    }


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
    public void MemberJoinEffect(Teams.Base.BaseTeamMember member)
    {
        ExampleMember exampleMember = member.GetComponent<ExampleMember>();
        if (exampleMember != null)
        {
            // set team's color
            if (forceTeamColor) exampleMember.meshRenderer.material.color = exampleMember.team.data.TeamColor;
            exampleMember.OnDeath = null;
            exampleMember.OnDeath += EvaluateDeath;
        }
    }

    public void EnsureExistance(Teams.Base.BaseTeam team)
    {
        if (team != null && !score.ContainsKey(team))
        {
            score.Add(team, startingScore);
            team.OnSuccessfulJoin += MemberJoinEffect;
        }
    }
    public void EvaluateDeath(Teams.Base.BaseTeamMember dead, Teams.Base.BaseTeamMember killer)
    {
        if (killer == null)
        {
            EnsureExistance(dead.team);
            score[dead.team] += suicideWorth;
            Debug.Log("Suicide " + dead.gameObject.name);
        }
        else
        {
            EnsureExistance(killer.team);
            score[killer.team] += killWorth;
            Debug.Log("Killer " + killer.gameObject.name);
        }
        dead.gameObject.SetActive(false);
    }
}
