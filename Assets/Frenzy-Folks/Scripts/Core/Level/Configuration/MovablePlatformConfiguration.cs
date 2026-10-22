using System;
using UnityEngine;

[Serializable]
public class MovablePlatformConfiguration : LevelElementConfiguration<MovablePlatform, MovablePlatformConfiguration>
{
    [SerializeField] public float speed = 5F;
    [SerializeField] public Vector3 secondPoint;

    public override void Apply(MovablePlatform target)
    {
        target.Speed = speed;
        target.SecondPoint = secondPoint;
    }

    public override MovablePlatformConfiguration From(MovablePlatform source)
    {
        speed = source.Speed;
        secondPoint = source.SecondPoint;
        return this;
    }
}