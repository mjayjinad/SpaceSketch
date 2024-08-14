using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ClockUIInteractionManager : MonoBehaviour
{
    [Header("UI Elements")]
    public RectTransform secondsHand; // Assign the RectTransform of the seconds hand
    public GameObject clockUI;

    [Header("XR Controller")]
    public XRBaseController controller;

    [Header("Input Actions")]
    public InputActionReference thumbstickAction; // Action to read thumbstick input
    public InputActionReference thumbstickTouchAction; // Action to read thumbstick input

    [Header("Command Manager")]
    private CommandManager commandManager;


    private float lastActionTime = 0f;
    private const float ActionCooldown = 0.1f; // 1 second cooldown between actions
    private float previousAngle = 0f;
    private float totalRotation = 0f;
    private bool isInDeadZone = false;
    private const float DeadZone = 10f;

    private void Awake()
    {
        commandManager = CommandManager.Instance;
    }

    private void OnEnable()
    {
        thumbstickTouchAction.action.performed += HandleActivateUI;
        thumbstickTouchAction.action.canceled += HandleDeactivateUI;

        thumbstickAction.action.Enable();
        thumbstickTouchAction.action.Enable();
    }

    private void OnDisable()
    {
        thumbstickTouchAction.action.performed -= HandleActivateUI;
        thumbstickTouchAction.action.canceled -= HandleDeactivateUI;

        thumbstickAction.action.Disable();
        thumbstickTouchAction.action.Disable();
    }

    private void Update()
    {
        if(!clockUI.activeInHierarchy)
            secondsHand.localRotation = Quaternion.Euler(0, 0, 0);

        Vector2 thumbstickInput = thumbstickAction.action.ReadValue<Vector2>();
        UpdateClockHandRotation(thumbstickInput);
        CheckAndTriggerActions();
    }

    private void HandleActivateUI(InputAction.CallbackContext context)
    {
        clockUI.gameObject.SetActive(true);
    }
    
    private void HandleDeactivateUI(InputAction.CallbackContext context)
    {
        secondsHand.localRotation = Quaternion.Euler(0, 0, 0);
        clockUI.gameObject.SetActive(false);
    }

    private void UpdateClockHandRotation(Vector2 input)
    {
        // Calculate the rotation change from the thumbstick input
        float rotationChange = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;

        // Only update the rotation if there is significant input
        if (input.magnitude > 0.1f)
        {
            totalRotation += rotationChange * Time.deltaTime * 2; // Adjust the multiplier for sensitivity
        }

        // Normalize and apply the rotation
        totalRotation = NormalizeAngle(totalRotation);
        secondsHand.localEulerAngles = new Vector3(0, 0, -totalRotation);
    }

    private void CheckAndTriggerActions()
    {
        if (!clockUI.activeInHierarchy)
            return;

        float currentAngle = secondsHand.rotation.eulerAngles.z;
        currentAngle = NormalizeAngle(currentAngle);

        // Determine rotation direction
        bool isClockwise = currentAngle >= previousAngle;
        if (currentAngle < previousAngle && (previousAngle - currentAngle) > 180)
        {
            // Handle wrap-around from 360 to 0 degrees
            isClockwise = true;
        }
        else if (currentAngle > previousAngle && (currentAngle - previousAngle) > 180)
        {
            // Handle wrap-around from 0 to 360 degrees
            isClockwise = false;
        }

        if (Time.time - lastActionTime > ActionCooldown)
        {
            if (!isInDeadZone)
            {
                bool triggeredAction = false;

                // Trigger redo action at 0 degrees when rotating clockwise
                if (isClockwise && IsWithinThreshold(currentAngle, 10))
                {
                    commandManager.RedoAction();
                    triggeredAction = true;
                    SendHapticFeedback();
                }
                // Trigger undo action at 0 degrees when rotating anticlockwise
                else if (!isClockwise && IsWithinThreshold(currentAngle, 10))
                {
                    commandManager.UndoAction();
                    triggeredAction = true;
                    SendHapticFeedback();
                }

                // Trigger undo action at 90 degrees when rotating anticlockwise
                else if (!isClockwise && IsWithinThreshold(currentAngle, 90))
                {
                    commandManager.UndoAction();
                    triggeredAction = true;
                    SendHapticFeedback();
                }

                // Trigger redo action at 180 degrees when rotating anticlockwise
                if (isClockwise && IsWithinThreshold(currentAngle, 180))
                {
                    commandManager.RedoAction();
                    triggeredAction = true;
                    SendHapticFeedback();
                }
                // Trigger undo action at 180 degrees when rotating anticlockwise
                else if (!isClockwise && IsWithinThreshold(currentAngle, 180))
                {
                    commandManager.UndoAction();
                    triggeredAction = true;
                    SendHapticFeedback();
                }

                // Trigger redo action at 270 degrees when rotating anticlockwise
                if (isClockwise && IsWithinThreshold(currentAngle, 270))
                {
                    commandManager.RedoAction();
                    triggeredAction = true;
                    SendHapticFeedback();
                }
                // Trigger undo action at 270 degrees when rotating anticlockwise
                else if (!isClockwise && IsWithinThreshold(currentAngle, 270))
                {
                    commandManager.UndoAction();
                    triggeredAction = true;
                    SendHapticFeedback();
                }

                if (triggeredAction)
                {
                    isInDeadZone = true;
                    lastActionTime = Time.time;
                }
            }
            else
            {
                // Check if the hand has moved out of the dead zone
                if (!IsWithinThreshold(currentAngle, 10, DeadZone) &&
                    !IsWithinThreshold(currentAngle, 90, DeadZone) &&
                    !IsWithinThreshold(currentAngle, 180, DeadZone) &&
                    !IsWithinThreshold(currentAngle, 270, DeadZone))
                {
                    isInDeadZone = false;
                }
            }
        }

        previousAngle = currentAngle; // Update the previous angle
    }

    private void SendHapticFeedback()
    {
        if (controller != null)
        {
            controller.SendHapticImpulse(0.3f, 0.2f); // Adjust intensity (0.5f) and duration (0.2f) as needed
        }
    }

    private bool IsWithinThreshold(float currentAngle, float targetAngle, float threshold = 5f)
    {
        return Mathf.Abs(currentAngle - targetAngle) <= threshold;
    }

    private float NormalizeAngle(float angle)
    {
        angle %= 360;
        if (angle < 0)
        {
            angle += 360;
        }
        return angle;
    }
}
