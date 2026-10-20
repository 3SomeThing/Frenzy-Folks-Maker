using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class UiHapticFeedback : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private bool enableOnEnter = true;

    [SerializeField]
    private bool enableOnExit = true;
    
    [SerializeField, Range(0f, 1f)]
    private float hapticAmplitude = 0.5f;
    [SerializeField, Range(0f, 1f)]
    private float hapticDuration = 0.5f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!enableOnEnter)
            return;

        if (eventData is not TrackedDeviceEventData tracked ||
            tracked.interactor is not XRBaseInputInteractor interactor)
            return;
        
        if (!interactor) { return; }

        interactor.SendHapticImpulse(hapticAmplitude, hapticDuration);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!enableOnExit)
            return;

        if (eventData is not TrackedDeviceEventData tracked ||
            tracked.interactor is not XRBaseInputInteractor interactor)
            return;

        if (!interactor) { return; }

        interactor.SendHapticImpulse(hapticAmplitude, hapticDuration);

    }
}