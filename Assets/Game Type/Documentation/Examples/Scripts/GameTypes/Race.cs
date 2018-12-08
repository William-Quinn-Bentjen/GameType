using System.Collections;
using System.Collections.Generic;
using Teams;
using UnityEngine;

[CreateAssetMenu(fileName = "Race", menuName = "GameType/Example/Race")]
public class Race : ExampleGameTypeIntegration {
    public List<ExampleMember> finishedRacers = new List<ExampleMember>();
    [Range(1,999999)]
    public int laps = 1;


    public Dictionary<int, RaceCheckPoint> checkPoints = new Dictionary<int, RaceCheckPoint>();
    public SortedList<int, RaceCheckPoint> orderedCheckPoints = new SortedList<int, RaceCheckPoint>();
    private int firstCheckPoint;
    private int finalCheckPoint;
    public Dictionary<ExampleMember, PlayerCheckPointData> racePositions = new Dictionary<ExampleMember, PlayerCheckPointData>();
    
    public enum RaceEndConditions
    {
        FirstToFinish,
        EveryoneFinished,
        WithinSecondsOfFirst,
        WithinSecondsOfPrevious
    }
    public RaceEndConditions raceEndConditions;
    [Range(0, 999999)]
    public float withinSeconds = 15;
    public float withinTimer = 0;
    private bool startedEndTimer = false;
    public class PlayerCheckPointData
    {
        public int Lap = 1;
        public int CheckPoint;
        public List<float> LapTimes = new List<float>();
        public float RecordLap(float gameTime)
        {
            int lapTimes = LapTimes.Count;
            if (lapTimes > 0)
            {
                float lapTime = gameTime - LapTimes[LapTimes.Count - 1];
                LapTimes.Add(lapTime);
                return lapTime;
            }
            else
            {
                LapTimes.Add(gameTime);
                return gameTime;
            }
        }
        public PlayerCheckPointData(int checkpoint, int lap = 1)
        {
            CheckPoint = checkpoint;
            Lap = lap;
        }
    }


