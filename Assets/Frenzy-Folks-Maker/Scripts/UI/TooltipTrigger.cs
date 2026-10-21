using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TooltipUI tooltip;
    [SerializeField, TextArea] string message;
    [SerializeField] float delay = 0.3f;

    Coroutine _pending;

    public void OnPointerEnter(PointerEventData _) =>
        _pending = StartCoroutine(ShowAfterDelay());

    public void OnPointerExit(PointerEventData _) => Cancel();

    void OnDisable() => Cancel();

    void Cancel()
    {
        if (_pending != null) StopCoroutine(_pending);
        if (tooltip) tooltip.Hide();
    }

    IEnumerator ShowAfterDelay()
    {
        yield return new WaitForSecondsRealtime(delay);
        tooltip.Show(message, (RectTransform)transform);
    }
}