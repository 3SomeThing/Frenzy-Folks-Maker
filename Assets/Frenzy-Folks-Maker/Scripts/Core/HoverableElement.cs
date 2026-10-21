using ITISKIRUHERE;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
[RequireComponent(typeof(LockableElement))]
public class HoverableElement : MonoBehaviour
{
    private XRGrabInteractable _grabInteractable;
    private LockableElement _lockableElement;
    private AdvancedOutline _currentOutline;
    
    public void Awake()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _lockableElement = GetComponent<LockableElement>();
        
        _grabInteractable.hoverEntered.AddListener(OnHoverEntered);
        _grabInteractable.hoverExited.AddListener(OnHoverExited);
        _grabInteractable.selectEntered.AddListener(OnSelectEntered);
        _grabInteractable.selectExited.AddListener(OnSelectExited);
    }

    public void OnDestroy()
    {
        _grabInteractable.hoverEntered.RemoveListener(OnHoverEntered);
        _grabInteractable.hoverExited.RemoveListener(OnHoverExited);
        _grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
        _grabInteractable.selectExited.RemoveListener(OnSelectExited);
    }

    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        ToggleOutline(true);
    }
    
    private void OnHoverExited(HoverExitEventArgs args)
    {
        ToggleOutline(_grabInteractable.isSelected);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        ToggleOutline(true);
    }
    
    private void OnSelectExited(SelectExitEventArgs args)
    {
        ToggleOutline(_grabInteractable.isHovered);
    }
    
    public void ToggleOutline(bool toggleStatus)
    {
        if (_lockableElement.IsLocked) return;

        if (toggleStatus)
        {
            if (_currentOutline == null)
            {
                _currentOutline = GetComponent<AdvancedOutline>();
                if (_currentOutline == null)
                    _currentOutline = gameObject.AddComponent<AdvancedOutline>();
            }

            if (_currentOutline == null) return;
            _currentOutline.OutlineMode = AdvancedOutline.Mode.OutlineAll;
        }
        else
        {
            if (_currentOutline != null)
            {
                DestroyImmediate(_currentOutline);
                _currentOutline = null;
            }
        }
    }
}