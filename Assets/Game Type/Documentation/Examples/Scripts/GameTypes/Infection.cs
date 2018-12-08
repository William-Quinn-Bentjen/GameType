using System.Collections;
using System.Collections.Generic;
using Teams;
using UnityEngine;
// place in Unity\Editor\Data\Resources\ScriptTemplates
[CreateAssetMenu(fileName = "Infection", menuName = "GameType/Example/Infection")]
public class Infection : ExampleGameTypeWithRoundsIntegration
{
    [Space(10)]
    [Header("Teams")]
    public Teams.Team InfectedTeam;
    public Teams.Team SurvivorTeam;
    [Space(5)]
    [Header("Game Settings")]
    [Range(0, .99f)]
    public float startingInfectedPercent = 0.5f;
    [Tooltip("Enable if you wanted 1 zombie instead of 1% at the start")]
    public bool useAmount = false;
    [Range(0, 999999)]
    public int startingInfectedAmount = 1;
    //public int startingNumberOfInfected
    public Mesh infectedMesh;
    public Mesh survivorMesh;

    public bool suicidesInfect = true;
    public bool teamKilledGetInfected = false;
    public bool teamKillersGetInfected = false;
    public bool forceInfectedMesh = false;
    public bool forceInfectedTeamColor = true;
    public bool forceSurvivorTeamColor = true;
    [Space(20)]
    [Header("Scoreing")]
    public int startingScore = 0;
    // surviving via time
    public int survivalWorth = 5;
    // when a infected kills a survivor
    public int infectionSpreadWorth = 1;
    // when a survivor kills an infected
    public int infectedKilledWorth = 1;
    // a teamkill on either team
    public int teamKillWorth = -1;
    // a suicide on either team
    public int suicideWorth = -1;
    public Dictionary<Teams.TeamMember, float> score = new Dictionary<Teams.TeamMember, float>();

