using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
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

        activated.AddListener(ToggleMenu);

        rightJoystickAction.Enable();
    }

    protected override void OnDisable()
    {
        rightJoystickAction.Disable();

        base.OnDisable();
    }

    private void ToggleMenu(ActivateEventArgs arg0)
    {
        ToggleEditMenu();
    }

    public void ToggleEditMenu()
    {
        if (EditObjectMenu.Instance == null)
            return;

        if (EditObjectMenu.Instance.selectedObject == this)
        {
            HideEditMenu();
        }
        else
        {
            ShowEditMenu();
        }
    }
    
    private void ShowEditMenu()
    {
        EditObjectMenu.Instance.gameObject.SetActive(false);
        EditObjectMenu.Instance.selectedObject = this;
        EditObjectMenu.Instance.gameObject.SetActive(true);
    }

    

    private void HideEditMenu()
    {
        EditObjectMenu.Instance.selectedObject = null;
        EditObjectMenu.Instance.gameObject.SetActive(false);
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