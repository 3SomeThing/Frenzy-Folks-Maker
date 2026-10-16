using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLoader : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
