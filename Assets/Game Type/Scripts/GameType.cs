using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "GameType", menuName = "GameTypes/Base/GameType")]
public class GameType : ScriptableObject {
    // Data
    /// <summary>
    /// The GameManager this gametype is hooked up to (used for coroutines)
    /// </summary>
    public MonoBehaviour GameManager;
    /// <summary>
    /// The total duration the game should be in seconds (0 is no limit)
    /// </summary>
    public float GameTimeLimit = 0;
    /// <summary>
    /// The time that the game has been running in realtime;
    /// </summary>
    private float GameTime = 0;
    /// <summary>
    /// Used to keep track of what state the game is currently in
    /// </summary>
    public GameState CurrentState = GameState.Lobby;

    // Enums and structs
    /// <summary>
    /// States the game may be in
    /// </summary>
    public enum GameState
    {
        Lobby,
        EnteringMap,
        Starting,
        InProgress,
        Ending,
        LeavingMap,
        Aborted
    }


    // Used to give the gametype info when it's created
    public void OnEnable()
    {
        OnEnablePreform();
    }


    // Virutal functions
    /// <summary>
    /// what should be preformed when enable is called 
    /// </summary>
    protected virtual void OnEnablePreform()
    {
        
    }
    /// <summary>
    /// Game timer (this is basically the tick function for GameTime)
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator GameTimer()
    {
        // Set the game time to 0 because the timer just started
        GameTime = 0;
        // 0 for no limit
        if (GameTimeLimit != 0)
        {
            // Actual timer logic
            while (GameTime < GameTimeLimit)
            {
                GameTime += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
            // End the game if the time is up
            if (CurrentState == GameState.InProgress)
            {
                EndGame();
            }
        }
        else
        {
            // Count until the game ends
            while (CurrentState == GameState.InProgress)
            {
                GameTime += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
        }
    }
    /// <summary>
    /// Used to Attempt to start the game
    /// </summary>
    /// <returns>If the game successfully started returns true</returns>
    public virtual bool BeginGame()
    {
        return CanStart();
    }
    /// <summary>
    /// Things like minimum player checks should be done here to determine if the game can start
    /// </summary>
    /// <returns>If the game can start</returns>
    public virtual bool CanStart()
    {
        return true;
    }
    /// <summary>
    /// Called before StartGame
    /// </summary>
    public virtual void EnterMap()
    {
        
    }
    /// <summary>
    /// Called at the begining of gameplay after the everthing is ready 
    /// (after this is called the game will o
    /// </summary>
    public virtual void StartGame()
    {
        GameManager.StartCoroutine(GameTimer());
    }
    /// <summary>
    /// Called at the end of gameplay
    /// (things like score can be sent off or saved before players should load to the end screen)
    /// </summary>
    public virtual void EndGame()
    {
        GameManager.StopCoroutine(GameTimer());
    }
    /// <summary>
    /// Called after the game has ended and is the very last thing the gamemode does
    /// </summary>
    public virtual void LeaveMap()
    {

    }
    public virtual bool AttemptJoin(Teams.Base.BaseTeam team, Teams.Base.BaseTeamMember member)
    {
        if (team != null && member != null)
        {
            return team.Join(member);
        }
        return false;
        
    }
    public virtual bool AttemptLeave(Teams.Base.BaseTeamMember member)
    {
        member.team.Leave(member);
        return true;
    }
}
