using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class SearchXRInteractionManager : MonoBehaviour
{
    private XRInteractionManager _xrInteractionManager;
    private TeleportationArea _teleportationArea;
    
    private void Awake()
    {
        _teleportationArea = GetComponent<TeleportationArea>();
        if(_teleportationArea == null) return;
        
        StartCoroutine(FindXRInteractionManager());
    }
    
    private IEnumerator FindXRInteractionManager()
    {
        while (_xrInteractionManager == null)
        {
            _xrInteractionManager = FindObjectOfType<XRInteractionManager>();
            yield return null;
        }
        
        _teleportationArea.interactionManager = _xrInteractionManager;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
