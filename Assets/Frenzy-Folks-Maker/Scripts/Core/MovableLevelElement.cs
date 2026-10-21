using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

[RequireComponent(typeof(XRGeneralGrabTransformer))]
public class MovableLevelElement : XRGrabInteractable
{
    protected bool isBuilding;

    [SerializeField]
    private InputAction rightJoystickAction = new(
        "Right Joystick",
        InputActionType.Value,
        "<XRController>{RightHand}/primary2DAxis");

    protected override void OnEnable()
    {
        base.OnEnable();

        rightJoystickAction.Enable();
    }

    protected override void OnDisable()
    {
        rightJoystickAction.Disable();

        base.OnDisable();
    }

    private void Update()
    {
        if (!isSelected)
            return;

        Vector2 rightJoystick = rightJoystickAction.ReadValue<Vector2>();

        // Both joysticks can rotate the selected object.
        float horizontalInput = Mathf.Clamp(
            rightJoystick.x,
            -1f,
            1f);

        const float rotationSpeed = 100f;
        transform.Rotate(
            !isBuilding ? Vector3.up : Vector3.forward,
            horizontalInput * rotationSpeed * Time.deltaTime);
    }
}