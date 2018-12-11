using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour {
    
    public struct PlayerData
    {
        
        public string playerName;
        public Color personalColor;
        public Teams.Team teamPreference;
        public JengaPlayer.InputType inputType;
        public void SetPlayerFromData(JengaPlayer player)
        {
            player.name = playerName;
            player.personalColor = personalColor;
            player.team = teamPreference;
            player.input = inputType;
        }
        public PlayerData(JengaPlayer player)
        {
            playerName = player.name;
            personalColor = player.personalColor;
            teamPreference = player.team;
            inputType = JengaPlayer.InputType.keyboard;
        }
    }
    public InputField playerName;
    public Slider r;
    public Slider g;
    public Slider b;
    public Image personalColor;
    public Color color;
    public Dropdown teamPref;
    public Dropdown input;
    public Button cancel;
    public Button done;
    private void Reset()
    {
        playerName = GetComponentInChildren<InputField>();
        Slider[] sliders = GetComponentsInChildren<Slider>();
        foreach(Slider sli in sliders)
        {
            switch(sli.name)
            {
                case "R Slider":
                    r = sli;
                    sli.onValueChanged.RemoveAllListeners();
                    sli.onValueChanged.AddListener(delegate { UpdateColor(); });
                    break;
                case "G Slider":
                    g = sli;
                    sli.onValueChanged.RemoveAllListeners();
                    sli.onValueChanged.AddListener(delegate { UpdateColor(); });
                    break;
                case "B Slider":
                    b = sli;
                    sli.onValueChanged.RemoveAllListeners();
                    sli.onValueChanged.AddListener(delegate { UpdateColor(); });
                    break;
            }
        }
        Image[] images = GetComponentsInChildren<Image>();
        foreach(Image img in images)
        {
            if (img.name == "Output Color")
            {
                personalColor = img;
            }
        }
        Dropdown[] dropdowns = GetComponentsInChildren<Dropdown>();
        foreach(Dropdown dropdown in dropdowns)
        {
            switch(dropdown.name)
            {
                case "Team Pref":
                    teamPref = dropdown;
                    break;
                case "Input":
                    input = dropdown;
                    break;
            }
        }
        Button[] buttons = GetComponentsInChildren<Button>();
        foreach(Button button in buttons)
        {
            switch(button.name)
            {
                case "Complete":
                    done = button;
                    break;
                case "Cancel":
                    cancel = button;
                    break;
            }
        }
    }
    private void OnEnable()
    {
        foreach (Slider sli in new Slider[3] { r, g, b })
        {
            sli.onValueChanged.RemoveAllListeners();
            sli.onValueChanged.AddListener(delegate { UpdateColor(); });
        }
        UpdateColor();
    }
    public void UpdateColor()
    {
        personalColor.color = new Color(r.value, g.value, b.value, 1);
    }
}
