using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GizmoController : MonoBehaviour
{
    public ObjectScaler currentScaler; // The current ObjectScaler that is controlling this gizmo
    private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void Start()
    {
        grabInteractable.selectExited.AddListener(Released);
    }

    // Call this method to assign the object scaler to this gizmo
    public void AssignObjectScaler(ObjectScaler scaler)
    {
        currentScaler = scaler;
    }

    // Call this method to clear the object scaler when it's no longer controlling this gizmo
    public void ClearObjectScaler()
    {
        currentScaler = null;
    }

    private void Released(SelectExitEventArgs arg)
    {
        // Set the mesh collider to concave for realistic collision detection.
        gameObject.GetComponentInParent<MeshCollider>().convex = false;

        // Disable the meshDeformer because scaling by axis already took place.
        gameObject.GetComponentInParent<MeshDeformer>().enabled = false;
    }

    void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectExited.RemoveListener(Released);
        }
    }
}
