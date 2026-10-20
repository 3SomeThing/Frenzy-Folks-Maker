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

        var spawned = Instantiate(prefab, hitPoint, interactor.transform.rotation);
        interactor.interactionManager.SelectEnter(interactor, (IXRSelectInteractable)spawned);
    }
}
