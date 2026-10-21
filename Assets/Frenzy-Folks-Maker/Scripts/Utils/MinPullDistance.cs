using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class MinPullDistance : MonoBehaviour
{
    [SerializeField] float minDistance = 0.3f;
    private Transform _attach;

    void Start()
    {
        var interactor = GetComponent<XRBaseInteractor>();
        _attach = interactor.attachTransform;
    }

    void LateUpdate()
    {
        var p = _attach.localPosition;
        p.z = Mathf.Max(p.z, minDistance);
        _attach.localPosition = p;
    }
}
