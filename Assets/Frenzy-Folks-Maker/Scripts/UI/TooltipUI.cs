using TMPro;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class TooltipUI : MonoBehaviour
{
    [SerializeField] TMP_Text label;
    [SerializeField] float gap = 20f; // canvas units below the button

    private CanvasGroup _group;
    private RectTransform _rect;

    void Awake()
    {
        _group = GetComponent<CanvasGroup>();
        _rect = (RectTransform)transform;
        _group.blocksRaycasts = false; // see gotcha below
        _group.alpha = 0f;
    }

    public void Show(string text, RectTransform target)
    {
        label.text = text;
        _rect.position = target.TransformPoint(
            new Vector3(target.rect.center.x, target.rect.yMin - gap, 0f));
        transform.SetAsLastSibling(); // draw on top of other menu elements
        _group.alpha = 1f;
    }

    public void Hide() => _group.alpha = 0f;
    
    public void HideAfterDelay(float delay)
    {
        Invoke(nameof(Hide), delay);
    }
}