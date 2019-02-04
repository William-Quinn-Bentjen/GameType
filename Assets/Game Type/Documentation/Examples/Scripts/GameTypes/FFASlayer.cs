using System.Collections;
using System.Collections.Generic;
using GameTypes.Interfaces;
using Teams;
using UnityEngine;
// place in Unity\Editor\Data\Resources\ScriptTemplates
[CreateAssetMenu(fileName = "FFA Slayer", menuName = "GameType/Example/FFA Slayer")]
public class FFASlayer : ExampleGameTypeIntegration {
    [Space(10)]
    [Header("Game Settings")]
    public bool forceColor = true;
    public Color forcedColor = Color.red;
    [Space(10)]
    [Header("Scoreing Settings")]
    public int killsToWin = 10;
    public int startingScore = 0;
    public int killWorth = 1;
    public int suicideWorth = -1;

    public Dictionary<Teams.TeamMember, float> score = new Dictionary<Teams.TeamMember, float>();
    public override void CreatePlayers()
    {
        base.CreatePlayers();
        if (forceColor == true)
        {
            foreach (JengaPlayer player in players)
            {
                player.SetColor(forcedColor);

            }
        }
    }
    public override void EvaluateDeath(DeathInfo deathInfo)
    {
        if (deathInfo.Victim != null)
        {
            JengaPlayer killerPlayer = deathInfo.Killer.GetComponent<JengaPlayer>();
            if (deathInfo.Killer != null && killerPlayer != null && deathInfo.Killer.gameObject != deathInfo.Victim)
            {
                //killed 
                AddScore(killerPlayer, killWorth);
                EvaluateWinCondition(killerPlayer);
            }
            else
            {
                AddScore(deathInfo.Victim, suicideWorth);
            }
        }
    }
    public void AddScore(Teams.TeamMember member, float scoreToAdd)
    {
        if (!score.ContainsKey(member) && member != null) score.Add(member, startingScore);
        score[member] += scoreToAdd;
    }
    public void EvaluateWinCondition(JengaPlayer player)
    {
        if (GameState.Key == ExampleGameState.InProgress)
        {
            if (score[player] >= killsToWin)
            {
                Debug.Log(player.name + " Won!");
                EndGame();
            }
        }
    }
}
