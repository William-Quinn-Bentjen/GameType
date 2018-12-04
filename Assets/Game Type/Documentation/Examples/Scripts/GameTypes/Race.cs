using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Race", menuName = "GameType/Example/Race")]
public class Race : ExampleGameTypeIntegration {
    [Range(1,999999)]
    public int laps = 1;
    public Dictionary<int, RaceCheckPoint> checkPoints;
    public void GetCheckPoints()
    {
        List<RaceCheckPoint> foundCheckPoints = new List<RaceCheckPoint>(FindObjectsOfType<RaceCheckPoint>());
        Dictionary<int, RaceCheckPoint> orderedCheckPoints = new Dictionary<int, RaceCheckPoint>();
        SortedList<int, RaceCheckPoint> orderedCheckPointsList = new SortedList<int, RaceCheckPoint>();
        foreach (RaceCheckPoint checkPoint in foundCheckPoints)
        {
            orderedCheckPoints.Add(checkPoint.checkPointNumber, checkPoint);
            orderedCheckPointsList.Add(checkPoint.checkPointNumber, checkPoint);
        }
        RaceCheckPoint[] race = new RaceCheckPoint[0];
        orderedCheckPointsList.Values.CopyTo(race, 0);
        //foreach (RaceCheckPoint)
       // Debug.Log
        foreach(var pair in orderedCheckPointsList)
        {
            Debug.Log(pair);
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
