using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLoaderVR : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
