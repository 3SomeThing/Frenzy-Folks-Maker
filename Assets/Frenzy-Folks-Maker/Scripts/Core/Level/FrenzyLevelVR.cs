using System;
using UnityEngine;

[Serializable]
public class FrenzyLevelVR : FrenzyLevel
{
    private void Awake()
    {
        LoadLevelConfiguration(VrGameState.Instance.SelectedLevelName);
    }

    private void Start()
    {
        PlayerPositionSpawner.SpawnAndRotatePlayerPosition(new Vector3(-3, 3, 0), Quaternion.Euler(0, 90, 0));
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