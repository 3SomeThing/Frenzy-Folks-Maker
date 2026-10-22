using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class MovablePlatformGhost : MonoBehaviour
{
    [SerializeField] private MovablePlatform platform;
    [SerializeField] private XRBaseInteractable platformInteractable;

    private XRGrabInteractable _grab;
    private MovableLevelElement _platformElement;
    private Renderer[] _renderers;
    private Collider[] _colliders;
    private bool _shown = true;
    private Vector3 _localOffset;
    private bool isInitialized;
    
    private void Awake()
    {
        _grab = GetComponent<XRGrabInteractable>();
        _renderers = GetComponentsInChildren<Renderer>(true);
        _colliders = GetComponentsInChildren<Collider>(true);

        if (platform == null) platform = GetComponentInParent<MovablePlatform>();
        if (platformInteractable == null)
            platformInteractable = platform.GetComponent<XRBaseInteractable>();
        _platformElement = platform.GetComponentInParent<MovableLevelElement>();

        bool wasEnabled = platformInteractable.enabled;
        platformInteractable.enabled = false; 
        foreach (var c in _colliders)
            platformInteractable.colliders.Remove(c);
        platformInteractable.enabled = wasEnabled;

        _localOffset = platform.transform.InverseTransformPoint(transform.position);

        var levelRoot = GameObject.Find("LevelEditor/Level");
        transform.SetParent(levelRoot ? levelRoot.transform : null, true);
    }

    private void OnEnable()
    {
        _grab.selectExited.AddListener(OnSelectExited);
        _grab.activated.AddListener(OnActivated);
    }

    private void OnDisable()
    {
        _grab.selectExited.RemoveListener(OnSelectExited);
        _grab.activated.RemoveListener(OnActivated);
    }

    private void Start() => SetShown(false);

    private void LateUpdate()
    {
        if (!platform)
        {
            Destroy(gameObject);
            return;
        }

        
        bool shouldShow = platformInteractable.isHovered || platformInteractable.isSelected || _grab.isHovered || _grab.isSelected;
        
        if (shouldShow && platform.isInitializedFromJson && !isInitialized)
        {
            isInitialized = true;
            transform.position = platform.SecondPoint;
            transform.localScale /= 20f;
        };
        
        if (shouldShow != _shown) SetShown(shouldShow);
    }

    private void SetShown(bool shown)
    {
        _shown = shown;
        foreach (var r in _renderers) r.enabled = shown;
        foreach (var c in _colliders) c.enabled = shown;
        _grab.enabled = shown;
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        platform.SetSecondPoint(transform.position);
    }

    private void OnActivated(ActivateEventArgs args)
    {
        _platformElement?.ToggleEditMenu();
    }
}