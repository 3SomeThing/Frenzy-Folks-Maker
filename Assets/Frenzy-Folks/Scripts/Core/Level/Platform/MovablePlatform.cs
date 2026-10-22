using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovablePlatform : NetworkBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float pauseDuration = 1f;
    [SerializeField] private bool smoothMovement = true;
    [SerializeField, Min(0.01f)] private float smoothTime = 0.15f;

    public float Speed
    {
        get => speed;
        set => speed = value;
    }

    /// <summary>World-space velocity of the platform, valid on host AND clients.</summary>
    public Vector3 Velocity { get; private set; }
    
    /// <summary>Point d'arrivée (world space). L'assigner relance le trajet.</summary>
    public Vector3 SecondPoint
    {
        get => _secondPoint;
        set => _secondPoint = value;
    }

    private Vector3 _startPoint;
    private Vector3 _secondPoint;
    private bool _goingToSecond = true;
    private float _pauseTimer;

    private Rigidbody _body;
    private Vector3 _lastObservedPos;
    private bool _hasObserved;
    private Vector3 _smoothVelocity;

    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
        _body.isKinematic = true; // on every instance; NetworkTransform moves it on clients
    }
    

    public void SetSecondPoint(Vector3 worldPos)
    {
        SecondPoint = worldPos;
    }
}