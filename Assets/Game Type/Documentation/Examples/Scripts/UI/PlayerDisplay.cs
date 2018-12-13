using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDisplay : MonoBehaviour {
    public PlayerInfo.PlayerData data = new PlayerInfo.PlayerData();
    public RectTransform rectTransform;
    public Button edit;

    public Image pannel;
    public PlayersPannel playersPannel;
    public PlayersDisplay playersDisplay;

    public Text playerName;
    public Text team;
    public Image input;

    public void Edit()
    {
        playersPannel.playerInfoPannel.OpenEditWindow(this);
    }
    public void UpdateUI(PlayerInfo.PlayerData newPlayerData)
    {
        data = newPlayerData;
        UpdateUI();
    }
    [ContextMenu("Update")]
    public void UpdateUI()
    {
        playersPannel.playerInfoPannel.onDone -= UpdateUI;
        if (data.teamPreference != null && data.teamPreference.data != null)
        {
            team.text = data.teamPreference.data.TeamName;
            Color color = data.teamPreference.data.TeamColor;
            color.a = 1;
            pannel.color = color;
        }
        else
        {
            team.text = "";
            pannel.color = new Color(1, 1, 1, 0.3921569f);
        }
        playerName.text = data.playerName;
        switch(data.inputType)
        {
            case JengaPlayer.InputType.none:
                input.sprite = playersDisplay.None;
                input.rectTransform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case JengaPlayer.InputType.keyboard:
                input.sprite = playersDisplay.Keyboard;
                input.rectTransform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case JengaPlayer.InputType.controller1:
                input.sprite = playersDisplay.Controller;
                input.rectTransform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case JengaPlayer.InputType.controller2:
                input.sprite = playersDisplay.Controller;
                input.rectTransform.rotation = Quaternion.Euler(0, 0, -90);
                break;
            case JengaPlayer.InputType.controller3:
                input.sprite = playersDisplay.Controller;
                input.rectTransform.rotation = Quaternion.Euler(0, 0, -270);
                break;
            case JengaPlayer.InputType.controller4:
                input.sprite = playersDisplay.Controller;
                input.rectTransform.rotation = Quaternion.Euler(0, 0, -180);
                break;
        }
    }
    private void Reset()
    {
        edit = GetComponentInChildren<Button>();
        pannel = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        foreach(Transform child in transform)
        {
            switch(child.name)
            {
                case "Input":
                    input = child.GetComponent<Image>();
                    break;
                case "Name":
                    playerName = child.GetComponent<Text>();
                    break;
                case "Team":
                    team = child.GetComponent<Text>();
                    break;
            }
        }
    }
}
