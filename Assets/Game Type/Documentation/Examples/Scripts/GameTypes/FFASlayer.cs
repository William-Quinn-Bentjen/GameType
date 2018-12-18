//using System.Collections;
//using System.Collections.Generic;
//using GameTypes.Interfaces;
//using Teams;
//using UnityEngine;
//// place in Unity\Editor\Data\Resources\ScriptTemplates
//[CreateAssetMenu(fileName = "FFA Slayer", menuName = "GameType/Example/FFA Slayer")]
//public class FFASlayer : ExampleGameTypeIntegration {
//    [Space(10)]
//    [Header("Game Settings")]
//    public bool forceColor = true;
//    public Color forcedColor = Color.red;
//    [Space(10)]
//    [Header("Scoreing Settings")]
//    public int killsToWin = 10;
//    public int startingScore = 0;
//    public int killWorth = 1;
//    public int suicideWorth = -1;
//    public Dictionary<Teams.TeamMember, float> score = new Dictionary<Teams.TeamMember, float>();
//    public override bool IsFFA()
//    {
//        return true;
//    }


//    // Use this for initialization
//    public override void OnEnable()
//    {
//        base.OnEnable();
//        score = new Dictionary<Teams.TeamMember, float>();
//        players = new List<Teams.TeamMember>();
//    }
//    public override bool BeginGame()
//    {
//        ResetScoreboard();
//        if (base.BeginGame())
//        {
//            StartGame();
//            return true;
//        }
//        return false;
//    }
//    public void ResetScoreboard()
//    {
//        score.Clear();
//        players = new List<Teams.TeamMember>(FindObjectsOfType<Teams.TeamMember>());
//        foreach (Teams.TeamMember player in players)
//        {
//            if (!score.ContainsKey(player))
//            {
//                score.Add(player, startingScore);
//                MemberJoinEffect(player);
//            }
//        }
//    }
//    public override bool CanStart()
//    {
//        if (base.CanStart() && players.Count > 0)
//        {
//            return true;
//        }
//        return false;
//    }
//    // Called at the end of gameplay 
//    // (things like score can be sent off or saved before players should load to the end screen)
//    public override void EndGame()
//    {
//        if (GameState.CurrentState.Key == ExampleGameState.InProgress)
//        {
//            GameManager.StopCoroutine(GameTimerFunction());
//            GameState.ChangeState(ExampleGameState.Ending);
//            Debug.Log("GameOver");
//        }
        
//    }
//    public override void MemberJoinEffect(Teams.TeamMember member)
//    {
//        ExampleMember exampleMember = member.GetComponent<ExampleMember>();
//        if (exampleMember != null)
//        {
//            if (forceColor) exampleMember.meshRenderer.material.color = forcedColor;
//            //remove later?
//            exampleMember.OnDeath = null;
//            exampleMember.OnDeath += EvaluateDeath;
//        }
//    }
//    public override void EnsureExistance(Teams.Team team, Teams.TeamMember member = null)
//    {
//        if (!score.ContainsKey(member))
//        {
//            MemberJoinEffect(member);
//            score.Add(member, startingScore);
//        }
//    }
//    public override void EvaluateDeath(Teams.TeamMember dead, Teams.TeamMember killer)
//    {
//        ExampleMember deadCheck = dead.GetComponent<ExampleMember>();
//        if (deadCheck != null && deadCheck.alive)
//        {
//            if (killer == null)
//            {
//                EnsureExistance(dead.team, dead);
//                score[dead] += suicideWorth;
//                Debug.Log("Suicide " + dead.gameObject.name);
//            }
//            else
//            {
//                EnsureExistance(killer.team, killer);
//                score[killer] += killWorth;
//                EvaluateWinCondition(killer);
//                Debug.Log(killer.gameObject.name + " killed " + dead.name);
//            }
//            deadCheck.alive = false;
//            dead.gameObject.SetActive(false);
//        }
//    }
//    public void EvaluateWinCondition(Teams.TeamMember member)
//    {
//        if (GameState.Key == ExampleGameState.InProgress)
//        {
//            if (score[member] >= killsToWin)
//            {
//                Debug.Log("Winner team: " + member.name);
//                SetWinnerText(new Teams.Team() { data = new Teams.TeamData() { TeamName = member.name, TeamColor = (member as ExampleMember).personalColor } });
//                EndGame();
//            }
//        }
//    }


//}
