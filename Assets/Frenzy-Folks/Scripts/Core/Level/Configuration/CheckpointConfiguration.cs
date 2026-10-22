using UnityEngine;

public class CheckpointConfiguration : LevelElementConfiguration<Checkpoint, CheckpointConfiguration>
{
    [SerializeField, Range(-1, -1)] private int checkpointIndex;
    
    public override void Apply(Checkpoint target)
    {
        target.checkpointIndex = checkpointIndex;
    }

    public override CheckpointConfiguration From(Checkpoint source)
    {
        checkpointIndex = source.checkpointIndex;
        return this;
    }
}