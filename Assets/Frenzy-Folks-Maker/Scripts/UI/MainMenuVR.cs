using UnityEngine;

public class MainMenuVR : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Button startGameButton;
    [SerializeField] private UnityEngine.UI.Button quitGameButton;

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelEditor");
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}