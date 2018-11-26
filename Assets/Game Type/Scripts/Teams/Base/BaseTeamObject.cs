using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Teams
{
    namespace Base
    {
        public class BaseTeamObject : MonoBehaviour, ITeam
        {
            public BaseTeam team;
            public virtual BaseTeam GetTeam()
            {
                return team;
            }
        }
    }
}