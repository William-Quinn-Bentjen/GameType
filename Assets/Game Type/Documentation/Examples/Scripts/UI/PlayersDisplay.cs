using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayersDisplay : MonoBehaviour
{
    public Sprite Keyboard;
    public Sprite Controller;
    public Sprite None;
    public float verticalSpaceing;
    public PlayersPannel playersPannel;
    public PlayerDisplay playerDisplayPrefab;
    public List<PlayerDisplay> players;
    public List<PlayerInfo.PlayerData> playersData;

    [ContextMenu("AddPlayer")]
    public void Test()
    {
        AddPlayer(new PlayerInfo.PlayerData() { playerName = "test" });
    }
    public void AddPlayer(PlayerInfo.PlayerData newPlayerData)
    {
        PlayerDisplay newPlayer = Instantiate(playerDisplayPrefab, transform);
        newPlayer.playersDisplay = this;
        newPlayer.playersPannel = playersPannel;
        newPlayer.UpdateUI(newPlayerData);
        players.Add(newPlayer);
        playersData.Add(newPlayer.data);
        newPlayer.rectTransform.localPosition = newPlayer.rectTransform.localPosition + new Vector3(0, -((playerDisplayPrefab.rectTransform.rect.height * (players.Count - 1)) + (verticalSpaceing * (players.Count - 1))), 0);
        RectTransform pos = newPlayer.GetComponent<RectTransform>();// = newPlayer.transform.position - newPlayer.GetComponent<RectTransform>()
    }

    public void RemovePlayer(PlayerInfo.PlayerData playerData)
    {
        if (playersData.Contains(playerData))
        {
            int idx = playersData.IndexOf(playerData);
            PlayerDisplay display = players[idx];
            players.RemoveAt(idx);
            playersData.RemoveAt(idx);
            for (int i = idx; i < players.Count; i++)
            {
                //BUGGED
                Vector3 localPos = players[i].rectTransform.localPosition;
                localPos.y += display.rectTransform.rect.height + verticalSpaceing;
                players[i].rectTransform.localPosition = localPos;
            }
            Destroy(display.gameObject);
        }
    }
    public void RemoveAllPlayers()
    {
        for(int i = 0; i < players.Count; i++)
        {
            Destroy(players[i].gameObject);
        }
        players.Clear();
        playersData.Clear();
        GameManager.Instance.lobby.CanPlay();
    }
}
