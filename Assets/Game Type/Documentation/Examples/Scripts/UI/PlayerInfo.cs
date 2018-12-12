
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
    [System.Serializable]
    public struct UIComponents
    {
        public InputField playerName;
        public Slider r;
        public Slider g;
        public Slider b;
        public Image personalColor;
        public Dropdown teamPref;
        public Dropdown input;
        public Button cancel;
        public Button done;
    }
    public UIComponents UI;

    //public InputField UI.playerName;
    //public Slider UI.r;
    //public Slider UI.g;
    //public Slider UI.b;
    //public Image UI.personalColor;
    //public Dropdown UI.teamPref;
    //public Dropdown UI.input;
    //public Button UI.cancel;
    //public Button UI.done;
    private void Reset()
    {
        UI.playerName = GetComponentInChildren<InputField>();
        Slider[] sliders = GetComponentsInChildren<Slider>();
        foreach(Slider sli in sliders)
        {
            switch(sli.name)
            {
                case "R Slider":
                    UI.r = sli;
                    sli.onValueChanged.RemoveAllListeners();
                    sli.onValueChanged.AddListener(delegate { UpdateColor(); });
                    break;
                case "G Slider":
                    UI.g = sli;
                    sli.onValueChanged.RemoveAllListeners();
                    sli.onValueChanged.AddListener(delegate { UpdateColor(); });
                    break;
                case "B Slider":
                    UI.b = sli;
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
                UI.personalColor = img;
            }
        }
        Dropdown[] dropdowns = GetComponentsInChildren<Dropdown>();
        foreach(Dropdown dropdown in dropdowns)
        {
            switch(dropdown.name)
            {
                case "Team Pref":
                    UI.teamPref = dropdown;
                    UI.teamPref.options.Clear();
                    break;
                case "Input":
                    UI.input = dropdown;
                    UI.input.options.Clear();
                    UI.input.options.Add(new Dropdown.OptionData("Keyboard"));
                    UI.input.options.Add(new Dropdown.OptionData("Controller 1"));
                    UI.input.options.Add(new Dropdown.OptionData("Controller 2"));
                    UI.input.options.Add(new Dropdown.OptionData("Controller 3"));
                    UI.input.options.Add(new Dropdown.OptionData("Controller 4"));
                    break;
            }
        }
        Button[] buttons = GetComponentsInChildren<Button>();
        foreach(Button button in buttons)
        {
            switch(button.name)
            {
                case "Complete":
                    UI.done = button;
                    UI.done.onClick.AddListener(new UnityEngine.Events.UnityAction(Done));
                    break;
                case "Cancel":
                    UI.cancel = button;
                    break;
            }
        }
    }
    private void OnEnable()
    {
        foreach (Slider sli in new Slider[3] { UI.r, UI.g, UI.b })
        {
            sli.onValueChanged.RemoveAllListeners();
            sli.onValueChanged.AddListener(delegate { UpdateColor(); });
        }
        UI.done.onClick.AddListener(new UnityEngine.Events.UnityAction(Done));
        UI.cancel.onClick.AddListener(new UnityEngine.Events.UnityAction(Cancel));
        UpdateColor();
    }
    public void UpdateColor()
    {
        UI.personalColor.color = new Color(UI.r.value, UI.g.value, UI.b.value, 1);
    }
    public void SetAvalableTeams(List<Teams.Team> teams)
    {
        foreach (Teams.Team team in teams)
        {

        }
    }
    public void Done()
    {
        //finalize
        gameObject.SetActive(false);
    }
    public void Cancel()
    {
        gameObject.SetActive(false);
    }
    
}
