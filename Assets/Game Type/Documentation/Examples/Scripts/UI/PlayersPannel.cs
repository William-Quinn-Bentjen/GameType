using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayersPannel : MonoBehaviour {
    [Tooltip("When a player is created these properties are copied")]
    public PlayerInfo.PlayerData newPlayerData = new PlayerInfo.PlayerData()
    {
        playerName = "",
        personalColor = Color.white,
        teamPreference = null,
        inputType = JengaPlayer.InputType.none
    };

    [Tooltip("Edit window copy of new player")]
    public PlayerInfo.PlayerData playerDataHolder;
    public PlayerInfo playerInfoPannel;
    public PlayersDisplay playersDisplay;

    public Button addPlayerButton;
    public Button removeAllPlayersButton;
    protected void NewPlayer()
    {
        playerInfoPannel.onCancel -= CancelNewPlayer;
        playerInfoPannel.onDone -= NewPlayer;
        playersDisplay.AddPlayer(playerInfoPannel.target);
    }
    protected void CancelNewPlayer()
    {
        playerInfoPannel.onCancel -= CancelNewPlayer;
        playerInfoPannel.onDone -= NewPlayer;
    }
    protected void DeletePlayer(PlayerInfo.PlayerData playerData)
    {
        playersDisplay.RemovePlayer(playerData);
    }
    public void AddPlayer()
    {
        PlayerInfo.PlayerData newPlayer = new PlayerInfo.PlayerData()
        {
            playerName = newPlayerData.playerName,
            personalColor = newPlayerData.personalColor,
            teamPreference = newPlayerData.teamPreference,
            inputType = newPlayerData.inputType
        };
        playerDataHolder = newPlayer;
        playerInfoPannel.OpenWindow(ref newPlayer);
        playerInfoPannel.onDone += NewPlayer;
        playerInfoPannel.onCancel += CancelNewPlayer;
    }
    public void RemoveAllPlayers()
    {
        playersDisplay.RemoveAllPlayers();
    }
    private void Awake()
    {
        if (playerInfoPannel != null)
        {
            playerInfoPannel.gameObject.SetActive(false);
        }
    }

    private void Reset()
    {
        playerInfoPannel = FindObjectOfType<PlayerInfo>();
        playersDisplay = FindObjectOfType<PlayersDisplay>();
        if (playerInfoPannel != null) playerInfoPannel.gameObject.SetActive(false);
    }
    public void ChangeAvalableTeams(List<Teams.Team> teams)
    {
        playerInfoPannel.SetAvalableTeams(teams);
        //make sure playerdata has only avalable teams

        //playerInfoPannel.avalableTeams
    }
}
