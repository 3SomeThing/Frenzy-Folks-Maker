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
        Debug.Log("Configuration initialized for " + objectName);
        Debug.Log("Configuration values initialized for " + configuration.GetType().Name);
        foreach (FieldInfo fieldInfo in configuration.GetType().GetRuntimeFields())
        {
            RangeAttribute range = fieldInfo.GetCustomAttribute<RangeAttribute>();
            
            if (range != null)
            {
                // If type is field is a int create a text input field for it
                if (fieldInfo.FieldType == typeof(int) &&  Mathf.Approximately(range.min, -1F))
                {
                    // Do not create a slider for int fields, instead create a text input field (not implemented yet)
                    continue;
                }
                
                if (fieldInfo.GetType() == typeof(bool) && Mathf.Approximately(range.min, -1F))
                {
                    // Do not create a slider for boolean fields, instead create a toggle button (not implemented yet)
                    continue;
                }
                
                if(fieldInfo.GetType() == typeof(float) && Mathf.Approximately(range.min, -1F))
                {
                    // Do not create a slider for string fields, instead create a text input field (not implemented yet)
                    continue;
                }
                
                GameObject sliderPrefabInstance = Instantiate(sliderPrefab, contentWindow.transform);
                EditObjectSlider sliderObject = sliderPrefabInstance.GetComponent<EditObjectSlider>();
                sliderObject.slider.minValue = range.min;
                sliderObject.slider.maxValue = range.max;
                sliderObject.slider.value = (float)fieldInfo.GetValue(configuration);
                sliderObject.labelText.text = fieldInfo.Name;
            }
        }
    }
    
    private void 

    private void OnDisable()
    {
        selectedObject = null;
        for (int i = contentWindow.transform.childCount - 1; i >= 0; i--)
        {
            if(contentWindow.transform.GetChild(i).GetComponent<EditObjectSlider>() == null) continue;
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
