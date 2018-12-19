using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendedWithRoundsGameType : ExtendedGameType {
    public TimerValues RoundTimer = new TimerValues();
    [Header("Read Only")]
    public int CurrentRound = 0;

    
    public override void StartGame()
    {
        CurrentRound = 0;
        base.StartGame();
        StartRound();
    }
    public override void EndGame()
    {
        base.EndGame();
        GameManager.StopCoroutine(RoundTimerFunction());
    }
    public virtual void EndRound()
    {
        GameManager.StopCoroutine(RoundTimerFunction());
    }
    public virtual void StartRound()
    {
        // Starts at 0 so when the first round starts the round will be 1
        CurrentRound++;
        RoundTimer.Time = 0;
        GameManager.StartCoroutine(RoundTimerFunction());
    }
    public virtual IEnumerator RoundTimerFunction()
    {
        if (RoundTimer.TimeLimit <= 0)
        {
            while (true)
            {
                RoundTimer.Time += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
        }
        else
        {
            while (RoundTimer.Time < RoundTimer.TimeLimit)
            {
                RoundTimer.Time += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
            if (GameState.Key == ExampleGameState.InProgress) EndRound();
        }
    }
}
