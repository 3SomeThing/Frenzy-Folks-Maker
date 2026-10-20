using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    [SerializeField]
    private GameObject objectToToggle;

    [SerializeField]
    private InputActionProperty leftPrimaryButton = new InputActionProperty(
        new InputAction(
            "Left Primary Button",
            InputActionType.Button,
            "<XRController>{LeftHand}/primaryButton"));

    private void OnEnable()
    {
        leftPrimaryButton.action.performed += OnLeftPrimaryButtonPerformed;
        leftPrimaryButton.action.Enable();
    }

    private void OnDisable()
    {
        leftPrimaryButton.action.performed -= OnLeftPrimaryButtonPerformed;
        leftPrimaryButton.action.Disable();
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
