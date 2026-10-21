using System;
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

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData is not TrackedDeviceEventData tracked ||
            tracked.interactor is not XRBaseInputInteractor interactor)
            return;

        Vector3 hitPoint = eventData.pointerCurrentRaycast.worldPosition;
        _spawnAudioSource.Play();
        
        if(prefab.gameObject.GetComponent<Building>() != null)
        {
            hitPoint += Vector3.forward * 50F;
            hitPoint.y = prefab.gameObject.transform.position.y / 20F;
        }

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
