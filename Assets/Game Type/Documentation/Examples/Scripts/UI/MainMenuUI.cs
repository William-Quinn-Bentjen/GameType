using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {
    public Dropdown dropdown;
    private void Awake()
    {
        dropdown.options.Clear();
        List<Dropdown.OptionData> data = new List<Dropdown.OptionData>();
        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            data.Add(new Dropdown.OptionData(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i))));
        }
        dropdown.options = data;
    }
    public void ChangeScene()
    {
        SceneManager.LoadScene(dropdown.value + 1);
    }
}