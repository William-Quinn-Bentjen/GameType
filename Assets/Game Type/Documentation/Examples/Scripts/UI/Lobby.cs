using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    public int map;
    public PlayersDisplay playersDisplay;
    public PlayerInfo playerInfo;
    public Button playButton;
    private void Awake()
    {
        playersDisplay.AddPlayer(playersDisplay.playerDisplayPrefab.data);
        CanPlay();

    }
    public void Play()
    {
        if (gameType != null)
        {
            gameType.GameManagerMonoBehaviour = this;
            ExampleInterface.IPlayerData playerDataList = gameType as ExampleInterface.IPlayerData;
            
            if (playerDataList != null) playerDataList.SetPlayerData(playersDisplay.playersData);

            // Can start checks done inside begin game
            if (gameType.BeginGame())
            {
                if (map > 0 && map < SceneManager.sceneCountInBuildSettings) SceneManager.LoadSceneAsync(map);
            }
        }
        else
        {
            Debug.LogWarning("GameType can't be null");
        }
    }
    public void CanPlay()
    {
        playButton.interactable = gameType.CanStart();
    }
}
