using UnityEngine;

public class PlatformConfiguration : LevelElementConfiguration<Platform, PlatformConfiguration>
{
    [SerializeField, Range(-1, -1)] private bool makePlatformFall = false;
    [SerializeField, Range(0f, 10f)] private float delayBeforeFalling = 2f;
    [SerializeField, Range(0f, 10f)] private float delayBeforeRespawning = 3f;

    public override void Apply(Platform target)
    {
        target.makePlatformFall = makePlatformFall;
        target.delayBeforeFalling = delayBeforeFalling;
        target.delayBeforeRespawning = delayBeforeRespawning;
    }

    public override PlatformConfiguration From(Platform source)
    {
        makePlatformFall = source.makePlatformFall;
        delayBeforeFalling = source.delayBeforeFalling;
        delayBeforeRespawning = source.delayBeforeRespawning;
        return this;
    }
}