using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabMoveInput : MonoBehaviour
{
    [Header("Input Actions:")]
    public InputAction leftGrabAction;
    public InputAction rightGrabAction;

    [Header("XRDirect Interactor:")]
    public XRDirectInteractor leftControllerXRDI;

    private bool leftGrabActivated;
    private bool rightGrabActivated;
    private GrabMove grabMove;

    private void Start()
    {
        grabMove = GetComponent<GrabMove>();

        // Initialize input actions
        leftGrabAction.Enable();
        rightGrabAction.Enable();

        leftControllerXRDI.selectEntered.AddListener(OnSelectEnter);
        leftControllerXRDI.selectExited.AddListener(OnSelectExit);
    }

    void OnSelectEnter(SelectEnterEventArgs args)
    {
        //grabMove.grabMoveActive = false;
    }

    void OnSelectExit(SelectExitEventArgs args)
    {
       // grabMove.grabMoveActive = true;
    }

    private void OnEnable()
    {
        // Subscribe to the action's performed and canceled callbacks
        leftGrabAction.started += OnLeftGrab;
        leftGrabAction.canceled += OnLeftGrabCanceled;

        rightGrabAction.started += OnRightGrab;
        rightGrabAction.canceled += OnRightGrabCanceled;
    }

    private void OnDisable()
    {
        // Unsubscribe from the action's callbacks
        leftGrabAction.started -= OnLeftGrab;
        leftGrabAction.canceled -= OnLeftGrabCanceled;

        rightGrabAction.started -= OnRightGrab;
        rightGrabAction.canceled -= OnRightGrabCanceled;

        // Disable the input actions
        leftGrabAction.Disable();
        rightGrabAction.Disable();
    }

    private void OnLeftGrab(InputAction.CallbackContext context)
    {
        leftGrabActivated = true;
        CheckForTwoHandedGrab();
    }

    private void OnLeftGrabCanceled(InputAction.CallbackContext context)
    {
        leftGrabActivated = false;
        CheckForTwoHandedGrab();
    }

    private void OnRightGrab(InputAction.CallbackContext context)
    {
        rightGrabActivated = true;
        CheckForTwoHandedGrab();
    }

    private void OnRightGrabCanceled(InputAction.CallbackContext context)
    {
        rightGrabActivated = false;
        CheckForTwoHandedGrab();
    }

    private void CheckForTwoHandedGrab()
    {
        if (leftGrabActivated && rightGrabActivated)
        {
            grabMove.OnGrabStart();
        }
        else
        {
            grabMove.OnGrabEnd();
        }
    }
}
