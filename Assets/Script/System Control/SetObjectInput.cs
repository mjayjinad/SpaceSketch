using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;
using xrc_assignments_project_g05.Scripts.Selection_and_Manipulation;

namespace xrc_assignments_project_g05.Scripts.System_Control
{
    public class ShapeInput : MonoBehaviour
    {
        public InputActionReference menuAction;
        public InputActionReference cycleShapeObject;

        public SphereSelectInput sphereSelectInput;
        public ClockUIInteractionManager clockUIInteractionManager;
        private ShapeLogic shapeLogic;
        public GameObject sphereInteractor, menuCanvas;
        private bool isActive = false;

        private enum ThumbstickDirection { None, Forward, Backward }
        private ThumbstickDirection lastThumbstickDirection = ThumbstickDirection.None;

        private void Start()
        {
            shapeLogic = GetComponent<ShapeLogic>();
        }

        private void OnEnable()
        {
            menuAction.action.performed += ActivateInteractor;
            cycleShapeObject.action.performed += SelectShape;

            menuAction.action.Enable();
            cycleShapeObject.action.Enable();
        }

        private void OnDisable()
        {
            menuAction.action.performed -= ActivateInteractor;
            cycleShapeObject.action.performed -= SelectShape;

            menuAction.action.Disable();
            cycleShapeObject.action.Disable();
        }

        private void ActivateInteractor(InputAction.CallbackContext context)
        {
            isActive = !isActive;
            if (isActive)
            {
                sphereSelectInput.enabled = false;
                clockUIInteractionManager.enabled = false;
                sphereInteractor.SetActive(false);
                menuCanvas.SetActive(true);
            }
            else
            {
                sphereSelectInput.enabled = true;
                clockUIInteractionManager.enabled = true;
                sphereInteractor.SetActive(true);
                menuCanvas.SetActive(false);
            }
        }

        private void SelectShape(InputAction.CallbackContext context)
        {
            if (menuCanvas.activeInHierarchy)
            {
                Vector2 thumbstickValue = cycleShapeObject.action.ReadValue<Vector2>();
                ThumbstickDirection currentDirection = GetThumbstickDirection(thumbstickValue);

                if (currentDirection != lastThumbstickDirection)
                {
                    if (currentDirection != ThumbstickDirection.None)
                    {
                        bool moveUpward = currentDirection == ThumbstickDirection.Forward;
                        shapeLogic.CycleShape(moveUpward);
                    }
                    lastThumbstickDirection = currentDirection;
                }
            }
        }

        private ThumbstickDirection GetThumbstickDirection(Vector2 thumbstickValue)
        {
            if (thumbstickValue.y > 0.5) // Threshold for forward movement
                return ThumbstickDirection.Forward;
            else if (thumbstickValue.y < -0.5) // Threshold for backward movement
                return ThumbstickDirection.Backward;

            return ThumbstickDirection.None;
        }
    }
}