using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class ObjectButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] XRGrabInteractable prefab;
    private AudioSource _spawnAudioSource;


    private void Awake()
    {
        _spawnAudioSource = GetComponent<AudioSource>();
    }
    
    // Attach to any object; point at the canvas and watch the console
    void Update()
    {
        var es = UnityEngine.EventSystems.EventSystem.current;
        Debug.Log($"EventSystem: {es != null}, module: {es?.currentInputModule?.GetType().Name}");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData is not TrackedDeviceEventData tracked ||
            tracked.interactor is not XRBaseInputInteractor interactor)
            return;

        Vector3 hitPoint = eventData.pointerCurrentRaycast.worldPosition;
        _spawnAudioSource.Play();

        XRGrabInteractable spawned = Instantiate(prefab, hitPoint, prefab.gameObject.transform.rotation, GetParentLevel().transform);
        interactor.interactionManager.SelectEnter(interactor, (IXRSelectInteractable)spawned);
    }

    public static GameObject GetParentLevel()
    {
        GameObject parentLevel = GameObject.Find("Level");
        if (parentLevel == null)
        {
            throw new Exception("Parent level object not found. Please ensure there is a GameObject named 'Level' in the scene.");
        }
        return parentLevel;
    }
}
