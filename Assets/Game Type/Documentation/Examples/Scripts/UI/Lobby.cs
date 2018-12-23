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
    public ExampleGameTypeIntegration gameType
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
    public void CanPlay()
    {
        if (GameManager.Instance.GameType != null)
        {
            IPlayerData playerDataList = GameManager.Instance.GameType as IPlayerData;
            if (playerDataList != null)
            {
                playerDataList.SetPlayerData(playersDisplay.playersData);
            }
            if (GameManager.Instance.GameType.CanStart())
            {
                playButton.interactable = true;
                return;
            }
        }
        playButton.interactable = false;
    }
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
    private void ShowLobbyUI()
    {
        gameObject.SetActive(true);
        if (gameType != null) gameType.GameState.States[ExtendedGameType.ExampleGameState.LeavingMap].OnStartInform = null;
        map.onLoaded -= HideLobbyUI;
    }
    public void Play()
    {
        if (gameType != null && map != null)
        {
            gameType.GameManager = GameManager.Instance;
            IPlayerData playerDataList = gameType as IPlayerData;
            
            if (playerDataList != null) playerDataList.SetPlayerData(playersDisplay.playersData);
            if (gameType.BeginGame())
            {
                map.onLoaded = HideLobbyUI;
                map.onLoaded += gameType.EnterMap;
                map.Load();
                gameType.GameState.States[ExtendedGameType.ExampleGameState.LeavingMap].OnStartInform = map.Unload;
                gameType.GameState.States[ExtendedGameType.ExampleGameState.LeavingMap].OnStartInform += ShowLobbyUI;
            }
        }
        else
        {
            Debug.LogWarning("GameType and map can't be null");
        }
    }
}
