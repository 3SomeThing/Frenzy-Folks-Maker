using UnityEngine;

public class BumperTrapConfiguration : LevelElementConfiguration<BumperTrap, BumperTrapConfiguration>
{
    [SerializeField, Range(0f, 100f)] private float pushSpeedHorizontal = 30f;
    [SerializeField, Range(0f, 50f)] private float pushSpeedVertical = 3F;
    
    public override void Apply(BumperTrap target)
    {
        target.pushSpeedHorizontal = pushSpeedHorizontal;
        target.pushSpeedVertical = pushSpeedVertical;
    }

    public override BumperTrapConfiguration From(BumperTrap source)
    {
        pushSpeedHorizontal = source.pushSpeedHorizontal;
        pushSpeedVertical = source.pushSpeedVertical;
        return this;
    }
}
