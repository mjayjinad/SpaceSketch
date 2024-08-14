using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VertexHandler : MonoBehaviour
{
    public MeshDeformer meshDeformer;

    private XRGrabInteractable grabInteractable;
    private Renderer objectRenderer;
    private Color originalColor;
    public Color grabbedColor = Color.yellow; // Color when grabbed
    public List<int> vertexGroup; // Indices of vertices in the group

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        originalColor = objectRenderer.material.color;

        grabInteractable = GetComponent<XRGrabInteractable>();

        // Subscribe to the grab and release events
        grabInteractable.selectEntered.AddListener(Grabbed);
        grabInteractable.selectExited.AddListener(Released);
    }

    private void Update()
    {
        if (meshDeformer != null)
        {
            meshDeformer.UpdateVertexGroupPosition(vertexGroup, transform.position);
        }
    }

    private void Grabbed(SelectEnterEventArgs arg)
    {
        // Change the appearance
        objectRenderer.material.color = grabbedColor;

        // Check the interactor type and try to get XRBaseController
        var controller = arg.interactor.GetComponentInParent<XRBaseController>();
        if (controller != null)
        {
            controller.SendHapticImpulse(0.5f, 0.25f);
        }

        if (!meshDeformer.IsDeforming)
        {
            meshDeformer.StartDeformation();
        }
    }

    private void Released(SelectExitEventArgs arg)
    {
        // reset the appearance
        objectRenderer.material.color = originalColor;

        // When handle is released, update the mesh collider of the object
        if (meshDeformer != null)
        {
            meshDeformer.UpdateMeshCollider();
        }

        // Disable the ObjectScaler because mesh deformation already occured.
        gameObject.GetComponentInParent<ObjectScaler>().enabled = false;
        // Set the mesh collider to concave for realistic collision detection.
        gameObject.GetComponentInParent<MeshCollider>().convex = false;

        // End deformation process
        meshDeformer.EndDeformation();

        // Reset the deformation state for new deformation
        meshDeformer.ResetDeformationState();
    }

    void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(Grabbed);
            grabInteractable.selectExited.RemoveListener(Released);
        }
    }
}