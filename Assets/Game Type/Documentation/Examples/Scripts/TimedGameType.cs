﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// place in Unity\Editor\Data\Resources\ScriptTemplates
[CreateAssetMenu(fileName = "TimedGameType", menuName = "GameType/TimedGameType")]
public class TimedGameType : GameType {

	// Use this for initialization
    protected override void OnEnablePreform()
    {
        base.OnEnablePreform();
    }
	
    // Used to Attempt to start the game
    public override bool BeginGame()
    {
        return base.BeginGame();
    }
	
    // Things like minimum player checks should be done here to determine if the game can start
    public override bool CanStart()
    {
		return base.CanStart();
    }
	
    // Called before StartGame
    public override void EnterMap()
    {
        
    }
	
    // Called at the begining of gameplay after the everthing is ready 
    public override void StartGame()
    {
		
    }
	
    // Called at the end of gameplay 
    // (things like score can be sent off or saved before players should load to the end screen)
    public override void EndGame()
    {
		
    }
	
    // Called after the game has ended and is the very last thing the gamemode does
    public override void LeaveMap()
    {
		
    }
}
