using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Platform : MonoBehaviour
{
    [SerializeField] public bool makePlatformFall = false;
    [SerializeField] public float delayBeforeFalling = 2f;
    [SerializeField] public float delayBeforeRespawning = 3f;

    [SerializeField] private AudioSource _crumbleSound;
    [SerializeField] private ParticleSystem _crumbleParticles;
}