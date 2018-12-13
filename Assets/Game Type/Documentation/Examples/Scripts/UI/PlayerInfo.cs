
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour {
    [System.Serializable]
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
        public Button remove;
    }
    public UIComponents UI;
    public PlayerData target;
    public PlayerDisplay targetDisplay;
    public List<Teams.Team> avalableTeams = new List<Teams.Team>();
    public delegate void OnAction();
    public OnAction onDone;
    public OnAction onCancel;
    public OnAction onOpen;
    public OnAction onClose;
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
                    UI.input.options.Add(new Dropdown.OptionData("None"));
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
                case "Remove Player":
                    UI.remove = button;
                    button.gameObject.SetActive(false);
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
        UpdateColor();
        target = new PlayerData();
    }
    public void UpdateColor()
    {
        UI.personalColor.color = new Color(UI.r.value, UI.g.value, UI.b.value, 1);
    }
    public void SetAvalableTeams(List<Teams.Team> teams)
    {
        UI.teamPref.options.Clear();
        avalableTeams.Clear();
        if (teams != null)
        {
            List<Dropdown.OptionData> newOptions = new List<Dropdown.OptionData>();
            foreach (Teams.Team team in teams)
            {
                if (team.data != null)
                {
                    newOptions.Add(new Dropdown.OptionData(team.data.TeamName));
                    avalableTeams.Add(team);
                }
            }
            UI.teamPref.options = newOptions;
        }
    }
    public void Done()
    {
        UI.remove.onClick.RemoveListener(RemovePlayer);
        //finalize
        target.playerName = UI.playerName.text;
        target.personalColor = new Color(UI.r.value, UI.g.value, UI.b.value);
        target.inputType = (JengaPlayer.InputType)UI.input.value;
        if (avalableTeams.Count != 0)
        {
            target.teamPreference = avalableTeams[UI.teamPref.value];
        }
        onDone?.Invoke();
        CloseWindow();
    }
    public void Cancel()
    {
        UI.remove.onClick.RemoveListener(RemovePlayer);
        onCancel?.Invoke();
        CloseWindow();
    }
    public void OpenWindow(ref PlayerData playerData)
    {
        UI.remove.gameObject.SetActive(false);
        target = playerData;
        UI.playerName.text = target.playerName;
        UI.r.value = target.personalColor.r;
        UI.g.value = target.personalColor.g;
        UI.b.value = target.personalColor.b;
        if (avalableTeams.Contains(playerData.teamPreference))
        {
            UI.teamPref.value = avalableTeams.IndexOf(playerData.teamPreference);
        }
        else
        {
            UI.teamPref.value = 0;
        }
        UI.input.value = (int)playerData.inputType;
        onOpen?.Invoke();
        gameObject.SetActive(true);

    }
    public void OpenEditWindow(PlayerDisplay playerDisplay)
    {
        targetDisplay = playerDisplay;
        onDone += playerDisplay.UpdateUI;
        OpenWindow(ref playerDisplay.data);
        UI.remove.gameObject.SetActive(true);
        UI.remove.onClick.AddListener(RemovePlayer);
    }
    public void CloseWindow()
    {
        onDone = null;
        onCancel = null;
        gameObject.SetActive(false);
        onClose?.Invoke();
}
    public void RemovePlayer()
    {
        UI.remove.onClick.RemoveListener(RemovePlayer);
        if (targetDisplay != null)
        {
            targetDisplay.playersDisplay.RemovePlayer(targetDisplay.data);
        }
        CloseWindow();
    }
}
