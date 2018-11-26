﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Teams
{
    namespace Base
    {
        [CreateAssetMenu(fileName = "BaseTeamData", menuName = "Team/Base/Data")]
        public class BaseTeamData : ScriptableObject
        {
            public string TeamName = "";
            public Color TeamColor = Color.clear;
        }
    }
}