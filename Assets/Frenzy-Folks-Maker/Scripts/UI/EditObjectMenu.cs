using System;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;

public class EditObjectMenu : MonoBehaviour
{
    public static EditObjectMenu Instance { get; private set; }
    
    [HideInInspector]
    public MovableLevelElement selectedObject;
    
    [SerializeField] private TextMeshProUGUI objectNameText;
    [SerializeField] private GameObject sliderPrefab;
    [SerializeField] private GameObject buttonPrefab; //TODO: Implement button prefab for non-numeric fields
    [SerializeField] private GameObject togglePrefab;
    [SerializeField] private GameObject inputFieldPrefab;

    [SerializeField] private GameObject contentWindow;
    [SerializeField] private GameObject confirmationWindow;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(this);
        }
    }

    private void OnEnable()
    {
        string objectName = selectedObject.GetComponent<SerializedLevelElement>().elementName;
        objectNameText.text = objectName.Replace('_', ' ').Trim();
        
        BaseConfigurationSerializable configuration = FrenzyLevelConfiguration.ConfigurationDefinitions[objectName].Invoke();
        configuration.From(selectedObject.gameObject);
        foreach (FieldInfo fieldInfo in configuration.GetType().GetRuntimeFields())
        {
            RangeAttribute range = fieldInfo.GetCustomAttribute<RangeAttribute>();
            
            if (range != null)
            {
                // If type is field is a int create a text input field for it
                if (fieldInfo.FieldType == typeof(int) )
                {
                    if (Mathf.Approximately(range.min, -1F))
                    {
                        GameObject inputFieldPrefabInstance = Instantiate(inputFieldPrefab, contentWindow.transform);
                        EditObjectInputField inputFieldObject = inputFieldPrefabInstance.GetComponent<EditObjectInputField>();
                        inputFieldObject.inputField.text = fieldInfo.GetValue(configuration).ToString();
                        inputFieldObject.inputField.characterValidation = TMP_InputField.CharacterValidation.Integer;
                        inputFieldObject.inputField.contentType = TMP_InputField.ContentType.IntegerNumber;
                        inputFieldObject.inputField.ForceLabelUpdate();
                        inputFieldObject.inputField.onValidateInput = (text, index, ch) =>
                            char.IsDigit(ch) ? ch : '\0';
                        inputFieldObject.labelText.text = fieldInfo.Name;
                        inputFieldObject.inputField.onValueChanged.AddListener((value) =>
                        {
                            if (string.IsNullOrEmpty(value)) value = "0";
                            
                            fieldInfo.SetValue(configuration, int.Parse(value));
                            configuration.Apply(selectedObject.gameObject);
                        });
                    }
                    else
                    {
                        GameObject sliderIntPrefabInstance = Instantiate(sliderPrefab, contentWindow.transform);
                        EditObjectSlider sliderIntObject = sliderIntPrefabInstance.GetComponent<EditObjectSlider>();
                        sliderIntObject.slider.minValue = range.min;
                        sliderIntObject.slider.maxValue = range.max;
                        sliderIntObject.slider.value = (int)fieldInfo.GetValue(configuration);
                        sliderIntObject.labelText.text = fieldInfo.Name;
                        sliderIntObject.slider.onValueChanged.AddListener((value) =>
                        {
                            fieldInfo.SetValue(configuration, (int)value);
                            configuration.Apply(selectedObject.gameObject);
                        });
                    }
                    continue;
                    
                }
                
                if (fieldInfo.FieldType == typeof(Boolean) && Mathf.Approximately(range.min, -1F))
                {
                    GameObject toggleButtonPrefabInstance = Instantiate(togglePrefab, contentWindow.transform);
                    EditObjectToggle toggleButtonObject = toggleButtonPrefabInstance.GetComponent<EditObjectToggle>();
                    toggleButtonObject.toggle.isOn = (bool)fieldInfo.GetValue(configuration);
                    toggleButtonObject.labelText.text = fieldInfo.Name;
                    toggleButtonObject.toggle.onValueChanged.AddListener((value) =>
                    {
                        fieldInfo.SetValue(configuration, value);
                        configuration.Apply(selectedObject.gameObject);
                    });
                    continue;
                }
                
                GameObject sliderPrefabInstance = Instantiate(sliderPrefab, contentWindow.transform);
                EditObjectSlider sliderObject = sliderPrefabInstance.GetComponent<EditObjectSlider>();
                sliderObject.slider.minValue = range.min;
                sliderObject.slider.maxValue = range.max;
                sliderObject.slider.value = (float)fieldInfo.GetValue(configuration);
                sliderObject.labelText.text = fieldInfo.Name;
                sliderObject.slider.onValueChanged.AddListener((value) =>
                {
                    fieldInfo.SetValue(configuration, value);
                    configuration.Apply(selectedObject.gameObject);
                });
            }
        }
    }

    private void OnDisable()
    {
        selectedObject = null;
        for (int i = contentWindow.transform.childCount - 1; i >= 0; i--)
        {
            if (contentWindow.transform.GetChild(i)
                    .GetComponent<EditObjectSlider>() ==
                null &&
                contentWindow.transform.GetChild(i)
                    .GetComponent<EditObjectToggle>() ==
                null &&
                contentWindow.transform.GetChild(i)
                    .GetComponent<EditObjectInputField>() ==
                null)
                continue;
            Destroy(contentWindow.transform.GetChild(i).gameObject);
        }
    }

    private void OnDestroy()
    {
        Instance = null;
    }
    
    public void ShowDestroyConfirmation()
    {
        contentWindow.SetActive(false);
        confirmationWindow.SetActive(true);
    }

    public void HideDestroyConfirmation()
    {
        confirmationWindow.SetActive(false);
        contentWindow.SetActive(true);
    }

    public void DestroyObject()
    {
        if (selectedObject == null) return;
        Destroy(selectedObject.gameObject);
        selectedObject = null;
        gameObject.SetActive(false);
    }

    public void ToggleLockObject()
    {
        if(selectedObject == null)
        {
            return;
        }
        
        LockableElement lockableElement = selectedObject.GetComponent<LockableElement>();
        
        if (lockableElement != null)
        {
            lockableElement.SetLocked(!lockableElement.IsLocked);
        }
    }
}
