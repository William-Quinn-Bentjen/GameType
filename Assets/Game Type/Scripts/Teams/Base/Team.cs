using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Teams
{
    [CreateAssetMenu(fileName = "Team", menuName = "Team/Base/Team")]
    public class Team : ScriptableObject
    {
        /// <summary>
        /// Team's data such as name and color (excludes list of members)
        /// </summary>
        public TeamData data;
        /// <summary>
        /// List of team members
        /// </summary>
        public List<TeamMember> members = new List<TeamMember>();
        public delegate void OnSuccessfullMember(TeamMember member);
        public OnSuccessfullMember OnSuccessfulJoin;
        public OnSuccessfullMember OnSuccessfulLeave;
        /// <summary>
        /// Attempts to join the team
        /// </summary>
        /// <param name="member">Member trying to join the team</param>
        /// <returns></returns>
        public virtual bool Join(TeamMember member)
        {
            //leave old team
            if (member.team != null && member.team != this)
            {
                member.team.Leave(member);
            }
            //check if was on team members list
            if (!members.Contains(member))
            {
                members.Add(member);
            }
            //join new team
            member.team = this;
            //tell delegates it's joining after it's on the member list and has had it's screen changed
            if (OnSuccessfulJoin != null) OnSuccessfulJoin(member);
            return true;

        }
        /// <summary>
        /// Leave the team
        /// </summary>
        /// <param name="member">Member trying to leave the team</param>
        public virtual void Leave(TeamMember member)
        {
            if (member.team == this)
            {
                members.Remove(member);
                //tell delegates it's leaving before it leaves so it can see the team it's leaving
                if (OnSuccessfulLeave != null) OnSuccessfulLeave(member);
                member.team = null;
            }
        }
        /// <summary>
        /// Tells all members to leave after telling them to leave the members list is cleared in case there are any members who didn't leave properly
        /// </summary>
        public virtual void KickAll()
        {
            for (int i = members.Count - 1; i >= 0; i--)
            {
                if (members[i] != null)
                {
                    Leave(members[i]);
                }
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

