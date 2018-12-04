using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// place in Unity\Editor\Data\Resources\ScriptTemplates
[CreateAssetMenu(fileName = "TeamSlayer", menuName = "GameType/Example/TeamSlayer")]
public class TeamSlayer : ExampleGameTypeIntegration {
    [Header("Game Settings")]
    public bool forceTeamColor = true;
    [Header("Scoreing Settings")]
    public int killsToWin = 10;
    public int startingScore = 0;
    public int killWorth = 1;
    public int teamKillWorth = -1;
    public int suicideWorth = -1;
    public Dictionary<Teams.Team, float> score = new Dictionary<Teams.Team, float>();

    

	// Use this for initialization
    public override void OnEnable()
    {
        base.OnEnable();
        score = new Dictionary<Teams.Team, float>();
    }

    // Called at the end of gameplay 
    // (things like score can be sent off or saved before players should load to the end screen)
    public override void EndGame()
    {
        if (GameState.CurrentState.Key == ExampleGameState.InProgress)
        {
            GameManager.StopCoroutine(GameTimer());
            GameState.ChangeState(ExampleGameState.Ending);
            Debug.Log("GameOver");
        }
        
    }
    public override void MemberJoinEffect(Teams.TeamMember member)
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
    public override void EnsureExistance(Teams.Team team, Teams.TeamMember member = null)
    {
        if (team != null && !score.ContainsKey(team))
        {
            score.Add(team, startingScore);
            team.OnSuccessfulJoin += MemberJoinEffect;
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
                score[dead.team] += suicideWorth;
                Debug.Log("Suicide " + dead.gameObject.name);
            }
            else
            {
                EnsureExistance(killer.team);
                score[killer.team] += killWorth;
                EvaluateWinCondition(killer.team);
                Debug.Log(killer.gameObject.name + " killed " + dead.name);
            }
            deadCheck.alive = false;
            dead.gameObject.SetActive(false);
        }
    }
    public override void EvaluateWinCondition(Teams.Team team)
    {
        if (GameState.Key == ExampleGameState.InProgress)
        {
            if (score[team] >= killsToWin)
            {
                Debug.Log("Winner team: " + team.data.TeamName);
                SetWinnerText(team);
                EndGame();
            }
        }
    }
}
