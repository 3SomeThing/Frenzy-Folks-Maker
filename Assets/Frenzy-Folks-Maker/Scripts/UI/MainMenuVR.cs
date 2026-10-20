using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuVR : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Button startGameButton;
    [SerializeField] private UnityEngine.UI.Button quitGameButton;
    
    [SerializeField] private XROrigin xrOrigin;

    public void StartGame()
    {
        SceneManager.LoadScene("LevelEditor");
        xrOrigin.transform.position = new Vector3(4.3F, 4.7F, -10.5F);
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}