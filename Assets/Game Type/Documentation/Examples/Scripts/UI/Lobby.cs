using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Lobby : MonoBehaviour {
    public static Lobby Instance;
    public bool isEnabled
    {
        get
        {
            return gameObject.activeInHierarchy;
        }
        set
        {
            if (value != gameObject.activeInHierarchy)
            {
                gameObject.SetActive(value);
            }
        }
    }
    public GameType gameType
    {
        get
        {
            return GameManager.Instance.GameType;
        }
        set
        {
            GameManager.Instance.GameType = value;
        }
    }
    public Map map
    {
        get
        {
            return GameManager.Instance.map;
        }
        set
        {
            GameManager.Instance.map = value;
        }
    }
    public PlayersDisplay playersDisplay;
    public PlayerInfo playerInfo;
    public Button playButton;
    private void Awake()
    {
        playersDisplay.AddPlayer(playersDisplay.playerDisplayPrefab.data);
        CanPlay();

    }
    private void HideLobbyUI()
    {
        gameObject.SetActive(false);
        map.onLoaded -= HideLobbyUI;
    }
    public void Play()
    {
        if (gameType != null && map != null)
        {
            gameType.GameManagerMonoBehaviour = this;
            ExampleInterface.IPlayerData playerDataList = gameType as ExampleInterface.IPlayerData;
            
            if (playerDataList != null) playerDataList.SetPlayerData(playersDisplay.playersData);
            if (gameType.BeginGame())
            {
                map.onLoaded += HideLobbyUI;
                map.Load();
            }
        }
        else
        {
            Debug.LogWarning("GameType can't be null");
        }
    }
}
