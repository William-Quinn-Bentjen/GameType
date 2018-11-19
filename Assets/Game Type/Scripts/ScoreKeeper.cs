using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : ScriptableObject {
    public Dictionary<Teams.Base.BaseTeam, float> scoreKeeper = new Dictionary<Teams.Base.BaseTeam, float>();
}
