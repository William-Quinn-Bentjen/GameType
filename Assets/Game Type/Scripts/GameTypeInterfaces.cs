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
        public interface IFFA
        {
            bool IsFFA();
        }
    }
}
