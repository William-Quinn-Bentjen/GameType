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
}
