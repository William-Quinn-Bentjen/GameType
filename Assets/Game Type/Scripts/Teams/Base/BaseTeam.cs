using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Teams
{
    namespace Base
    {
        [CreateAssetMenu(fileName = "BaseTeam", menuName = "Teams/Base/Team")]
        public class BaseTeam : Abstract.Team
        {
            /// <summary>
            /// Team's data such as name and color (excludes list of members)
            /// </summary>
            public BaseTeamData data;
            /// <summary>
            /// List of team members
            /// </summary>
            public List<BaseTeamMember> members = new List<BaseTeamMember>();
            /// <summary>
            /// Attempts to join the team
            /// </summary>
            /// <param name="member">Member trying to join the team</param>
            /// <returns></returns>
            public virtual bool Join(BaseTeamMember member)
            {
                //leave old team
                if (member.team != null && member.team != this)
                {
                    member.team.Leave(member);
                    //join new team
                    member.team = this;
                }
                //check if was on team members list
                if (!members.Contains(member))
                {
                    members.Add(member);
                    return true;
                }
                return false;
            }
            /// <summary>
            /// Leave the team
            /// </summary>
            /// <param name="member">Member trying to leave the team</param>
            public virtual void Leave(BaseTeamMember member)
            {
                if (member.team == this)
                {
                    members.Remove(member);
                    member.team = null;
                }
            }
            /// <summary>
            /// Tells all members to leave after telling them to leave the members list is cleared in case there are any members who didn't leave properly
            /// </summary>
            public virtual void KickAll()
            {
                for (int i = members.Count -1; i >= 0; i--)
                {
                    Leave(members[i]);
                }
                members.Clear();
            }
            /// <summary>
            /// Clears member list of null members
            /// </summary>
            public virtual void ClearNullMembers()
            {
                for (int i = 0; i < members.Count;)
                {
                    if (members[i] != null)
                    {
                        i++;
                    }
                    else
                    {
                        members.RemoveAt(i);
                    }
                }
            }
        }
    }
}

