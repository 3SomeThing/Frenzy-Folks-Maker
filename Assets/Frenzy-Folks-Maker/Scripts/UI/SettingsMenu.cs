using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;

public class SettingsMenu : NestedMenus
{
    [SerializeField] private GameObject confirmationWindow;
    [SerializeField] private TooltipUI _tooltipUI;
    
    public void TrySaveLevelButton(TMP_InputField name)
    {
        if(name != null && string.IsNullOrWhiteSpace(name.text))
        {
            _tooltipUI.Show("Please enter a valid name.", name.transform as RectTransform);
            _tooltipUI.HideAfterDelay(2f);
            return;
        }
        
        TrySaveLevel(name.text);
    }
    
    private void TrySaveLevel(string name)
    {
        name = name + ".json";
        
        if(File.Exists(Path.Combine(FrenzyLevelConfiguration.LevelsPath, name)))
        {
            OpenMenu(confirmationWindow);
        }
        else
        {
            SaveLevel(name);
        }
    }
    
    public void AutoFillLevelName(TMP_InputField name)
    {
        if(VrGameState.Instance.SelectedLevelName != null && !string.IsNullOrWhiteSpace(VrGameState.Instance.SelectedLevelName))
        {
            name.text = VrGameState.Instance.SelectedLevelName;
        }
    }

    public void SaveLevel(TMP_InputField name)
    {
        SaveLevel(name.text + ".json");
    }

    private void SaveLevel(string name)
    {
        FindAnyObjectByType<FrenzyLevel>().SaveLevelConfiguration(name);
        
        CloseAllSubMenus();
    }
    
    public void ReturnToMenu(string scene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
        
        CloseAllMenus();
        gameObject.SetActive(false);
    }
}
