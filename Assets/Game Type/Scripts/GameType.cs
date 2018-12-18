using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "GameType", menuName = "GameTypes/Base/GameType")]
public class GameType : ScriptableObject {
    [Header("Basic Settings")]
    // Data
    /// <summary>
    /// The GameManager this gametype is hooked up to (used for coroutines)
    /// </summary>
    public MonoBehaviour GameManagerMonoBehaviour;
    [System.Serializable]
    public struct TimerValues
    {
        /// <summary>
        /// The total duration in seconds (0 is no limit)
        /// </summary>
        public float TimeLimit;
        /// <summary>
        /// The time that the timer has been running in realtime;
        /// </summary>
        public float Time;
    }
    public TimerValues GameTimer = new TimerValues();


    // Used to give the gametype info when it's created
    public virtual void OnEnable()
    {

    }
    /// <summary>
    /// Game timer (this is basically the tick function for GameTime)
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator GameTimerFunction()
    {
        // Set the game time to 0 because the timer just started
        GameTimer.Time = 0;
        // 0 for no limit
        if (GameTimer.TimeLimit != 0)
        {
            // Actual timer logic
            while (GameTimer.Time < GameTimer.TimeLimit)
            {
                GameTimer.Time += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
            // End the game 
            EndGame();
        }
        else
        {
            // Count until the game ends
            while (true)
            {
                GameTimer.Time += Time.deltaTime;
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
        if (CanStart())
        {
            EnterMap();
            return true;
        }
        return false;
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
        GameManagerMonoBehaviour.StartCoroutine(GameTimerFunction());
    }
    /// <summary>
    /// Called at the end of gameplay
    /// (things like score can be sent off or saved before players should load to the end screen)
    /// </summary>
    public virtual void EndGame()
    {
        GameManagerMonoBehaviour.StopCoroutine(GameTimerFunction());
    }
    /// <summary>
    /// Called after the game has ended and is the very last thing the gamemode does
    /// </summary>
    public virtual void LeaveMap()
    {

    }
    /// <summary>
    /// Tells the game type that a player is attempting to join the team
    /// </summary>
    /// <param name="team">The team the member is trying to join</param>
    /// <param name="member">The member who's trying to join</param>
    /// <returns>true if the join was successful</returns>
    public virtual bool AttemptJoin(Teams.Team team, Teams.TeamMember member)
    {
        if (team != null && member != null)
        {
            return team.Join(member);
        }
        return false;
        
    }
    /// <summary>
    /// Tells the game type that a player is attempting to leave the team they are on
    /// </summary>
    /// <param name="member">the member who is attempting to leave</param>
    /// <returns>true if the leave was successful</returns>
    public virtual bool AttemptLeave(Teams.TeamMember member)
    {
        member.team.Leave(member);
        return true;
    }
}
