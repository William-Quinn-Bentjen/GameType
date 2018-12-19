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
    
    public override void EndGame()
    {
        if (GameState.CurrentState.Key == ExampleGameState.InProgress)
        {
            GameManager.StopCoroutine(GameTimerFunction());
            GameState.ChangeState(ExampleGameState.Ending);
            Debug.Log("GameOver");
        }
        
    }
    public void EvaluateWinCondition(Teams.Team team)
    {
        if (GameState.Key == ExampleGameState.InProgress)
        {
            if (score[team] >= killsToWin)
            {
                Debug.Log("Winner team: " + team.data.TeamName);
                EndGame();
            }
        }
    }
}
