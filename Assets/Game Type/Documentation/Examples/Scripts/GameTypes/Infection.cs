using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// place in Unity\Editor\Data\Resources\ScriptTemplates
[CreateAssetMenu(fileName = "Infection", menuName = "GameType/Example/Infection")]
public class Infection : ExampleGameTypeIntegration
{
    public Teams.Team InfectedTeam;
    public Teams.Team SurvivorTeam;
    [Range(0, .99f)]
    public float startingInfectedPercent = 0.5f;
    public int startingScore = 0;
    public int survivalWorth = 5;
    public int killWorth = 1;
    public int teamKillWorth = -1;
    public int suicideWorth = -1;
    public bool forceTeamColor = true;
    public Dictionary<Teams.TeamMember, float> score;



    // Use this for initialization
    public override void OnEnable()
    {
        base.OnEnable();
        InfectedTeam.OnSuccessfulJoin += InfectedMemberJoinEffect;
        InfectedTeam.OnSuccessfulJoin += MemberJoinEffect;
        score = new Dictionary<Teams.TeamMember, float>();
    }
    public override bool BeginGame()
    {
        if (base.BeginGame())
        {
            CurrentRound = 1;
            return true;
        }
        return false;
    }
    public override void StartGame()
    {
        base.StartGame();
        RoundStart();
    }

    // Called at the end of gameplay 
    // (things like score can be sent off or saved before players should load to the end screen)
    public override void EndGame()
    {
        base.EndGame();
        if (SurvivorTeam.members.Count <= 0)
        {
            Debug.Log("Winner team: " + InfectedTeam.data.TeamName);
            SetWinnerText(InfectedTeam);
        }
        else
        {
            Debug.Log("Winner team: " + SurvivorTeam.data.TeamName);
            foreach(Teams.TeamMember member in SurvivorTeam.members)
            {
                EnsureExistance(member.team, member);
                score[member] += survivalWorth;
            }
            SetWinnerText(SurvivorTeam);
        }
        
        Debug.Log("GameOver");
    }
    //infected
    public void InfectedMemberJoinEffect(Teams.TeamMember member)
    {
        ExampleMember exampleMember = member.GetComponent<ExampleMember>();
        if (exampleMember != null)
        {
            if (forceTeamColor) exampleMember.meshRenderer.material.color = member.team.data.TeamColor;
            //remove later?
            exampleMember.OnDeath = null;
            exampleMember.OnDeath += EvaluateDeath;
        }
    }
    public override void MemberJoinEffect(Teams.TeamMember member)
    {
        ExampleMember exampleMember = member.GetComponent<ExampleMember>();
        if (exampleMember != null)
        {
            if (forceTeamColor) exampleMember.meshRenderer.material.color = exampleMember.personalColor;
            //remove later?
            exampleMember.OnDeath = null;
            exampleMember.OnDeath += EvaluateDeath;
        }
    }
    public override void EnsureExistance(Teams.Team team, Teams.TeamMember member = null)
    {
        if (team == null)
        {
            //choose infected or not
        }
        if (!score.ContainsKey(member))
        {
            score.Add(member, startingScore);
            
        }
    }
    public override void EvaluateDeath(Teams.TeamMember dead, Teams.TeamMember killer)
    {
        ExampleMember deadCheck = dead.GetComponent<ExampleMember>();
        if (deadCheck != null && deadCheck.alive)
        {
            if (killer == null)
            {
                EnsureExistance(dead.team);
                score[dead] += suicideWorth;
                Debug.Log("Suicide " + dead.gameObject.name);
            }
            else
            {
                EnsureExistance(killer.team);
                score[killer] += killWorth;
                Debug.Log(killer.gameObject.name + " killed " + dead.name);
            }
            deadCheck.alive = false;
            dead.gameObject.SetActive(false);
            EvaluateWinCondition(killer.team);
        }
    }
    public override void EvaluateWinCondition(Teams.Team team)
    {
        if (GameState.Key == ExampleGameState.InProgress)
        {
            if (SurvivorTeam.members.Count <= 0)
            {
                EndGame();
            }
        }
    }

    int Rounds = 1;
    int CurrentRound = 0;

    public virtual void RoundEnd()
    {

    }
    public virtual void RoundStart()
    {

    }
}
