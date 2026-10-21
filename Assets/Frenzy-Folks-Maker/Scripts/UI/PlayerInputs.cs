using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInputs : MonoBehaviour
{
    [Serializable]
    private class MenuInput
    {
        [Tooltip("Input button that toggles the menu.")]
        public InputActionProperty button;

        [Tooltip("Menu to toggle when the button is pressed.")]
        public GameObject menu;

        [NonSerialized]
        public Action<InputAction.CallbackContext> callback;
    }

    private bool _inputEnabled;

    [Tooltip("Additional button-to-menu bindings.")]
    [SerializeField]
    private List<MenuInput> additionalMenuInputs = new List<MenuInput>();

    [Tooltip("Scene names where this input is active. Leave empty to allow every scene.")]
    [SerializeField]
    private List<string> allowedScenes = new List<string>();

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

            foreach (MenuInput menuInput in additionalMenuInputs)
            {
                if (menuInput == null || menuInput.button.action == null)
                    continue;

                menuInput.callback = context => OnAdditionalMenuInputPerformed(menuInput, context);
                menuInput.button.action.performed += menuInput.callback;
                menuInput.button.action.Enable();
            }

            _inputEnabled = true;
            return;
        }

        DisableInput();
    }

    private void DisableInput()
    {
        if (!_inputEnabled)
            return;

        foreach (MenuInput menuInput in additionalMenuInputs)
        {
            if (menuInput == null || menuInput.button.action == null)
                continue;

            if (menuInput.callback != null)
                menuInput.button.action.performed -= menuInput.callback;

            menuInput.button.action.Disable();
            menuInput.callback = null;
        }

        _inputEnabled = false;
    }

    private bool IsAllowedScene()
    {
        if (allowedScenes.Count == 0)
            return true;

        string activeSceneName = SceneManager.GetActiveScene().name;
        return allowedScenes.Contains(activeSceneName);
    }

    private void ToggleMenu(GameObject menu)
    {
        if (menu == null)
        {
            Debug.LogWarning($"{nameof(PlayerInputs)} requires an object to toggle.", this);
            return;
        }

        bool shouldOpen = !menu.activeSelf;

        if (shouldOpen)
        {
            foreach (MenuInput menuInput in additionalMenuInputs)
            {
                if (menuInput != null && menuInput.menu != null && menuInput.menu != menu)
                    menuInput.menu.SetActive(false);
            }
        }

        menu.SetActive(shouldOpen);
    }

    private void OnAdditionalMenuInputPerformed(MenuInput menuInput, InputAction.CallbackContext context)
    {
        ToggleMenu(menuInput.menu);
    }
}
