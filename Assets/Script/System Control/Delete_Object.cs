using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace SpaceSketch.Scripts.System_Control
{
    public class Delete_Object : MonoBehaviour
    {
        public bool isGrabbed;

        private XRGrabInteractable grabInteractable;
        private IXRSelectInteractor currentInteractor; // Store the current interactor

        private void Awake()
        {
            grabInteractable = gameObject.GetComponent<XRGrabInteractable>();
            FindObjectOfType<InputManager>().RegisterDeleteObject(this);
        }

        private void OnDestroy()
        {
            FindObjectOfType<InputManager>()?.UnregisterDeleteObject(this);
        }

        public void AttemptToDelete()
        {
            if (isGrabbed && currentInteractor != null)
            {
                // End manual interaction before destroying
                (currentInteractor as XRBaseInteractor)?.EndManualInteraction();
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
            grabInteractable.selectEntered.AddListener(OnGrabbed);
            grabInteractable.selectExited.AddListener(OnReleased);
        }

        private void OnDisable()
        {
            grabInteractable.selectEntered.RemoveListener(OnGrabbed);
            grabInteractable.selectExited.RemoveListener(OnReleased);
        }

        private void OnGrabbed(SelectEnterEventArgs args)
        {
            isGrabbed = true;
            currentInteractor = args.interactorObject; // Store the current interactor
        }

        private void OnReleased(SelectExitEventArgs args)
        {
            isGrabbed = false;
            currentInteractor = null; // Clear the interactor reference
        }
    }
}