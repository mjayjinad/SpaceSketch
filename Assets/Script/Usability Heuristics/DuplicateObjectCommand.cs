using UnityEngine;
using System;

public class DuplicateObjectCommand : ICommand
{
    private string originalObjectName;
    private GameObject duplicatedObject;

    // Variables to store the transform states of the duplicated object
    private Vector3 lastKnownDuplicatedPosition;
    private Quaternion lastKnownDuplicatedRotation;
    private Vector3 lastKnownDuplicatedScale;
    private string duplicatedObjectName;
    private bool undoExecuted;

    public DuplicateObjectCommand(GameObject original)
    {
        this.originalObjectName = original.name;
        undoExecuted = false;

        // Generate a unique identifier for the duplicated object
        duplicatedObjectName = originalObjectName + "_dup_" + Guid.NewGuid().ToString();
    }

    private GameObject FindOriginalObject()
    {
        return GameObject.Find(originalObjectName);
    }

    private DuplicateObject FindDuplicator()
    {
        GameObject originalObject = FindOriginalObject();
        if (originalObject != null)
        {
            return originalObject.GetComponent<DuplicateObject>();
        }
        return null;
    }

    public void Execute()
    {
        GameObject originalObject = FindOriginalObject();
        DuplicateObject duplicator = FindDuplicator();

        if (originalObject != null && originalObject.activeInHierarchy && duplicator != null)
        {
            if (!undoExecuted)
            {
                duplicatedObject = GameObject.Instantiate(originalObject, originalObject.transform.position, originalObject.transform.rotation);
                duplicatedObject.name = duplicatedObjectName;
            }
            else
            {
                duplicatedObject = GameObject.Instantiate(originalObject, lastKnownDuplicatedPosition, lastKnownDuplicatedRotation);
                duplicatedObject.transform.localScale = lastKnownDuplicatedScale;
                duplicatedObject.name = duplicatedObjectName;
            }

            if (duplicator != null)
            {
                duplicator.ResetObjectScaler(duplicatedObject);
                duplicator.UpdateDuplicatedObjectColor(duplicatedObject);
                duplicator.CleanUpDuplicatedObject(duplicatedObject);
            }
        }
        else
        {
            Debug.LogError("Cannot duplicate: Original object or duplicator is not valid or not found.");
        }
    }

    public void Undo()
    {
        if (duplicatedObject != null)
        {

            // Save the transform states
            lastKnownDuplicatedPosition = duplicatedObject.transform.position;
            lastKnownDuplicatedRotation = duplicatedObject.transform.rotation;
            lastKnownDuplicatedScale = duplicatedObject.transform.localScale;

            GameObject.Destroy(duplicatedObject);
            duplicatedObject = null;
            undoExecuted = true;
        }
    }
}