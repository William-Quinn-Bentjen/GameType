using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// A GameType with gametime and a gamestate built in
/// </summary>
public class ExtendedGameType : GameType
{
    /// <summary>
    /// keeps track of what state the game is in
    /// </summary>
    public StateMachine<ExampleGameState> GameState = new StateMachine<ExampleGameState>();


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
        GameState.ChangeState(ExampleGameState.EnteringMap);
    }
    // Called at the begining of gameplay after the everthing is ready 
    public override void StartGame()
    {
        CurrentRound = 0;
        GameState.ChangeState(ExampleGameState.Starting);
        GameManager.StartCoroutine(GameTimer());
        StartRound();
    }
    // Called at the end of gameplay 
    // (things like score can be sent off or saved before players should load to the end screen)
    public override void EndGame()
    {
        if (GameState.Key == ExampleGameState.InProgress)
        {
            GameState.ChangeState(ExampleGameState.Ending);
            GameManager.StopCoroutine(GameTimer());
        }
    }
    // Called after the game has ended and is the very last thing the gamemode does
    public override void LeaveMap()
    {
        GameState.ChangeState(ExampleGameState.LeavingMap);
    }
    // Game timer (this is basically the tick function for GameTime)
    public override IEnumerator GameTimer()
    {
        // Set the game time to 0 because the timer just started
        GameTimerValues.Time = 0;
        // 0 for no limit
        if (GameTimerValues.TimeLimit <= 0)
        {
            // Count until the game ends
            while (GameState.Key == ExampleGameState.InProgress)
            {
                GameTimerValues.Time += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
        }
        else
        {
            // Actual timer logic
            while (GameTimerValues.Time < GameTimerValues.TimeLimit)
            {
                GameTimerValues.Time += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
            if (GameState.Key == ExampleGameState.InProgress)
            {   // End the game 
                EndGame();
            }
        }
    }
    public virtual IEnumerator RoundTimer()
    {
        if (RoundTimerValues.TimeLimit <= 0)
        {
            while (true)
            {
                RoundTimerValues.Time += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
        }
        else
        {
            while (RoundTimerValues.Time < RoundTimerValues.TimeLimit)
            {
                RoundTimerValues.Time += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
            EndRound();
        }
    }

    //rounds
    public TimerValues RoundTimerValues = new TimerValues();
    [Header("Read Only")]
    public int CurrentRound = 0;

    public virtual void EndRound()
    {
        GameManager.StopCoroutine(RoundTimer());
    }
    public virtual void StartRound()
    {
        GameState.ChangeStateIf(ExampleGameState.Starting, ExampleGameState.InProgress);
        GameManager.StartCoroutine(RoundTimer());
        CurrentRound++;
    }





    public class StateMachine<T>
    {
        /// <summary>
        /// A dictionary of states and keys
        /// </summary>
        public Dictionary<T, State> States;
        protected State _currentState;
        /// <summary>
        /// the current state of the state machine
        /// </summary>
        public State CurrentState
        {
            get
            {
                return _currentState;
            }
            set
            {
                if (value != _currentState)
                {
                    if (_currentState != null && _currentState.OnEnd != null)_currentState.OnEnd(_currentState, value);
                    State tempState = _currentState;
                    _currentState = value;
                    if (value != null)
                    {
                        if (_currentState != null && _currentState.OnStart != null) _currentState.OnStart(tempState, _currentState);
                    }
                }
            }
        }

        /// <summary>
        /// the current state's key (returns key default if current state is null)
        /// </summary>
        public T Key
        {
            get
            {
                if (_currentState != null && _currentState.Key != null)
                {
                    return _currentState.Key;
                }
                return default(T);
            }
            set
            {
                if (States.ContainsKey(value))
                {
                    CurrentState = States[value];
                }
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
            if (states.ContainsKey(currentState.Key) == false) AddState(currentState);
            _currentState = currentState;
            if (ignoreOnStart == false && currentState != null && currentState.OnStart != null) currentState.OnStart(null, currentState);
            
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
                if (_currentState == oldState)
                {
                    ChangeState(newState, false, ignoreOnEndIfCurrent);
                }
                States.Remove(oldState.Key);
            }
        }
        /// <summary>
        /// Use to chage between states
        /// </summary>
        /// <param name="newState">The state you wish to chage to</param>
        public void ChangeState(State newState, bool ignoreOnStart = false, bool ignoreOnEnd = false)
        {
            if (_currentState != null && newState != null)
            {
                if (States.ContainsKey(newState.Key) == false && States.ContainsValue(newState) == false)
                {
                    States.Add(newState.Key, newState);
                }
                if (ignoreOnStart == false && _currentState.OnEnd != null) _currentState.OnEnd(_currentState, newState);
                State oldState = _currentState;
                _currentState = newState;
                if (ignoreOnEnd == false && _currentState.OnStart != null) _currentState.OnStart(oldState, _currentState);
            }
        }
        public void ChangeState(T newStateKey, bool ignoreOnStart = false, bool ignoreOnEnd = false)
        {
            if (States.ContainsKey(newStateKey))
            {
                ChangeState(States[newStateKey], ignoreOnStart, ignoreOnEnd);
            }
        }
        public bool ChangeStateIf(State currentState, State newState)
        {
            if (_currentState == currentState)
            {
                ChangeState(newState);
                return true;
            }
            return false;
        }
        public bool ChangeStateIf(T currentState, T newState)
        {
            if (States.ContainsKey(currentState))
            {
                ChangeState(newState);
                return true;
            }
            return false;
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