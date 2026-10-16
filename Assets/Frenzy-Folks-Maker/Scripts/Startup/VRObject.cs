using UnityEngine;

public class VRObject : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        for (int i = 0; i < transform.childCount; i++)
        {
            DontDestroyOnLoad(transform.GetChild(i).gameObject);
        }
    }
}