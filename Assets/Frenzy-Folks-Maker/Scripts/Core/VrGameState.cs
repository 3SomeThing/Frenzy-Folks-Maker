using System;
using Unity.XR.CoreUtils;
using UnityEngine;

public class VrGameState : MonoBehaviour
{
    public static VrGameState Instance;
    
    [SerializeField] public XROrigin xrOrigin;
    [NonSerialized] public string SelectedLevelName;
    [NonSerialized] public bool IsNewLevel;
    
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

        if (!xrOrigin)
        {
            xrOrigin = GameObject.Find("VR Setup").GetComponent<XROrigin>();
        }
    }
}