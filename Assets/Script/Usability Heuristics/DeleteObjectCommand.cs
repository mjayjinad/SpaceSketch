using UnityEngine;
using System;

public class DeleteObjectCommand : ICommand
{
    //private Vector3 savedPosition;
    //private Quaternion savedRotation;
    //private Vector3 savedScale;
    //private Material savedMaterial;
    //private GameObject objectPrefab;
    //private string uniqueIdentifier;
    //private bool wasDeleted;

    //public DeleteObjectCommand(GameObject objectToDelete)
    //{
    //    // Store the prefab or the necessary components to recreate the object
    //    objectPrefab = objectToDelete;
    //    savedPosition = objectToDelete.transform.position;
    //    savedRotation = objectToDelete.transform.rotation;
    //    savedScale = objectToDelete.transform.localScale;
    //    savedMaterial = objectToDelete.GetComponent<MeshRenderer>()?.material;
    //    uniqueIdentifier = objectToDelete.name;
    //    wasDeleted = false;
    //}

    public void Execute()
    {
        //if (!wasDeleted)
        //{
        //    GameObject.Destroy(objectPrefab);
        //    wasDeleted = true;
        //}
        //else
        //{
        //    // Recreate the object
        //    GameObject recreatedObject = RecreateObject(objectPrefab, savedPosition, savedRotation, savedScale, savedMaterial, uniqueIdentifier);
        //    wasDeleted = false;
        //}
    }

    public void Undo()
    {
        //if (wasDeleted)
        //{
        //    // Recreate the object
        //    GameObject recreatedObject = RecreateObject(objectPrefab, savedPosition, savedRotation, savedScale, savedMaterial, uniqueIdentifier);
        //    wasDeleted = false;
        //}
        //else
        //{
        //    GameObject.Destroy(objectPrefab);
        //    wasDeleted = true;
        //}
    }

    //private static GameObject RecreateObject(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 scale, Material material, string uniqueId)
    //{
    //    GameObject newObj = GameObject.Instantiate(prefab, position, rotation);
    //    newObj.transform.localScale = scale;
    //    if (newObj.GetComponent<MeshRenderer>() != null)
    //    {
    //        newObj.GetComponent<MeshRenderer>().material = material;
    //    }
    //    newObj.name = uniqueId;
    //    return newObj;
    //}
}