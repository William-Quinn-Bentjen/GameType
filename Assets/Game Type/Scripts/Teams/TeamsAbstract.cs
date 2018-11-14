using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Teams
{
    namespace Base
    {
        public interface ITeam
        {
            Teams.Base.BaseTeam GetTeam();
        }
    }
    namespace Abstract
    {
        public abstract class Team : ScriptableObject { };
        public abstract class TeamData : ScriptableObject { };
        public abstract class TeamObject : MonoBehaviour { };
        public abstract class TeamManager : MonoBehaviour { };
    }
}
