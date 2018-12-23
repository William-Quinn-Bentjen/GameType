using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MapAndGameTypePannel : MonoBehaviour {
    public List<Map> maps = new List<Map>();
    public List<ExampleGameTypeIntegration> gameTypes = new List<ExampleGameTypeIntegration>();
    public Dropdown mapDropdown;
    public Dropdown gameTypeDropdown;
    private void Reset()
    {
        foreach (Dropdown dropdown in GetComponentsInChildren<Dropdown>())
        {
            switch(dropdown.name)
            {
                case "Map":
                    mapDropdown = dropdown;
                    dropdown.options.Clear();
                    dropdown.onValueChanged.RemoveAllListeners();
                    dropdown.onValueChanged.AddListener(delegate { DropDownsChanged(); });
                    break;
                case "GameType":
                    gameTypeDropdown = dropdown;
                    dropdown.options.Clear();
                    dropdown.onValueChanged.RemoveAllListeners();
                    dropdown.onValueChanged.AddListener(delegate { DropDownsChanged(); });
                    break;
            }
        }
    }
    void DropDownsChanged()
    {
        if (mapDropdown.value < maps.Count)
        {
            GameManager.Instance.map = maps[mapDropdown.value];
        }
        else
        {
            GameManager.Instance.map = null;
        }
        if (gameTypeDropdown.value < maps.Count)
        {
            GameManager.Instance.GameType = gameTypes[gameTypeDropdown.value];
        }
        else
        {
            GameManager.Instance.GameType = null;
        }
        GameManager.Instance.lobby.CanPlay();
    }
    private void Awake()
    {
        mapDropdown.options.Clear();
        gameTypeDropdown.options.Clear();
        GameManager.Instance.GameType = null;
        GameManager.Instance.map = null;
        for (int i = 0; i < maps.Count; i++)
        {
            if (maps[i] == null)
            {
                maps.RemoveAt(i);
                i--;
            }
            else
            {
                mapDropdown.options.Add(new Dropdown.OptionData(maps[i].sceneName));
            }
        }
        for (int i = 0; i < gameTypes.Count; i++)
        {
            if (gameTypes[i] == null)
            {
                gameTypes.RemoveAt(i);
                i--;
            }
            else
            {
                gameTypeDropdown.options.Add(new Dropdown.OptionData(gameTypes[i].name));
            }
        }
        gameTypeDropdown.value = 0;
        mapDropdown.value = 0;
        DropDownsChanged();
    }
}
