using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class ObjectButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] XRGrabInteractable prefab;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData is not TrackedDeviceEventData tracked ||
            tracked.interactor is not XRBaseInputInteractor interactor)
            return;

        Vector3 hitPoint = eventData.pointerCurrentRaycast.worldPosition;

        XRGrabInteractable spawned = Instantiate(prefab, hitPoint, Quaternion.identity, GetParentLevel().transform);
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