    public void FinishRace(ExampleMember player)
    {
        if (!finishedRacers.Contains(player))
        {
            finishedRacers.Add(player);
            Debug.Log(player.name + " Finished race with time " + GameTimer.Time);
        }
        EvaluateWinCondition(null);
        
    }
    public override void EvaluateWinCondition(Team team)
    {
        if (GameState.Key == ExampleGameState.InProgress)
        {
            switch (raceEndConditions)
            {
                case RaceEndConditions.FirstToFinish:
                    if (finishedRacers.Count > 0)
                    {
                        EndGame();
                    }
                    break;
                case RaceEndConditions.EveryoneFinished:
                    if (finishedRacers.Count == players.Count)
                    {
                        EndGame();
                    }
                    break;
                case RaceEndConditions.WithinSecondsOfFirst:
                    if (finishedRacers.Count == 1 && !startedEndTimer)
                    {
                        GameManager.StartCoroutine(EndTimer());
                        //start timer if not started 
                    }
                    break;
                case RaceEndConditions.WithinSecondsOfPrevious:
                    if (finishedRacers.Count >= 1)
                    {
                        if (finishedRacers.Count >= players.Count)
                        {
                            //everyone finished
                            EndGame();
                            break;
                        }
                        else if (startedEndTimer)
                        {
                            //reset timer
                            withinTimer = 0;
                        }
                        else
                        {
                            //start timer
                            GameManager.StartCoroutine(EndTimer());
                        }
                    }
                    break;
            }
        }
        base.EvaluateWinCondition(team);
    }
    public void GetCheckPoints()
    {
        List<RaceCheckPoint> foundCheckPoints = new List<RaceCheckPoint>(FindObjectsOfType<RaceCheckPoint>());
        if (foundCheckPoints.Count > 0)
        {
            SortedList<int, RaceCheckPoint> orderedCheckPointsList = new SortedList<int, RaceCheckPoint>();
            foreach (RaceCheckPoint checkPoint in foundCheckPoints)
            {
                orderedCheckPointsList.Add(checkPoint.checkPointNumber, checkPoint);
            }
            RaceCheckPoint[] race = new RaceCheckPoint[orderedCheckPointsList.Values.Count];
            orderedCheckPointsList.Values.CopyTo(race, 0);
            // Debug.Log
            List<int> checkPointOrder = new List<int>();
            checkPoints.Clear();
            foreach (var pair in orderedCheckPointsList)
            {
                pair.Value.onTriggered = null;
                pair.Value.onTriggered += CheckPointTouched;
                checkPointOrder.Add(pair.Key);
                checkPoints.Add(pair.Key, pair.Value);
                Debug.Log(pair);
            }
            orderedCheckPoints = orderedCheckPointsList;
            firstCheckPoint = orderedCheckPoints.Keys[0];
            finalCheckPoint = orderedCheckPoints.Keys[orderedCheckPoints.Count - 1];
        }
    }
    public void CheckPointTouched(Collider other, int checkPointNumber)
    {
        ExampleMember player = other.GetComponent<ExampleMember>();
        if (player != null)
        {
            if (!racePositions.ContainsKey(player))
            {
                racePositions.Add(player, new PlayerCheckPointData(orderedCheckPoints.Keys[0])); 
            }
            PlayerCheckPointData data = racePositions[player];
            // did we reach the correct checkpoint?
            if (data.CheckPoint == checkPointNumber)
            {
                //is it the final checkpoint?
                if (checkPointNumber == finalCheckPoint)
                {
                    // lap complete back to first check point
                    data.RecordLap(GameTimer.Time);
                    data.Lap++;
                    data.CheckPoint = firstCheckPoint;
                    //end of race?
                    if (data.Lap >= laps)
                    {
                        //someone finished
                        FinishRace(player);
                    }
                    
                }
                else
                {
                    Debug.Log(other.name + " reached checkpoint #" + checkPointNumber);
                    data.CheckPoint = orderedCheckPoints.Keys[orderedCheckPoints.IndexOfKey(data.CheckPoint) + 1];
                }
            }
        }
    }
    public override bool CanStart()
    {
        GetCheckPoints();
        return (checkPoints.Count > 0 && players.Count > 0);
    }
    public override void StartGame()
    {
        startedEndTimer = false;
        racePositions.Clear();
        finishedRacers.Clear();
        foreach (Teams.TeamMember player in players)
        {
            ExampleMember member = player.GetComponent<ExampleMember>();
            if (member != null) racePositions.Add(member, new PlayerCheckPointData(orderedCheckPoints.Keys[0]));
        }
        base.StartGame();
    }
    public override void EndGame()
    {
        base.EndGame();
        GameManager.StopCoroutine(EndTimer());
        ExampleMember first = null;
        if (finishedRacers.Count >0)
        {
            first = finishedRacers[0];
        }
        if (first != null)
        {
            List<float> lapTimes = racePositions[first].LapTimes;
            if (lapTimes != null && lapTimes.Count > 0)
            {
                SetWinnerText(new Team() { data = new TeamData() { TeamName = first.name + racePositions[first].LapTimes[racePositions[first].LapTimes.Count - 1], TeamColor = first.personalColor } });
            }
            else
            {
                SetWinnerText(new Team { data = new TeamData { TeamName = first.name, TeamColor = first.personalColor } });
            }
        }
        else
        {
            SetWinnerText(new Team() { data = new TeamData() { TeamName = "No one finished" , TeamColor = Color.red} });
        }
    }
    // END CONDITIONS

    //1st to finish                         someone finished
    //within x secs of 1st place finisher   I was 2 seconds behind can I finish like in GTA5
    //within x secs of previous finisher    as long as someone else finished in the last x seconds
    //last to fin                           everyone must finish the race before the game ends

    //time limit                            5mins is up
    //all active racers dead                1st place finished but the 
    public IEnumerator EndTimer()
    {
        if (GameState.Key == ExampleGameState.InProgress)
        {
            startedEndTimer = true;
            while(withinTimer < withinSeconds)
            {
                withinTimer += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
            EndGame();
        }
    }
}
