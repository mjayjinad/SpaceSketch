using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class MeshDeformer : MonoBehaviour
{
    public GameObject vertexPrefab;
    public Mesh mesh;

    public GameObject[] handles;
    private Dictionary<Vector3, List<int>> vertexGroups; // Map corner position to vertex indices
    private Mesh originalMesh;
    private Mesh originalMeshColliderMesh;
    private Mesh originalMeshClone;
    private Mesh deformedMeshClone;
    private bool isGrabbed = false;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private XRGrabInteractable grabInteractable;

    public bool IsDeforming { get; private set; }

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        FindObjectOfType<InputManager>()?.RegisterMeshDeformationObjects(this);
    }

    void Start()
    {
        StartDeformation();

        initialRotation = Quaternion.identity;
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        originalMesh = meshFilter.mesh;
        mesh = Instantiate(originalMesh);
        meshFilter.mesh = mesh;

        MeshCollider meshCollider = GetComponent<MeshCollider>();
        if (meshCollider != null)
        {
            originalMeshColliderMesh = meshCollider.sharedMesh; // Store the original mesh collider mesh
        }
    }

    void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrabbed);
        grabInteractable.selectExited.RemoveListener(OnReleased);
    }

    public void AttemptToDeform()
    {
        if (isGrabbed)
        {
            // Get the interactor that is currently grabbing this object
            XRBaseInteractor interactor = grabInteractable.selectingInteractor;

            if (interactor != null)
            {
                IXRSelectInteractor selectableInteractor = grabInteractable.selectingInteractor;
                // Force the release of the object
                interactor.interactionManager.SelectExit(selectableInteractor, grabInteractable);
            }

            // Now the object is no longer grabbed, we can reset it
            ResetObject();

            ActivateGizmos(true);
        }
        else
        {
            ActivateGizmos(false);
            handles = new GameObject[0];
        }
    }

    private Dictionary<Vector3, List<int>> GroupVerticesByPosition(Vector3[] vertices)
    {
        var groups = new Dictionary<Vector3, List<int>>();
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 roundedPos = RoundVector(vertices[i], 3);
            if (!groups.ContainsKey(roundedPos))
            {
                groups[roundedPos] = new List<int>();
            }
            groups[roundedPos].Add(i);
        }
        return groups;
    }

    private Vector3 RoundVector(Vector3 vector, int decimalPlaces)
    {
        float multiplier = Mathf.Pow(10.0f, decimalPlaces);
        return new Vector3(
            Mathf.Round(vector.x * multiplier) / multiplier,
            Mathf.Round(vector.y * multiplier) / multiplier,
            Mathf.Round(vector.z * multiplier) / multiplier
        );
    }

    public void UpdateVertexGroupPosition(List<int> vertexIndices, Vector3 newPosition)
    {
        Vector3[] vertices = mesh.vertices;
        Vector3 localPosition = transform.InverseTransformPoint(newPosition);

        foreach (int index in vertexIndices)
        {
            vertices[index] = localPosition;
        }

        mesh.vertices = vertices; // Reassign the modified vertices array to the mesh
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }

    public void UpdateMeshCollider()
    {
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        if (meshCollider != null)
        {
            meshCollider.sharedMesh = null; // Clear the current mesh
            meshCollider.sharedMesh = mesh; // Assign the updated mesh
        }
    }

    void OnDestroy()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            meshFilter.mesh = originalMesh; // Reassign the original mesh
        }

        MeshCollider meshCollider = GetComponent<MeshCollider>();
        if (meshCollider != null)
        {
            meshCollider.sharedMesh = originalMeshColliderMesh; // Reset the mesh collider
        }

        FindObjectOfType<InputManager>()?.UnregisterMeshDeformationObject(this);
    }

    private void ResetObject()
    {
        transform.localPosition = initialPosition;
        transform.localRotation = initialRotation;
    }

    private void OnGrabbed(SelectEnterEventArgs arg)
    {
        initialPosition = transform.localPosition;
        isGrabbed = true;

        ActivateGizmos(false);
    }

    private void ActivateGizmos(bool activate)
    {
        if (activate)
        {
            vertexGroups = GroupVerticesByPosition(mesh.vertices);
            handles = new GameObject[vertexGroups.Count];
            int handleIndex = 0;

            foreach (var group in vertexGroups)
            {
                handles[handleIndex] = Instantiate(vertexPrefab, transform.TransformPoint(group.Key), Quaternion.identity, transform);

                VertexHandler handleScript = handles[handleIndex].GetComponent<VertexHandler>();
                if (handleScript != null)
                {
                    handleScript.vertexGroup = group.Value;
                    handleScript.meshDeformer = this;
                }
                handleIndex++;
            }
        }
        else
        {
            if (handles.Length > 0)
            {
                foreach (GameObject go in handles)
                {
                    Destroy(go);
                }
            }
        }
    }

    private void OnReleased(SelectExitEventArgs arg)
    {
        isGrabbed = false;
    }

    public void StartDeformation()
    {
        if (!IsDeforming)
        {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            originalMeshClone = CloneMesh(meshFilter.mesh); // Clone the mesh at the start of deformation

            // Update the `mesh` variable to reference the newly cloned mesh
            mesh = meshFilter.mesh;

            IsDeforming = true;
        }
    }

    public void EndDeformation()
    {
        if (IsDeforming)
        {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            deformedMeshClone = CloneMesh(meshFilter.mesh); // Clone the mesh at the end of deformation

            // Update the `mesh` variable to reference the newly deformed mesh
            mesh = deformedMeshClone;
            meshFilter.mesh = mesh;

            ICommand deformMeshCommand = new DeformMeshCommand(gameObject, originalMeshClone, deformedMeshClone);
            CommandManager.Instance.ExecuteCommand(deformMeshCommand);
            IsDeforming = false;

            ResetGizmo();
        }
    }

    public void ResetGizmo()
    {
        if (handles.Length > 0)
        {
            foreach (GameObject go in handles)
            {
                Destroy(go);
            }

            ActivateGizmos(true);
        }
    }

    public void ResetDeformationState()
    {
        IsDeforming = false;
    }

    private Mesh CloneMesh(Mesh mesh)
    {
        return Instantiate(mesh);
    }
}