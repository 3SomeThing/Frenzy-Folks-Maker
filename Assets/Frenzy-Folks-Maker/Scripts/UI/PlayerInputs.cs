using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInputs : MonoBehaviour
{
    private bool _inputEnabled;
    
    [SerializeField]
    private GameObject objectToToggle;

    [Tooltip("Scene names where this input is active. Leave empty to allow every scene.")]
    [SerializeField]
    private List<string> allowedScenes = new List<string>();

    [SerializeField]
    private InputActionProperty leftPrimaryButton = new InputActionProperty(
        new InputAction(
            "Left Primary Button",
            InputActionType.Button,
            "<XRController>{LeftHand}/primaryButton"));


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        UpdateInputState();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        DisableInput();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateInputState();
    }

    private void UpdateInputState()
    {
        if (IsAllowedScene())
        {
            if (_inputEnabled)
                return;

            leftPrimaryButton.action.performed += OnLeftPrimaryButtonPerformed;
            leftPrimaryButton.action.Enable();
            _inputEnabled = true;
            return;
        }

        DisableInput();
    }

    private void DisableInput()
    {
        if (!_inputEnabled)
            return;

        leftPrimaryButton.action.performed -= OnLeftPrimaryButtonPerformed;
        leftPrimaryButton.action.Disable();
        _inputEnabled = false;
    }

    private bool IsAllowedScene()
    {
        if (allowedScenes.Count == 0)
            return true;

        string activeSceneName = SceneManager.GetActiveScene().name;
        return allowedScenes.Contains(activeSceneName);
    }

    private void OnLeftPrimaryButtonPerformed(InputAction.CallbackContext context)
    {
        if (objectToToggle == null)
        {
            Debug.LogWarning($"{nameof(PlayerInputs)} requires an object to toggle.", this);
            return;
        }

        objectToToggle.SetActive(!objectToToggle.activeSelf);
    }
}
