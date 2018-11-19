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




        //IF THINGS BREAK USE THIS
        //string debug = "";
        //if (GameManager.Instance == null)
        //{
        //    debug = "GM instance null";
        //}
        //else if (GameManager.Instance.GameType == null)
        //{
        //    debug = "GT is null";
        //}
        //else if (GameManager.Instance.GameType.Score == null)
        //{
        //    debug = "Score = null";
        //}
        //else if (GameManager.Instance.GameType.Score.scoreKeeper == null)
        //{
        //    debug = "scorekeeper in score is null";
        //}
        //text.text = debug;

        text.text = GameManager.Instance.GameType.Score.scoreKeeper.Keys.Count.ToString();    //teamCount + "\n" + teamNames;
    }
}
