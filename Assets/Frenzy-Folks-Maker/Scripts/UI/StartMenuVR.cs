using System.Globalization;
using System.IO;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuVR : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] private Button quitGameButton;
    [SerializeField] private Button loadGameButton;
    [SerializeField] private XROrigin xrOrigin;

    [Header("Level list")]
    [SerializeField] private GameObject levelItemPrefab;
    [SerializeField] private Transform levelListContainer;
    [SerializeField] private GameObject newLevelItem;

    private Color normalColor = new Color(0.3735848f, 0.137451f, 0.3161876f);
    private Color newLevelColor = new Color(0.8566037f, 0.5600169f, 0.06303301f);
    private Color selectedColor = new Color(0.2f, 0.5f, 1f);

    private GameObject _currentSelectedLevel;

    public void Awake()
    {
        VrGameState.Reset();
        loadGameButton.interactable = false;

        loadGameButton.onClick.AddListener(StartGame);
        quitGameButton.onClick.AddListener(QuitGame);

        SetupNewLevelItem();
        BuildLevelList();
    }
    
    public void OnDestroy()
    {
        VrGameState.Reset();

        loadGameButton.onClick.RemoveListener(StartGame);
        quitGameButton.onClick.RemoveListener(QuitGame);
    }

    private void SetupNewLevelItem()
    {
        if (newLevelItem == null) return;

        SetLevelItemColor(newLevelItem, newLevelColor);
        newLevelItem.GetComponentInChildren<Button>().onClick.AddListener(SelectNewLevel);
    }

    private void BuildLevelList()
    {
        string levelsPath = FrenzyLevelConfiguration.LevelsPath;

        if (!Directory.Exists(levelsPath))
        {
            Directory.CreateDirectory(levelsPath);
            return;
        }

        string[] jsonFiles = Directory.GetFiles(levelsPath, "*.json", SearchOption.TopDirectoryOnly);

        foreach (string jsonFile in jsonFiles)
        {
            string rawName = Path.GetFileNameWithoutExtension(jsonFile);

            GameObject levelItem = Instantiate(levelItemPrefab, levelListContainer);
            levelItem.GetComponentInChildren<TextMeshProUGUI>().text = FormatName(rawName);
            levelItem.GetComponentInChildren<Button>().onClick.AddListener(() => SelectLevel(rawName, levelItem));
            SetLevelItemColor(levelItem, normalColor);
        }
    }

    private void SelectNewLevel()
    {
        HighlightItem(newLevelItem);

        VrGameState.SelectedLevelName = null;
        VrGameState.IsNewLevel = true;
        loadGameButton.interactable = true;
    }

    private void SelectLevel(string rawName, GameObject clickedLevelItem)
    {
        HighlightItem(clickedLevelItem);

        VrGameState.SelectedLevelName = rawName;
        VrGameState.IsNewLevel = false;
        loadGameButton.interactable = true;
    }

    private void HighlightItem(GameObject item)
    {
        if (_currentSelectedLevel != null)
            SetLevelItemColor(_currentSelectedLevel, GetBaseColor(_currentSelectedLevel));

        _currentSelectedLevel = item;
        SetLevelItemColor(_currentSelectedLevel, selectedColor);
    }

    private Color GetBaseColor(GameObject item)
    {
        return item == newLevelItem ? newLevelColor : normalColor;
    }

    private void SetLevelItemColor(GameObject levelItem, Color color)
    {
        levelItem.GetComponent<Image>().color = color;
    }

    private string FormatName(string name)
    {
        TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
        return textInfo.ToTitleCase(name.Replace('-', ' ').Replace('_', ' ').ToLowerInvariant());
    }

    public void StartGame()
    {
        if (!VrGameState.HasSelection) return;

        SceneManager.LoadScene("LevelEditor");
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}