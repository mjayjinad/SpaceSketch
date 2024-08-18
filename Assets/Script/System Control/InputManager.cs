using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using SpaceSketch.Scripts.System_Control;

public class InputManager : MonoBehaviour
{
    [Header("Input Actions")]
    public InputActionReference deleteObjectAction;
    public InputActionReference duplicateObjectAction;
    public InputActionReference colorSelectorFeedbackAction;
    public InputActionReference ScaleAxisAction;
    public InputActionReference deformerAction;

    private List<Delete_Object> deleteObjectReference = new List<Delete_Object>();
    private List<DuplicateObject> duplicateObjectReference = new List<DuplicateObject>();
    private List<ColorSelectorFeedback> colorFeedbackReference = new List<ColorSelectorFeedback>();
    private List<ObjectScaler> ScaleAxisReference = new List<ObjectScaler>();
    private List<MeshDeformer> meshDeformerReference = new List<MeshDeformer>();

    private void OnEnable()
    {
        deleteObjectAction.action.performed += HandleDeleteAction;
        duplicateObjectAction.action.performed += HandleDuplicateAction;
        ScaleAxisAction.action.performed += HandleScaleAxisAction;
        colorSelectorFeedbackAction.action.started += HandleColorFeedbackAction;
        colorSelectorFeedbackAction.action.canceled += HandleColorFeedbackCancelAction;
        deformerAction.action.canceled += HandleDeformationAction;

        deleteObjectAction.action.Enable();
        duplicateObjectAction.action.Enable();
        colorSelectorFeedbackAction.action.Enable();
        ScaleAxisAction.action.Enable();
        deformerAction.action.Enable();
    }

    private void OnDisable()
    {
        deleteObjectAction.action.performed -= HandleDeleteAction;
        duplicateObjectAction.action.performed -= HandleDuplicateAction;
        ScaleAxisAction.action.performed -= HandleScaleAxisAction;
        colorSelectorFeedbackAction.action.started -= HandleColorFeedbackAction;
        colorSelectorFeedbackAction.action.canceled -= HandleColorFeedbackCancelAction;
        deformerAction.action.canceled -= HandleDeformationAction;

        deleteObjectAction.action.Disable();
        duplicateObjectAction.action.Disable();
        colorSelectorFeedbackAction.action.Disable();
        ScaleAxisAction.action.Disable();
        deformerAction.action.Disable();
    }

    private void HandleDeleteAction(InputAction.CallbackContext context)
    {
        // Call the delete method on all registered objects
        foreach (var deleteObject in deleteObjectReference)
        {
            deleteObject.AttemptToDelete();
        }
    }
    
    private void HandleDuplicateAction(InputAction.CallbackContext context)
    {
        foreach (var duplicatedObject in new List<DuplicateObject>(duplicateObjectReference))
        {
            duplicatedObject.AttemptToDuplicate();
        }
    }
     
    private void HandleScaleAxisAction(InputAction.CallbackContext context)
    {
        foreach (var objectScaler in ScaleAxisReference)
        {
            objectScaler.AttemptToActivateScale(context);
        }
    }
     
    private void HandleColorFeedbackAction(InputAction.CallbackContext context)
    {
        foreach (var objectToColor in colorFeedbackReference)
        {
            objectToColor.AttemptToChangeColor();
        }
    }
    
    private void HandleColorFeedbackCancelAction(InputAction.CallbackContext context)
    {
        foreach (var objectToColor in colorFeedbackReference)
        {
            objectToColor.ActivateUI(false);
        }
    }
    
    private void HandleDeformationAction(InputAction.CallbackContext context)
    {
        foreach (var objectToColor in meshDeformerReference)
        {
            objectToColor.AttemptToDeform();
        }
    }

    public void RegisterDeleteObject(Delete_Object obj)
    {
        if (!deleteObjectReference.Contains(obj))
        {
            deleteObjectReference.Add(obj);
        }
    }

    public void UnregisterDeleteObject(Delete_Object obj)
    {
        deleteObjectReference.Remove(obj);
    }

    public void RegisterDuplicateObjects(DuplicateObject obj)
    {
        if (!duplicateObjectReference.Contains(obj))
        {
            duplicateObjectReference.Add(obj);
        }
    }

    public void UnregisterDuplicateObject(DuplicateObject obj)
    {
        duplicateObjectReference.Remove(obj);
    }

    public void RegisterColorFeedbackObjects(ColorSelectorFeedback obj)
    {
        if (!colorFeedbackReference.Contains(obj))
        {
            colorFeedbackReference.Add(obj);
        }
    }
    
    public void UnregisterColorFeedbackObject(ColorSelectorFeedback obj)
    {
        colorFeedbackReference.Remove(obj);
    }

    public void RegisterScaleAxisObject(ObjectScaler obj)
    {
        if (!ScaleAxisReference.Contains(obj))
        {
            ScaleAxisReference.Add(obj);
        }
    }

    public void UnregisterScaleAxisObject(ObjectScaler obj)
    {
        ScaleAxisReference.Remove(obj);
    }

    public void RegisterMeshDeformationObjects(MeshDeformer obj)
    {
        if (!meshDeformerReference.Contains(obj))
        {
            meshDeformerReference.Add(obj);
        }
    }

    public void UnregisterMeshDeformationObject(MeshDeformer obj)
    {
        meshDeformerReference.Remove(obj);
    }
}
