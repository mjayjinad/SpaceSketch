using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RightControllerVisibility : MonoBehaviour
{
    [Header("Input Action For Buttons")]
    public InputActionReference gripButton;
    public InputActionReference triggerButton;
    public InputActionReference primaryButton;
    public InputActionReference secondaryButton;
    public InputActionReference thumbstick;

    private void OnEnable()
    {
        gripButton.action.performed += GripPressed;
        triggerButton.action.performed += TriggerPressed;
        primaryButton.action.performed += PrimaryButtonPressed;
        secondaryButton.action.performed += SecondaryButtonPressed;
        //thumbstick.action.performed += ThumbstickPressed;

        gripButton.action.canceled += GripReleased;
        triggerButton.action.canceled += TriggerReleased;
        primaryButton.action.canceled += PrimaryButtonReleased;
        secondaryButton.action.canceled += SecondaryButtonReleased;
        //thumbstick.action.canceled += ThumbstickReleased;


        gripButton.action.Enable();
        triggerButton.action.Enable();
        primaryButton.action.Enable();
        secondaryButton.action.Enable();
        //thumbstick.action.Enable();
    }

    private void OnDisable()
    {
        gripButton.action.performed -= GripPressed;
        triggerButton.action.performed -= TriggerPressed;
        primaryButton.action.performed -= PrimaryButtonPressed;
        secondaryButton.action.performed -= SecondaryButtonPressed;
        //thumbstick.action.performed -= ThumbstickPressed;

        gripButton.action.canceled -= GripReleased;
        triggerButton.action.canceled -= TriggerReleased;
        primaryButton.action.canceled -= PrimaryButtonReleased;
        secondaryButton.action.canceled -= SecondaryButtonReleased;
        //thumbstick.action.canceled -= ThumbstickReleased;

        gripButton.action.Disable();
        triggerButton.action.Disable();
        primaryButton.action.Disable();
        secondaryButton.action.Disable();
        //optionsButton.action.Disable();
    }

    private void ThumbstickPressed(InputAction.CallbackContext context)
    {
        Vector2 thumbstickValue = context.ReadValue<Vector2>();

        // You can adjust these thresholds based on your thumbstick sensitivity
        float horizontalThreshold = 0.5f;
        float verticalThreshold = 0.5f;

        if (thumbstickValue.x > horizontalThreshold)
        {
            // Play right direction thumbstick animation
            GetComponent<Animator>().Play("RightThumbstickAnimation");
        }
        else if (thumbstickValue.x < -horizontalThreshold)
        {
            // Play left direction thumbstick animation
            GetComponent<Animator>().Play("LeftThumbstickAnimation");
        }
        else if (thumbstickValue.y > verticalThreshold)
        {
            // Play up thumbstick animation
            GetComponent<Animator>().Play("UpThumbstickAnimation");
        }
        else if (thumbstickValue.y < -verticalThreshold)
        {
            // Play down thumbstick animation
            GetComponent<Animator>().Play("DownThumbstickAnimation");
        }
    }

    private void SecondaryButtonPressed(InputAction.CallbackContext obj)
    {
        GetComponent<Animator>().Play("B Button Pressed", -1, 0f);
    }

    private void PrimaryButtonPressed(InputAction.CallbackContext obj)
    {
        GetComponent<Animator>().Play("A Button Pressed", -1, 0f);
    }

    private void TriggerPressed(InputAction.CallbackContext obj)
    {
        GetComponent<Animator>().Play("Trigger Pressed", -1, 0f);
    }

    private void GripPressed(InputAction.CallbackContext obj)
    {
        GetComponent<Animator>().Play("Grip Pressed", -1, 0f);
    }

    private void ThumbstickReleased(InputAction.CallbackContext obj)
    {

    }

    private void SecondaryButtonReleased(InputAction.CallbackContext obj)
    {
        GetComponent<Animator>().Play("B Button Released", -1, 0f);
    }

    private void PrimaryButtonReleased(InputAction.CallbackContext obj)
    {
        GetComponent<Animator>().Play("A Button Released", -1, 0f);
    }

    private void TriggerReleased(InputAction.CallbackContext obj)
    {
        GetComponent<Animator>().Play("Trigger Released", -1, 0f);
    }

    private void GripReleased(InputAction.CallbackContext obj)
    {
        GetComponent<Animator>().Play("Grip Released", -1, 0f);
    }
}
