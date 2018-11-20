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
            public delegate void MemberDelegate(BaseTeamMember member);
            public MemberDelegate OnJoin;
            public MemberDelegate OnLeave;
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
                BaseTeam teamToBeJoined = teamToJoin ?? team;
                if (teamToBeJoined != null)
                {
                    if (GameManager.Instance.GameType != null)
                    {
                        if (GameManager.Instance.GameType.AttemptJoin(teamToBeJoined, this))
                        {
                            if (OnJoin != null) OnJoin(this);
                            return true;
                        }
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
                    GameManager.Instance.GameType.AttemptLeave(this);
                }
                return (team == null);
            }
        }
    }
}

