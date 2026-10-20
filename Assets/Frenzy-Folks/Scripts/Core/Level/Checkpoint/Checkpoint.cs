using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private static int nextCheckpointIndex;
    private static readonly Dictionary<int, Checkpoint> checkpoints =
        new Dictionary<int, Checkpoint>();

    [SerializeField] public int checkpointIndex;

    [SerializeField] private AudioSource checkpointSound;
    [SerializeField] private ParticleSystem checkpointParticules;
    [SerializeField] private SkinnedMeshRenderer cylinder;
    
    [SerializeField] public bool playSoundAndAnimation = true;
    [SerializeField] public bool changeColor = true;
    
    private MaterialPropertyBlock block;
    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
    
    private static readonly Color reachedColor = new(0.0f, 0.8f, 0.0f, 0.5f);
        
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetIndex()
    {
        nextCheckpointIndex = 1;
        checkpoints.Clear();
    }

    private void Awake()
    {        
        block = new MaterialPropertyBlock();
        
        if (checkpointIndex <= 0)
        {
            while (checkpoints.ContainsKey(nextCheckpointIndex))
            {
                nextCheckpointIndex++;
            }

            checkpointIndex = nextCheckpointIndex;
        }

        nextCheckpointIndex = Mathf.Max(nextCheckpointIndex, checkpointIndex + 1);
        checkpoints[checkpointIndex] = this;
    }

    private void OnDestroy()
    {
        if (checkpoints.TryGetValue(checkpointIndex, out Checkpoint checkpoint) &&
            checkpoint == this)
        {
            checkpoints.Remove(checkpointIndex);
        }
    }

    public static bool TryGetByIndex(int index, out Checkpoint checkpoint)
    {
        return checkpoints.TryGetValue(index, out checkpoint);
    }
}
