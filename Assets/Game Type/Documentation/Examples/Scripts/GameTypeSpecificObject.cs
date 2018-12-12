using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTypeSpecificObject : MonoBehaviour {
    public enum GameTypeTag
    {
        All,
        FFASlayer,
        TeamSlayer,
        Infection,
        Race
    }
    public GameTypeTag gameTypeTag;
}
