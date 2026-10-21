using Unity.Netcode;
using UnityEngine;

public class VrGameState : MonoBehaviour
{
    public static VrGameState Instance;

    public string SelectedLevelName;
    public bool IsNewLevel;
    public bool HasSelection => IsNewLevel || !string.IsNullOrEmpty(SelectedLevelName);

    public void ResetState()
    {
        SelectedLevelName = null;
        IsNewLevel = false;
    }
    
    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}