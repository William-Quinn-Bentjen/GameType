using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Teams
{
    public class TeamObject : MonoBehaviour, ITeam
    {
        public Team team;
        public virtual Team GetTeam()
        {
            return team;
        }
    }
    public interface ITeam
    {
        Team GetTeam();
    }
}