using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Teams
{
    namespace Base
    {
        [AddComponentMenu("Teams/BaseTeamMember")]
        public class BaseTeamMember : BaseTeamObject
        {
            [System.Serializable]
            public class JoinOn
            {
                public bool Awake = true;
                public bool Enable = true;
            }
            [System.Serializable]
            public class LeaveOn
            {
                public bool Destory = true;
                public bool Disable = true;
            }
            public JoinOn joinOn;
            public LeaveOn leaveOn;
            protected virtual void Awake()
            {
                if (joinOn.Awake)
                {
                    Join(team);
                }
            }
            protected virtual void OnEnable()
            {
                if (joinOn.Enable)
                {
                    Join(team);
                }
            }
            protected virtual void OnDisable()
            {
                if (leaveOn.Disable)
                {
                    Leave();
                }
            }
            protected virtual void OnDestroy()
            {
                if (leaveOn.Destory)
                {
                    Leave();
                }
            }
            /// <summary>
            /// Joins a team 
            /// </summary>
            /// <param name="teamToJoin">joins this team (if left null will attempt to join the team in this component's team variable</param>
            /// <returns>true if successfully joined the team</returns>
            protected virtual bool Join(Teams.Base.BaseTeam teamToJoin = null)
            {
                if (teamToJoin != null)
                {
                    if (team != null)
                    {
                        team.Leave(this);
                    }
                    return teamToJoin.Join(this);
                }
                else
                {
                    if (team != null)
                    {
                        return team.Join(this);
                    }
                }
                return false;
            }
            /// <summary>
            /// Leaves whatever team you are currently a part of
            /// </summary>
            /// <returns>true if successfully left the team</returns>
            protected virtual bool Leave()
            {
                if (team != null)
                {
                    team.Leave(this);
                }
                return (team == null);
            }
        }
    }
}

