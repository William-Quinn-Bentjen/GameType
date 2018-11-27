﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// A GameType with gametime and a gamestate built in
/// </summary>
public class ExtendedGameType : GameType
{

    // Enums and structs
    /// <summary>
    /// States the game may be in
    /// </summary>
    public enum ExampleGameState
    {
        Lobby,
        EnteringMap,
        Starting,
        InProgress,
        Ending,
        LeavingMap,
        Aborted
    }
    /// <summary>
    /// keeps track of what state the game is in
    /// </summary>
    public StateMachine<ExampleGameState> GameState = new StateMachine<ExampleGameState>();

    public override void OnEnable()
    {
        StateMachine<ExampleGameState>.State Lobby = new StateMachine<ExampleGameState>.State(ExampleGameState.Lobby);
        Dictionary<ExampleGameState, StateMachine<ExampleGameState>.State> states = new Dictionary<ExampleGameState, StateMachine<ExampleGameState>.State>();
        states.Add(ExampleGameState.Lobby, Lobby);
        states.Add(ExampleGameState.EnteringMap, new StateMachine<ExampleGameState>.State(ExampleGameState.EnteringMap));
        states.Add(ExampleGameState.Starting, new StateMachine<ExampleGameState>.State(ExampleGameState.Starting));
        states.Add(ExampleGameState.InProgress, new StateMachine<ExampleGameState>.State(ExampleGameState.InProgress));
        states.Add(ExampleGameState.Ending, new StateMachine<ExampleGameState>.State(ExampleGameState.Ending));
        states.Add(ExampleGameState.LeavingMap, new StateMachine<ExampleGameState>.State(ExampleGameState.LeavingMap));
        states.Add(ExampleGameState.Aborted, new StateMachine<ExampleGameState>.State(ExampleGameState.Aborted));
        GameState = new StateMachine<ExampleGameState>(states, Lobby);
    }
    // Called before StartGame
    public override void EnterMap()
    {
        if (GameState.States.ContainsKey(ExampleGameState.EnteringMap)) GameState.StateChange(ExampleGameState.EnteringMap);
    }
    // Called at the begining of gameplay after the everthing is ready 
    public override void StartGame()
    {
        if (GameState.States.ContainsKey(ExampleGameState.Starting)) GameState.StateChange(ExampleGameState.Starting);
        if (GameState.States.ContainsKey(ExampleGameState.InProgress)) GameState.StateChange(ExampleGameState.InProgress);
        GameManager.StartCoroutine(GameTimer());
    }
    // Called at the end of gameplay 
    // (things like score can be sent off or saved before players should load to the end screen)
    public override void EndGame()
    {
        base.EndGame();
        if (GameState.States.ContainsKey(ExampleGameState.Ending)) GameState.StateChange(ExampleGameState.Ending);
    }
    // Called after the game has ended and is the very last thing the gamemode does
    public override void LeaveMap()
    {
        if (GameState.States.ContainsKey(ExampleGameState.LeavingMap)) GameState.StateChange(ExampleGameState.LeavingMap);
    }
    // Game timer (this is basically the tick function for GameTime)
    public override IEnumerator GameTimer()
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
            if (GameState.Key == ExampleGameState.InProgress)
            {   // End the game 
                EndGame();
            }
            
        }
        else
        {
            // Count until the game ends
            while (GameState.Key == ExampleGameState.InProgress)
            {
                GameTime += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
        }
    }






    public class StateMachine<T>
    {
        /// <summary>
        /// A dictionary of states and keys
        /// </summary>
        public Dictionary<T, State> States = new Dictionary<T, State>();
        /// <summary>
        /// the current state of the state machine
        /// </summary>
        public State CurrentState = null;
        /// <summary>
        /// the current state's key (returns key default if current state is null)
        /// </summary>
        public T Key
        {
            get
            {
                if (CurrentState != null && CurrentState.Key != null)
                {
                    return CurrentState.Key;
                }
                return default(T);
            }

        }
        public StateMachine()
        {
            States = new Dictionary<T, State>();
        }
        /// <summary>
        /// Create state machine with states and a state to 
        /// </summary>
        /// <param name="states">the dictionary of states for the state machine</param>
        /// <param name="currentState">the state the statemachine should start in</param>
        public StateMachine(Dictionary<T, State> states, State currentState, bool ignoreOnStart = false)
        {
            States = states;
            AddState(currentState);
            if (ignoreOnStart == false && currentState != null && currentState.OnStart != null) currentState.OnStart(null, currentState);
            CurrentState = currentState;
        }
        /// <summary>
        /// Ensure state is added properly
        /// </summary>
        /// <param name="newState"></param>
        public void AddState(State newState)
        {
            if (States.ContainsKey(newState.Key) == false && States.ContainsValue(newState) == false)
            {
                States.Add(newState.Key, newState);
            }
        }
        /// <summary>
        /// Used to remove a state from the state machine
        /// </summary>
        /// <param name="oldState">The state to be removed</param>
        /// <param name="newState">A new state to set as current if the old state is the current state</param>
        /// <param name="ignoreOnEndIfCurrent">should the state being removed preform OnEnd?</param>
        public void RemoveState(State oldState, State newState = null, bool ignoreOnEndIfCurrent = false)
        {
            if (States.ContainsKey(oldState.Key))
            {
                if (CurrentState == oldState)
                {
                    StateChange(newState, false, ignoreOnEndIfCurrent);
                }
                States.Remove(oldState.Key);
            }
        }
        /// <summary>
        /// Use to chage between states
        /// </summary>
        /// <param name="newState">The state you wish to chage to</param>
        public void StateChange(State newState, bool ignoreOnStart = false, bool ignoreOnEnd = false)
        {
            if (CurrentState != null && newState != null)
            {
                if (States.ContainsKey(newState.Key) == false && States.ContainsValue(newState) == false)
                {
                    States.Add(newState.Key, newState);
                }
                if (ignoreOnStart == false && CurrentState.OnEnd != null) CurrentState.OnEnd(CurrentState, newState);
                State oldState = CurrentState;
                CurrentState = newState;
                if (ignoreOnEnd == false && CurrentState.OnStart != null) CurrentState.OnStart(oldState, CurrentState);
            }
        }
        public void StateChange(T newStateKey, bool ignoreOnStart = false, bool ignoreOnEnd = false)
        {
            if (States.ContainsKey(newStateKey))
            {
                StateChange(States[newStateKey], ignoreOnStart, ignoreOnEnd);
            }
        }
        /// <summary>
        /// A state with on start and on end as well as a key
        /// </summary>
        /// <typeparam name="T">the key of the state</typeparam>
        public class State
        {
            /// <summary>
            /// the key of the state
            /// </summary>
            public T Key;

            public delegate void OnStateChange(State oldState, State newState);
            /// <summary>
            /// Called when the state starts
            /// </summary>
            public OnStateChange OnStart;
            /// <summary>
            /// Called when the state ends
            /// </summary>
            public OnStateChange OnEnd;

            public State(T newKey)
            {
                Key = newKey;
            }
        }
    }
}