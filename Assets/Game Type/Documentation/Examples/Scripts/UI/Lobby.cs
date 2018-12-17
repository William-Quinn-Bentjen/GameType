using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Lobby : MonoBehaviour {
    public static Lobby Instance;
    public GameType gameType;
    public PlayersDisplay playersDisplay;
    public PlayerInfo playerInfo;
    public Button playButton;
    public void Play()
    {
        if (gameType != null)
        {
            // Can start checks done inside begin game
            gameType.BeginGame();
            // testing
            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("GameType can't be null");
        }
    }
}
