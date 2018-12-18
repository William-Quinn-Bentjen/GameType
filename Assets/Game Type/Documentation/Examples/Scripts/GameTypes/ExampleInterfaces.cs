using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ExampleInterface
{
    public interface IPlayerData
    {
        List<PlayerInfo.PlayerData> GetPlayerData();
        void SetPlayerData(List<PlayerInfo.PlayerData> newPlayerData);
    }
}
