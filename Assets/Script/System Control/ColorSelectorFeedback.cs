using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using xrc_assignments_project_g05.Scripts.Selection_and_Manipulation;

public class ColorSelectorFeedback : MonoBehaviour
{
    public bool isGrabbed = false;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    public bool isActive = false;

    XRGrabInteractable grabInteractable;
    private ColorSelectorInput colorSelectorInput;
    private IXRSelectInteractor currentInteractor; // Store the current interactor

    private void Awake()
    {
        FindObjectOfType<InputManager>().RegisterColorFeedbackObjects(this);
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    // Start is called before the first frame update
    void Start()
    {
        colorSelectorInput = FindObjectOfType<ColorSelectorInput>();
        initialRotation = Quaternion.identity;
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

    private void OnDestroy()
    {
        FindObjectOfType<InputManager>()?.UnregisterColorFeedbackObject(this);
    }

    public void AttemptToChangeColor()
    {
        // Check if the object is currently grabbed and an interactor is available
        if (!isGrabbed || currentInteractor == null) return;

        // Toggle the active state
        isActive = !isActive;

        // If active, proceed with color change process
        if (!isActive) return;

        // Force release the object if it is currently selected
        ForceReleaseIfSelected();

        // Activate the UI for color change
        ActivateUI(true);

        // Change the color of the mesh renderer
        ChangeMeshRendererColor();

        // Reset the object's position and rotation
        ResetObject();
    }

    private void ForceReleaseIfSelected()
    {
        // Check the tag of the grabbed object
        if (gameObject.CompareTag("SelectableObject"))
        {
            // Get the interactor that is currently grabbing this object
            XRBaseInteractor interactor = grabInteractable.selectingInteractor;

            if (interactor != null)
            {
                IXRSelectInteractor selectableInteractor = grabInteractable.selectingInteractor;
                // Force the release of the object
                interactor.interactionManager.SelectExit(selectableInteractor, grabInteractable);
            }
        }
        else
        {
            // If the current interactor has a selection, end the manual interaction
            if (currentInteractor.hasSelection)
            {
                var baseInteractor = currentInteractor as XRBaseInteractor;
                baseInteractor?.EndManualInteraction();
            }
        }
    }

    public void ActivateUI(bool activate)
    {
        if (activate)
        {
            colorSelectorInput.colorMenu.SetActive(true);
            colorSelectorInput.colorRay.SetActive(true);
            colorSelectorInput.colorInteractor.SetActive(true);
            colorSelectorInput.createObject.enabled = false;
        }
        else
        {
            colorSelectorInput.colorMenu.SetActive(false);
            colorSelectorInput.colorRay.SetActive(false);
            colorSelectorInput.colorInteractor.SetActive(false);
            colorSelectorInput.createObject.enabled = true;
        }
    }

    private void ResetObject()
    {
        transform.localPosition = initialPosition;
        transform.localRotation = initialRotation;
    }

    private void OnGrabbed(SelectEnterEventArgs arg)
    {
        currentInteractor = arg.interactorObject; // Store the current interactor
        initialPosition = transform.localPosition;
        isGrabbed = true;
        isActive = false;
    }

    private void OnReleased(SelectExitEventArgs arg)
    {
        isGrabbed = false;
        currentInteractor = null; // Clear the interactor reference
    }
   
    public void ChangeMeshRendererColor()
    {
        MeshRenderer[] meshRenderers;

        colorSelectorInput.currentRenderers.Clear();
        // Check if the object has any children
        if (gameObject.transform.childCount > 0)
        {
            // Get all MeshRenderers in the children
            meshRenderers = gameObject.GetComponentsInChildren<MeshRenderer>();
        }
        else
        {
            // Get the MeshRenderer of the object itself
            meshRenderers = gameObject.GetComponents<MeshRenderer>();
        }

        // Register all found MeshRenderers in the input current renderers
        foreach (var renderer in meshRenderers)
        {
            colorSelectorInput.RegisterMeshRenderer(renderer);
        }
    }
}
