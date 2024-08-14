using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace xrc_assignments_project_g05.Scripts.Selection_and_Manipulation
{
    public class SphereSelectInput : MonoBehaviour
    {
        [SerializeField]
        private XRBaseInteractor interactor;

        [Header("Input Actions")]
        public InputActionReference thumbstickMovementAction;
        public InputActionReference gripButtonAction;

        [Header("Selection Sphere")]
        public SelectionSphereFeedback selectionSphereFeedback;

        [Header("Controller Reference")]
        public Transform rightController;

        public SphereSelect sphereSelect;

        private void Awake()
        {
            if (rightController == null)
            {
                rightController = GameObject.FindGameObjectWithTag("RightController").transform;
            }
        }

        private void OnEnable()
        {
            thumbstickMovementAction.action.performed += HandleThumbstickMovement;
            gripButtonAction.action.started += DeavtivateSelectionSphere;
            gripButtonAction.action.canceled += ActivateSelectionSphere;

            thumbstickMovementAction.action.Enable();
            gripButtonAction.action.Enable();
        }

        private void OnDisable()
        {
            thumbstickMovementAction.action.performed -= HandleThumbstickMovement;
            gripButtonAction.action.started -= DeavtivateSelectionSphere;
            gripButtonAction.action.canceled -= ActivateSelectionSphere;

            thumbstickMovementAction.action.Disable();
            gripButtonAction.action.Disable();
        }
        private void HandleThumbstickMovement(InputAction.CallbackContext context)
        {
            Vector2 thumbstickPosition = context.ReadValue<Vector2>();
    
            sphereSelect.AdjustSelectionSphereRadius(thumbstickPosition.y);

            if (selectionSphereFeedback != null)
            {
                selectionSphereFeedback.AdjustSphereRadius(thumbstickPosition.y);
            }
        }

        private void DeavtivateSelectionSphere(InputAction.CallbackContext context)
        {
            selectionSphereFeedback.IsTouchingSelectableObject();
            sphereSelect.SelectObjects();
            StartCoroutine(ScaleDownCoroutine(0.05f, 0.2f, () => {selectionSphereFeedback.gameObject.GetComponent<MeshRenderer>().enabled = false;}));
        }

        private void ActivateSelectionSphere(InputAction.CallbackContext context)
        {
            if (interactor.hasSelection)
            {
                interactor.EndManualInteraction();
            }

            StartCoroutine(ScaleDownCoroutine(0.05f, 0.3f, () => {selectionSphereFeedback.gameObject.GetComponent<MeshRenderer>().enabled = true;}));

            // Reset the scale back to the minimum scale
            selectionSphereFeedback.transform.localScale = selectionSphereFeedback.minScale;
            sphereSelect.ReleaseGroup();
        }

        private IEnumerator ScaleDownCoroutine(float targetScale, float duration, Action onFinish)
        {
            Vector3 originalScale = selectionSphereFeedback.transform.localScale;
            Vector3 targetScaleVector = new Vector3(targetScale, targetScale, targetScale);
            float time = 0;

            while (time < duration)
            {
                selectionSphereFeedback.transform.localScale = Vector3.Lerp(originalScale, targetScaleVector, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            selectionSphereFeedback.transform.localScale = targetScaleVector;

            onFinish?.Invoke();
        }
    }
}
