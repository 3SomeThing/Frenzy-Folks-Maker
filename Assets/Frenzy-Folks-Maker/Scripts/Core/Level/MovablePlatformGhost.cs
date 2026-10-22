// using UnityEngine;
// using UnityEngine.XR.Interaction.Toolkit;
// using UnityEngine.XR.Interaction.Toolkit.Interactables;
//
// [RequireComponent(typeof(XRGrabInteractable))]
// public class MovablePlatformGhost : MonoBehaviour
// {
//     [SerializeField] private MovablePlatform platform;
//     [SerializeField] private XRBaseInteractable platformInteractable;
//     [SerializeField] private GameObject visual;
//
//     private XRGrabInteractable _grab;
//     private bool _shown;
//
//     private void Awake()
//     {
//         _grab = GetComponent<XRGrabInteractable>();
//
//         if (platform == null) platform = GetComponentInParent<MovablePlatform>();
//         if (platformInteractable == null) platformInteractable = platform.GetComponent<XRBaseInteractable>();
//
//         transform.SetParent(GameObject.Find("LevelEditor/Level").transform, true);
//     }
//
//     private void OnEnable() => _grab.selectExited.AddListener(OnSelectExited);
//     private void OnDisable() => _grab.selectExited.RemoveListener(OnSelectExited);
//
//     private void Start() => SetShown(false);
//
//     private void LateUpdate()
//     {
//         if (!platform)
//         {
//             Destroy(gameObject);
//             return;
//         }
//
//         bool shouldShow = platformInteractable.isHovered;
//         if (shouldShow != _shown) SetShown(shouldShow);
//     }
//
//     private void SetShown(bool shown)
//     {
//         _shown = shown;
//         visual.SetActive(shown);
//     }
//
//     private void OnSelectExited(SelectExitEventArgs args)
//     {
//         platform.SetSecondPoint(transform.position);
//     }
// }

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class MovablePlatformGhost : MonoBehaviour
{
    [SerializeField] private MovablePlatform platform;
    [SerializeField] private XRBaseInteractable platformInteractable;

    private XRGrabInteractable _grab;
    private Renderer[] _renderers;
    private Collider[] _colliders;
    private bool _shown = true;
    private Vector3 _localOffset;

    private void Awake()
    {
        _grab = GetComponent<XRGrabInteractable>();
        _renderers = GetComponentsInChildren<Renderer>(true);
        _colliders = GetComponentsInChildren<Collider>(true);

        if (platform == null) platform = GetComponentInParent<MovablePlatform>();
        if (platformInteractable == null)
            platformInteractable = platform.GetComponent<XRBaseInteractable>();

        bool wasEnabled = platformInteractable.enabled;
        platformInteractable.enabled = false; 
        foreach (var c in _colliders)
            platformInteractable.colliders.Remove(c);
        platformInteractable.enabled = wasEnabled;

        _localOffset = platform.transform.InverseTransformPoint(transform.position);

        var levelRoot = GameObject.Find("LevelEditor/Level");
        transform.SetParent(levelRoot ? levelRoot.transform : null, true);
    }

    private void OnEnable() => _grab.selectExited.AddListener(OnSelectExited);
    private void OnDisable() => _grab.selectExited.RemoveListener(OnSelectExited);

    private void Start() => SetShown(false);

    private void LateUpdate()
    {
        if (!platform)
        {
            Destroy(gameObject);
            return;
        }

        bool shouldShow = platformInteractable.isHovered || platformInteractable.isSelected || _grab.isHovered || _grab.isSelected;
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
}