using System;
using System.Collections.Generic;
using System.IO;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class FrenzyLevelVR : FrenzyLevel
{
    private void Awake()
    {
        Debug.Log($"Loading level {VrGameState.Instance.SelectedLevelName}");
        LoadLevelConfiguration(VrGameState.Instance.SelectedLevelName);
    }

    private void LoadLevelConfiguration(string levelName)
    {
        if(levelName == null) return;
        
        FrenzyLevelConfiguration.Load(this, levelName + ".json" , levelPrefabs);
    }

    public void SaveLevelConfiguration(string levelName)
    {
        FrenzyLevelConfiguration configuration = new FrenzyLevelConfiguration(this);
        configuration.SaveLevelToFile(levelName);
        Debug.Log($"Level configuration saved to {levelName}");
    }
}