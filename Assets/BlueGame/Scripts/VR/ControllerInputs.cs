using UnityEngine;
using UnityEngine.InputSystem;
using VRFPSKit;
using static UnityEngine.InputSystem.InputAction;

public class ControllerInputs : MonoBehaviour
{
    public bool triggerPressed;
    public bool primaryButtonPressed;
    public bool secondaryButtonPressed;
    public bool gripButtonPressed;
    public Vector2 joystickValue;
    public Vector3 joystickValueVector3;

    public InputActionReference triggerPressAction;
    public InputActionReference PrimaryButton;
    public InputActionReference SecondaryButton;
    public InputActionReference gripButton;
    public InputActionReference Joystick;

    public string controllerStr;

    private void Start()
    {
        triggerPressAction.action.performed += OnTriggerChanged;
        triggerPressAction.action.canceled += OnTriggerCanceled;

        //PrimaryButton.action.performed += OnPrimaryButtonChanged;
        //PrimaryButton.action.canceled += OnPrimaryCanceled;

        //SecondaryButton.action.performed += OnSecondaryButtonChanged;
        //SecondaryButton.action.canceled += OnSecondaryCanceled;

        gripButton.action.performed += OnGripButtonChanged;
        gripButton.action.canceled += OnGripCanceled;

        //Joystick.action.performed += OnJoystickValueChanged;
    }

    public void OnTriggerChanged(CallbackContext context)
    {
        triggerPressed = context.ReadValueAsButton();

        //ThirdMissionCarController.trigerCallback?.Invoke(controllerStr, true);
        Player.trigerCallback?.Invoke(controllerStr, true);

        //Debug.Log($"Trigger Pressed: {triggerPressed}");
    }
    public void OnTriggerCanceled(CallbackContext context)
    {
        triggerPressed = false;
        
        //ThirdMissionCarController.trigerCallback?.Invoke(controllerStr, false);
        Player.trigerCallback?.Invoke(controllerStr, false);
        //Debug.Log($"Trigger Released: {triggerPressed}");
    }

    public void OnPrimaryButtonChanged(CallbackContext context)
    {
        primaryButtonPressed = context.ReadValueAsButton();

        //Debug.Log($"Primary Button Pressed: {primaryButtonPressed}");
    }
    public void OnPrimaryCanceled(CallbackContext context)
    {
        primaryButtonPressed = false;

        //Debug.Log($"Primary Button Released: {primaryButtonPressed}");
    }

    public void OnSecondaryButtonChanged(CallbackContext context)
    {
        secondaryButtonPressed = context.ReadValueAsButton();

        //Debug.Log($"Secondary Button Pressed: {secondaryButtonPressed}");
    }
    public void OnSecondaryCanceled(CallbackContext context)
    {
        secondaryButtonPressed = false;

        //Debug.Log($"Secondary Button Released: {secondaryButtonPressed}");
    }

    public void OnGripButtonChanged(CallbackContext context)
    {
        gripButtonPressed = context.ReadValueAsButton();

        //Debug.Log($"Grip Button Pressed: {gripButtonPressed}");
    }
    public void OnGripCanceled(CallbackContext context)
    {
        gripButtonPressed = false;

        //Debug.Log($"Grip Button Released: {gripButtonPressed}");
    }

    public void OnJoystickValueChanged(CallbackContext context)
    {
        joystickValue = context.ReadValue<Vector2>();

        //Debug.Log($"Joystick Value: {joystickValue}");
    }
}