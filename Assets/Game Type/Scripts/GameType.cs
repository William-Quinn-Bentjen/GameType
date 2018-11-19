using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameType", menuName = "GameTypes/Base/GameType")]
public class GameType : ScriptableObject {
    // Data
    /// <summary>
    /// Keeps the points and teams for this GameType
    /// </summary>
    public ScoreKeeper Score;
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
        
    
    // Used to give the gametype a scorekeeper when it's created
    public void OnEnable()
    {
        Score = CreateInstance<ScoreKeeper>();
    }
    
    // Virutal functions of GameType
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
    public virtual void StartPlay()
    {

    }
    /// <summary>
    /// Called at the end of gameplay 
    /// (things like score can be sent off or saved before players should load to the end screen)
    /// </summary>
    public virtual void EndGame()
    {

    }
    /// <summary>
    /// Called after the game has ended and is the very last thing the gamemode does
    /// </summary>
    public virtual void LeaveMap()
    {

    }
}
