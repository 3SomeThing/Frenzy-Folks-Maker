using UnityEngine;

public class TrampolineTrapConfiguration : LevelElementConfiguration<TrampolineTrap, TrampolineTrapConfiguration>
{
    [SerializeField, Range(0f, 100f)] private float pushSpeedVertical = 30f;
    
    public override void Apply(TrampolineTrap trampoline)
    {
        trampoline.pushSpeedVertical = pushSpeedVertical;
    }

    public override TrampolineTrapConfiguration From(TrampolineTrap source)
    {
        pushSpeedVertical = source.pushSpeedVertical;
        return this;
    }
}
