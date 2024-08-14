using UnityEngine;

public class DeformMeshCommand : ICommand
{
    private string uniqueObjectName;
    private Mesh originalMesh;
    private Mesh deformedMesh;

    public DeformMeshCommand(GameObject objectToDeform, Mesh originalMesh, Mesh deformedMesh)
    {
        this.uniqueObjectName = objectToDeform.name; // Store the unique name of the object
        this.originalMesh = originalMesh;
        this.deformedMesh = deformedMesh;
    }

    private GameObject FindObjectByUniqueName()
    {
        // Find the GameObject by its unique name
        return GameObject.Find(uniqueObjectName);
    }

    public void Execute()
    {
        GameObject objectToDeform = FindObjectByUniqueName();
        if (objectToDeform != null)
        {
            SetMesh(objectToDeform, deformedMesh);
        }
    }

    public void Undo()
    {
        GameObject objectToDeform = FindObjectByUniqueName();
        if (objectToDeform != null)
        {
            SetMesh(objectToDeform, originalMesh);
        }
    }

    private void SetMesh(GameObject obj, Mesh newMesh)
    {
        MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            meshFilter.mesh = newMesh;
        }

        MeshCollider meshCollider = obj.GetComponent<MeshCollider>();
        if (meshCollider != null)
        {
            meshCollider.sharedMesh = newMesh;
        }

        MeshDeformer deformer = obj.GetComponent<MeshDeformer>();
        if (deformer != null)
        {
            deformer.mesh = newMesh;
            deformer.ResetGizmo();
        }
    }
}
