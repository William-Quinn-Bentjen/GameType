using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestBuild : MonoBehaviour {
    public Teams.Base.BaseTeam team;
    public Teams.Base.BaseTeamMember member;
    public Text text;

    // Use this for initialization
    void Start () {
        string teamName = "Name";
        int teamMembers = 0;
        bool memberHasTeam = false;
        if (team != null && team.data != null)
        {
            teamName = team.data.TeamName;
            teamMembers = team.members.Count;
            if (member.team != null)
            {
                memberHasTeam = true;
            }
        }
        text.text = teamName + "\nMembers: " + teamMembers + "\nMember has team = " + memberHasTeam.ToString();
    }
	
	// Update is called once per frame
	void Update () {
        string teamName = "Name";
        int teamMembers = 0;
        bool memberHasTeam = false;
        if (team != null && team.data != null)
        {
            teamName = team.data.TeamName;
            teamMembers = team.members.Count;
            if (member.team != null)
            {
                memberHasTeam = true;
            }
        }
        TeamSlayer teamSlayer = (TeamSlayer)GameManager.Instance.GameType;
        if (teamSlayer != null && teamSlayer.score.ContainsKey(team))
        {
            text.text = teamName + "\nMembers: " + teamMembers + "\nScore " + teamSlayer.score[team];
        }
        
    }
}
