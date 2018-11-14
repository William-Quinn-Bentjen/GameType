using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameType", menuName = "GameTypes/Base/GameType")]
public class GameType : ScriptableObject {

    /// <summary>
    /// Keeps the points and teams for this GameType
    /// </summary>
    public PointKeeper Points;
    /// <summary>
    /// used to keep track of if the game has started 
    /// </summary>
    public bool gameStarted = false;
    /// <summary>
    /// Starts the game
    /// </summary>
    public virtual void GameBegin()
    {
#if UNITY_EDITOR
        if (gameStarted)
        {
            Debug.LogWarning("GameType can not start because it has already started");
        }
#endif
        gameStarted = true;
    }
    /// <summary>
    /// Ends the game
    /// </summary>
    public virtual void GameOver()
    {

#if UNITY_EDITOR
        if (gameStarted == false)
        {
            Debug.LogWarning("GameType can not end because it hasn't started");
        }
#endif
        gameStarted = false;
    }
    /// <summary>
    /// Add a team to the GameType's point keeper
    /// </summary>
    /// <param name="team">The team to be added to the point keeper</param>
    /// <param name="startingPoints">The points the team should start with</param>
    /// <returns>returns true if team was successfully added or false if the team already exists in the GameType's point keeper</returns>
    public virtual bool AddTeam(Teams.Base.BaseTeam team, float startingPoints = 0)
    {
        if (Points.ContainsTeam(team))
        {
            return false;
        }
        Points.SetPoints(team, startingPoints);
        return true;
    }
    /// <summary>
    /// Used to add points to a team (note logic may be added here to end the game after a certain amount of points is reached)
    /// </summary>
    /// <param name="team">The team to add points to</param>
    /// <param name="pointsToAdd">How many points to add</param>
    public virtual void AddPoints(Teams.Base.BaseTeam team, float pointsToAdd = 0)
    {
        Points.AddPoints(team, pointsToAdd);
    }
    /// <summary>
    /// Used to set the amount of points a team has
    /// </summary>
    /// <param name="team">The team whos points will be set</param>
    /// <param name="newPointsValue">The amount of points the team should now have</param>
    public virtual void SetPoints(Teams.Base.BaseTeam team, float newPointsValue)
    {
        Points.SetPoints(team, newPointsValue);
    }
    private void OnEnable()
    {
        Points = CreateInstance<PointKeeper>();
    }
}
