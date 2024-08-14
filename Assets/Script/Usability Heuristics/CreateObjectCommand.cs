using System;
using UnityEngine;

public class CreateObjectCommand : ICommand
{
    private GameObject objectPrefab;
    private Vector3 savedPosition;
    private Quaternion savedRotation;
    private Vector3 savedScale;
    private Material savedMaterial;
    private GameObject createdObject;
    private Transform parentTransform;
    private bool hasSavedState = false;
    private string uniqueIdentifier;

    public CreateObjectCommand(GameObject prefab, Transform midPointFollower)
    {
        objectPrefab = prefab;
        parentTransform = midPointFollower;
    }

    public GameObject CreatedObject
    {
        get { return createdObject; }
    }

    public static GameObject RecreateObject(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 scale, Material material, string uniqueId)
    {
        GameObject newObj = GameObject.Instantiate(prefab, position, rotation);
        newObj.transform.localScale = scale;
        if (newObj.GetComponent<MeshRenderer>() != null)
        {
            newObj.GetComponent<MeshRenderer>().material = material;
        }
        newObj.name = uniqueId; // Assign the unique identifier
        return newObj;
    }

    public void Execute()
    {
        if (!hasSavedState)
        {
            // Initial creation
            createdObject = GameObject.Instantiate(objectPrefab, parentTransform.transform);
            uniqueIdentifier = createdObject.name + Guid.NewGuid().ToString(); // Assign and store unique identifier
            createdObject.name = uniqueIdentifier;
        }
        else
        {
            // Recreate with saved state
            createdObject = RecreateObject(objectPrefab, savedPosition, savedRotation, savedScale, savedMaterial, uniqueIdentifier);
        }
    }

    public void Undo()
    {
        // Save current state before destroying
        if(createdObject != null)
        {
            savedPosition = createdObject.transform.position;
            savedRotation = createdObject.transform.rotation;
            savedScale = createdObject.transform.localScale;
            savedMaterial = createdObject.GetComponent<MeshRenderer>()?.material;
            hasSavedState = true;

            // Destroy the object and keep the unique identifier
            GameObject.Destroy(createdObject);
        }
    }
}