using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Race", menuName = "GameType/Example/Race")]
public class Race : ExampleGameTypeIntegration {
    [Range(1,999999)]
    public int laps = 1;


    public Dictionary<int, RaceCheckPoint> checkPoints = new Dictionary<int, RaceCheckPoint>();
    public SortedList<int, RaceCheckPoint> orderedCheckPoints = new SortedList<int, RaceCheckPoint>();
    private int firstCheckPoint;
    private int finalCheckPoint;
    public Dictionary<ExampleMember, PlayerCheckPointData> racePositions = new Dictionary<ExampleMember, PlayerCheckPointData>();

    public class PlayerCheckPointData
    {
        public int Lap = 1;
        public int CheckPoint;
        public PlayerCheckPointData(int checkpoint, int lap = 1)
        {
            CheckPoint = checkpoint;
            Lap = lap;
        }
    }


    public void FinishRace(ExampleMember player)
    {
        Debug.Log(player.name + "Finished race with time " + RoundTimerValues.Time);
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
                    //end of race?
                    if (data.Lap >= laps)
                    {
                        //someone finished
                        FinishRace(player);
                    }
                    else
                    {
                        // lap complete back to first check point
                        data.Lap++;
                        data.CheckPoint = firstCheckPoint;
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
        return base.CanStart();
    }
    // END CONDITIONS

    //time limit
    //1st to finish
    //within x secs of 1st place finisher
    //within x secs of previous finisher 
    //last to fin

    //all racers dead
}
