using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class DuplicateObject : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        FindObjectOfType<InputManager>().RegisterDuplicateObjects(this);
    }

    private void OnDestroy()
    {
        FindObjectOfType<InputManager>()?.UnregisterDuplicateObject(this);
    }

    public void AttemptToDuplicate()
    {
        if (grabInteractable.isSelected)
        {
            ICommand duplicateCommand = new DuplicateObjectCommand(gameObject);
            CommandManager.Instance.ExecuteCommand(duplicateCommand);
        }
    }

    public void CleanUpDuplicatedObject(GameObject duplicatedObject)
    {
        // Destroy children without MeshRenderer
        foreach (Transform child in duplicatedObject.transform)
        {
            if (child.gameObject.GetComponent<MeshRenderer>() == null)
            {
                Destroy(child.gameObject);
            }
        }

        // Additional handling for grouped objects
        if (duplicatedObject.transform.childCount > 1)
        {
            duplicatedObject.transform.DetachChildren();
            if (duplicatedObject.GetComponent<MeshRenderer>() == null)
            {
                Destroy(duplicatedObject);
            }
        }
    }

    public void ResetObjectScaler(GameObject duplicatedObject)
    {
        ObjectScaler scaler = duplicatedObject.GetComponent<ObjectScaler>();
        if (scaler != null)
        {
            scaler.isGrabbed = false;
        }
    }

    public void UpdateDuplicatedObjectColor(GameObject duplicatedObject)
    {
        if (duplicatedObject.transform.childCount > 1)
        {
            UpdateColorForGroupedObject(duplicatedObject);
        }
        else
        {
            UpdateColorForSingleObject(duplicatedObject);
        }
    }

    private void UpdateColorForSingleObject(GameObject duplicatedObject)
    {
        MeshRenderer originalMeshRenderer = gameObject.GetComponent<MeshRenderer>();
        MeshRenderer duplicatedMeshRenderer = duplicatedObject.GetComponent<MeshRenderer>();
        UpdateMeshRendererColor(originalMeshRenderer, duplicatedMeshRenderer);
    }

    private void UpdateColorForGroupedObject(GameObject duplicatedObject)
    {
        Transform[] originalChildren = gameObject.GetComponentsInChildren<Transform>();
        Transform[] duplicatedChildren = duplicatedObject.GetComponentsInChildren<Transform>();

        for (int i = 0; i < originalChildren.Length; i++)
        {
            MeshRenderer originalMeshRenderer = originalChildren[i].GetComponent<MeshRenderer>();
            MeshRenderer duplicatedMeshRenderer = duplicatedChildren[i].GetComponent<MeshRenderer>();
            UpdateMeshRendererColor(originalMeshRenderer, duplicatedMeshRenderer);
        }
    }

    private void UpdateMeshRendererColor(MeshRenderer original, MeshRenderer duplicatedMeshRenderer)
    {
        if (original != null && duplicatedMeshRenderer != null)
        {
            Material newMaterial = Instantiate(original.material);
            newMaterial.color = original.material.color;
            duplicatedMeshRenderer.material = newMaterial;
        }
    }
}