using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LeftControllerVisibility : MonoBehaviour
{

    [Header("Input Action For Buttons")]
    public InputActionReference gripButton;
    public InputActionReference triggerButton;
    public InputActionReference primaryButton;
    public InputActionReference secondaryButton;
    public InputActionReference thumbstick;
    public InputActionReference optionsButton;

    private void OnEnable()
    {
        gripButton.action.performed += GripPressed;
        triggerButton.action.performed += TriggerPressed;
        primaryButton.action.performed += PrimaryButtonPressed;
        secondaryButton.action.performed += SecondaryButtonPressed;
        optionsButton.action.performed += OptionsButtonPressed;
        //thumbstick.action.performed += ThumbstickPressed;

        gripButton.action.canceled += GripReleased;
        triggerButton.action.canceled += TriggerReleased;
        primaryButton.action.canceled += PrimaryButtonReleased;
        secondaryButton.action.canceled += SecondaryButtonReleased;
        optionsButton.action.canceled += OptionsButtonReleased;
        //thumbstick.action.canceled += ThumbstickReleased;


        gripButton.action.Enable();
        triggerButton.action.Enable();
        primaryButton.action.Enable();
        secondaryButton.action.Enable();
        optionsButton.action.Enable();
        //thumbstick.action.Enable();
    }

    private void OnDisable()
    {
        gripButton.action.performed -= GripPressed;
        triggerButton.action.performed -= TriggerPressed;
        primaryButton.action.performed -= PrimaryButtonPressed;
        secondaryButton.action.performed -= SecondaryButtonPressed;
        optionsButton.action.performed -= OptionsButtonPressed;
        //thumbstick.action.performed -= ThumbstickPressed;

        gripButton.action.canceled -= GripReleased;
        triggerButton.action.canceled -= TriggerReleased;
        primaryButton.action.canceled -= PrimaryButtonReleased;
        secondaryButton.action.canceled -= SecondaryButtonReleased;
        optionsButton.action.canceled -= OptionsButtonReleased;
        //thumbstick.action.canceled -= ThumbstickReleased;

        gripButton.action.Disable();
        triggerButton.action.Disable();
        primaryButton.action.Disable();
        secondaryButton.action.Disable();
        optionsButton.action.Disable();
        //optionsButton.action.Disable();
    }

    private void OptionsButtonPressed(InputAction.CallbackContext obj)
    {
        GetComponent<Animator>().Play("Options Button Pressed", -1, 0f);
    }
    
    private void ThumbstickPressed(InputAction.CallbackContext obj)
    {

    }

    private void SecondaryButtonPressed(InputAction.CallbackContext obj)
    {
        GetComponent<Animator>().Play("Y Button Pressed", -1, 0f);
    }

    private void PrimaryButtonPressed(InputAction.CallbackContext obj)
    {
        GetComponent<Animator>().Play("X Button Pressed", -1, 0f);
    }

    private void TriggerPressed(InputAction.CallbackContext obj)
    {
        GetComponent<Animator>().Play("LTrigger Pressed", -1, 0f);
    }

    private void GripPressed(InputAction.CallbackContext obj)
    {
        GetComponent<Animator>().Play("LGrip Pressed", -1, 0f);
    }

    private void OptionsButtonReleased(InputAction.CallbackContext obj)
    {
        GetComponent<Animator>().Play("Options Button Released", -1, 0f);
    }

    private void ThumbstickReleased(InputAction.CallbackContext obj)
    {

    }

    private void SecondaryButtonReleased(InputAction.CallbackContext obj)
    {
        GetComponent<Animator>().Play("Y Button Released", -1, 0f);
    }

    private void PrimaryButtonReleased(InputAction.CallbackContext obj)
    {
        GetComponent<Animator>().Play("X Button Released", -1, 0f);
    }

    private void TriggerReleased(InputAction.CallbackContext obj)
    {
        GetComponent<Animator>().Play("LTrigger Released", -1, 0f);
    }

    private void GripReleased(InputAction.CallbackContext obj)
    {
        GetComponent<Animator>().Play("LGrip Released", -1, 0f);
    }
}
