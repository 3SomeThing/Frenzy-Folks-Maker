using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Button startGameButton;
    [SerializeField] private UnityEngine.UI.Button quitGameButton;

    private void Start()
    {
        // startGameButton.onClick.AddListener(StartGame);
        // quitGameButton.onClick.AddListener(QuitGame);
    }

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelEditor");
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}