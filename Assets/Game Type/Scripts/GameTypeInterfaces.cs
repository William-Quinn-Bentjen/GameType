using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GameTypes
{
    namespace Interfaces
    {
        public interface IPlayers
        {
            List<Teams.TeamMember> GetPlayers();
            void SetPlayers(List<Teams.TeamMember> playersList);
        }
        public interface ITeams
        {
            List<Teams.Team> GetTeams();
            void SetTeams(List<Teams.Team> teams);
        }
        public interface IFFA
        {
            bool IsFFA();
        }

    }
}
