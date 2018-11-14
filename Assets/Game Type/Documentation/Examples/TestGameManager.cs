using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TestGameManager : MonoBehaviour {
    public static TestGameManager instance;
    public Text text;
	// Use this for initialization
	void Awake () {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
        //string teamNames = "";
        //int teamCount = 0;
        //foreach (Teams.Base.BaseTeam team in GameManager.Instance.GameType.Points.GetAllTeams())
        //{
        //    teamCount++;
        //    teamNames += team.data.TeamName + "\n";
        //}

        //debugs total count (comment out to have member set this to team name)
        //text.text = GameManager.Instance.GameType.Points.GetAllTeams().Count.ToString();    //teamCount + "\n" + teamNames;
    }
}
