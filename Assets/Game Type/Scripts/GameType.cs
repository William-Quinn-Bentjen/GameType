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
    public float GameTime = 0;
    

    // Used to give the gametype info when it's created
    public virtual void OnEnable()
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
            // End the game 
            EndGame();
        }
        else
        {
            // Count until the game ends
            while (true)
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
