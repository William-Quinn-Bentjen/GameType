using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MapAndGameTypePannel : MonoBehaviour {
    public List<Map> maps = new List<Map>();
    public List<GameType> gameTypes = new List<GameType>();
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
                    break;
                case "GameType":
                    gameTypeDropdown = dropdown;
                    dropdown.options.Clear();
                    break;
            }
        }
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
        if (maps.Count > 0)
        {
            GameManager.Instance.map = maps[0];
        }
        if (gameTypes.Count > 0)
        {
            GameManager.Instance.GameType = gameTypes[0];
        }
    }
}
