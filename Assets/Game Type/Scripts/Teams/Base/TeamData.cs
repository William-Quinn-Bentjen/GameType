using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Teams
{
    [CreateAssetMenu(fileName = "BaseTeamData", menuName = "Team/Base/Data")]
    public class TeamData : ScriptableObject
    {
        public string TeamName = "";
        public Color TeamColor = Color.clear;
    }
}