    public override void StartRound()
    {
        AssignTeams();
        base.StartRound();
    }
    public override bool IsFFA()
    {
        return true;
    }
    // Use this for initialization
    public override void OnEnable()
    {
        base.OnEnable();
        score = new Dictionary<Teams.TeamMember, float>();
    }
    public override bool CanStart()
    {
        if (InfectedTeam != null && SurvivorTeam != null && players.Count > 1) 
        {
            return true;
        }
        return false;
    }
    public override bool BeginGame()
    {
        if (base.BeginGame())
        {
            HookUpTeams();
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
            Debug.Log("Winning team: " + InfectedTeam.data.TeamName);
            SetWinnerText(InfectedTeam);
        }
        else
        {
            Debug.Log("Winning team: " + SurvivorTeam.data.TeamName);
            foreach(Teams.TeamMember member in SurvivorTeam.members)
            {
                if (member != null)
                {
                    EnsureExistance(member.team, member);
                    score[member] += survivalWorth;
                }
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
            //remove later?
            exampleMember.OnDeath = null;
            exampleMember.OnDeath += EvaluateDeath;
            Infect(exampleMember);
        }
    }
    public override void MemberJoinEffect(Teams.TeamMember member)
    {
        ExampleMember exampleMember = member.GetComponent<ExampleMember>();
        if (exampleMember != null)
        {
            //remove later?
            exampleMember.OnDeath = null;
            exampleMember.OnDeath += EvaluateDeath;
            Disinfect(exampleMember);
        }
    }
    public override void EnsureExistance(Teams.Team team, Teams.TeamMember member = null)
    {
        if (team == null)
        {
            //choose infected or not
        }
        if (member != null && !score.ContainsKey(member))
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
                EnsureExistance(dead.team, dead);
                score[dead] += suicideWorth;
                Debug.Log("Suicide " + dead.gameObject.name + dead.gameObject.GetComponent<Rigidbody>().velocity);
                if (suicidesInfect && dead.team == SurvivorTeam)
                {
                    // Infect
                    Infect(deadCheck);
                }
                else
                {
                    // Deavtivate dead
                    dead.gameObject.SetActive(false);
                    deadCheck.alive = false;
                }
            }
            else
            {
                EnsureExistance(killer.team, killer);
                Debug.Log(killer.gameObject.name + " killed " + dead.name);
                if (killer.team == dead.team)
                {
                    // Team kill
                    score[killer] += teamKillWorth;
                    if (dead.team != InfectedTeam)
                    {
                        if (teamKilledGetInfected) Infect(deadCheck);
                        if (teamKillersGetInfected) Infect(killer as ExampleMember);
                    }
                }
                else if (killer.team == InfectedTeam && dead.team == SurvivorTeam)
                {
                    // Infected kills survivor
                    score[killer] += infectionSpreadWorth;
                    // Infect
                    Infect(deadCheck);
                }
                else if (killer.team == SurvivorTeam && dead.team == InfectedTeam)
                {
                    // Survivor kills infected
                    score[killer] += infectedKilledWorth;
                    // Deactivate infected killed
                    dead.gameObject.SetActive(false);
                    deadCheck.alive = false;
                }
            }
        }
        EvaluateWinCondition(dead.team);
    }
    public virtual void Disinfect(ExampleMember member)
    {
        if (forceSurvivorTeamColor) member.meshRenderer.material.color = member.personalColor;
        if (forceInfectedMesh) member.meshFilter.mesh = survivorMesh;
        InfectedTeam.members.Remove(member);
        if (!SurvivorTeam.members.Contains(member))
        {
            SurvivorTeam.members.Add(member);
        }
        member.team = SurvivorTeam;
    }
    public virtual void Infect(ExampleMember member)
    {
        if (forceInfectedMesh) member.meshFilter.mesh = infectedMesh;
        if (forceInfectedTeamColor) member.meshRenderer.material.color = InfectedTeam.data.TeamColor;
        SurvivorTeam.members.Remove(member);
        if (!InfectedTeam.members.Contains(member))
        {
            InfectedTeam.members.Add(member);
        }
        member.team = InfectedTeam;
    }
    public override void EvaluateWinCondition(Teams.Team team)
    {
        if (GameState.Key == ExampleGameState.InProgress)
        {
            if (SurvivorTeam.members.Count <= 0)
            {
                EndGame();
            }
            if (InfectedTeam.members.Count <= 0)
            {
                EndGame();
            }
        }
    }
    
    //int CurrentRound = 0;

    public virtual void RoundEnd()
    {

    }
    public virtual void RoundStart()
    {

    }
    public virtual void HookUpTeams()
    {
        if (InfectedTeam != null)
        {
            InfectedTeam.OnSuccessfulJoin = null;
            InfectedTeam.OnSuccessfulJoin += InfectedMemberJoinEffect;
            InfectedTeam.OnAttemptJoin = null;
            InfectedTeam.OnAttemptJoin += AttemptJoin;
        }
        if (SurvivorTeam != null)
        {
            SurvivorTeam.OnSuccessfulJoin = null;
            SurvivorTeam.OnSuccessfulJoin += MemberJoinEffect;
            SurvivorTeam.OnAttemptJoin = null;
            SurvivorTeam.OnAttemptJoin = AttemptJoin;
        }
        AssignTeams();
    }
    public void AssignTeams()
    {
        if (players.Count > 1)
        {
            List<Teams.TeamMember> unassignedPlayers = new List<TeamMember>(players.ToArray());
            if (useAmount)
            {
                if (startingInfectedAmount < players.Count)
                {
                    for (int i = 0; i < startingInfectedAmount; i++)
                    {
                        int removeAt = Random.Range(0, unassignedPlayers.Count - 1);
                        InfectedTeam.Join(unassignedPlayers[removeAt]);
                        unassignedPlayers.RemoveAt(removeAt);
                    }
                    for(int i = 0; i < unassignedPlayers.Count; i++)
                    {
                        SurvivorTeam.Join(unassignedPlayers[i]);
                    }
                }
                else
                {
                    int removeAt = Random.Range(0, unassignedPlayers.Count - 1);
                    SurvivorTeam.Join(unassignedPlayers[removeAt]);
                    unassignedPlayers.RemoveAt(removeAt);
                    for(int i = 0; i < unassignedPlayers.Count; i++)
                    {
                        InfectedTeam.Join(unassignedPlayers[i]);
                    }
                }
            }
            else
            {
                int infectedAmount = Mathf.FloorToInt(startingInfectedPercent * unassignedPlayers.Count);
                if (infectedAmount != 0)
                {
                    if (infectedAmount < players.Count)
                    {
                        for (int i = 0; i < infectedAmount; i++)
                        {
                            int removeAt = Random.Range(0, unassignedPlayers.Count - 1);
                            InfectedTeam.Join(unassignedPlayers[removeAt]);
                            unassignedPlayers.RemoveAt(removeAt);
                        }
                        for (int i = 0; i < unassignedPlayers.Count; i++)
                        {
                            SurvivorTeam.Join(unassignedPlayers[i]);
                        }
                    }
                    else
                    {
                        int removeAt = Random.Range(0, unassignedPlayers.Count - 1);
                        SurvivorTeam.Join(unassignedPlayers[removeAt]);
                        unassignedPlayers.RemoveAt(removeAt);
                        for (int i = 0; i < unassignedPlayers.Count; i++)
                        {
                            InfectedTeam.Join(unassignedPlayers[i]);
                        }
                    }

                }
                else
                {
                    for (int i = 0; i < unassignedPlayers.Count; i++)
                    {
                        SurvivorTeam.Join(unassignedPlayers[i]);
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("not enough players, no zombies will be spawned");
        }
    }
    public bool AttemptJoin(TeamMember member)
    {
        return true;
    }
}
