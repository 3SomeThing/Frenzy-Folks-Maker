using Unity.Netcode;

public class VrGameState : NetworkBehaviour
{
    public static VrGameState Instance;

    public static string SelectedLevelName;
    public static bool IsNewLevel;
    public static bool HasSelection => IsNewLevel || !string.IsNullOrEmpty(SelectedLevelName);

    public static void Reset()
    {
        SelectedLevelName = null;
        IsNewLevel = false;
    }
    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}