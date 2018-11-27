using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinUI : MonoBehaviour {
    public Image pannel;
    public Text text;
    private void Reset()
    {
        pannel = GetComponent<Image>();
        text = GetComponentInChildren<Text>();
    }
    // Use this for initialization
    void Awake () {
        pannel.gameObject.SetActive(false);
        text.text = "";
	}
    public void SetWinnerText(Teams.Team team)
    {
        text.color = team.data.TeamColor;
        text.text = "Winner\n" + team.data.TeamName;
        pannel.gameObject.SetActive(true);
    }
}
