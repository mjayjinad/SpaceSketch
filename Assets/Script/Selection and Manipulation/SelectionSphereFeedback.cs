using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SelectionSphereFeedback : MonoBehaviour
{
    [Tooltip("The scale factor applied to the user input when determining the radius value.")]
    public float inputSensitivity = 0.1f;

    [Tooltip("Minimum and maximum scale for the sphere.")]
    public Vector3 minScale = Vector3.one * 0.05f;
    public Vector3 maxScale = Vector3.one * 5.0f;

    [Header("Material for Sphere")]
    public Material onHoverEnterMaterial;
    public Material onHoverExitMaterial;

    [Header("Material for Interactable")]
    public Color hoverColor;


    // Dictionary to store the default color of each interactable object
    private Dictionary<GameObject, Color> objectOriginalColors = new Dictionary<GameObject, Color>();
    private Collider[] hitColliders;

    private void Start()
    {
        gameObject.GetComponent<MeshRenderer>().material = onHoverExitMaterial;
    }

    public void AdjustSphereRadius(float input)
    {
        float radiusChange = input * inputSensitivity * Time.deltaTime;
        Vector3 newScale = transform.localScale + Vector3.one * radiusChange;
        newScale = Vector3.Max(minScale, Vector3.Min(newScale, maxScale));
        transform.localScale = newScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SelectableObject"))
        {
            gameObject.GetComponent<MeshRenderer>().material = onHoverEnterMaterial;
            //other.GetComponent<XRGrabInteractable>().enabled = false;

            // Store the original color in the dictionary
            MeshRenderer meshRenderer = other.gameObject.GetComponent<MeshRenderer>();
            if (meshRenderer != null && !objectOriginalColors.ContainsKey(other.gameObject))
            {
                objectOriginalColors[other.gameObject] = meshRenderer.material.color;
            }

            meshRenderer.material.color = hoverColor;

            SelectionState selectionState = other.GetComponent<SelectionState>();
            selectionState.isSelected = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("SelectableObject"))
        {
            gameObject.GetComponent<MeshRenderer>().material = onHoverEnterMaterial;
            //other.GetComponent<XRGrabInteractable>().enabled = false;

            // Store the original color in the dictionary
            MeshRenderer meshRenderer = other.gameObject.GetComponent<MeshRenderer>();
            if (meshRenderer != null && !objectOriginalColors.ContainsKey(other.gameObject))
            {
                objectOriginalColors[other.gameObject] = meshRenderer.material.color;
            }

            meshRenderer.material.color = hoverColor;

            SelectionState selectionState = other.GetComponent<SelectionState>();
            selectionState.isSelected = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SelectableObject"))
        {
            gameObject.GetComponent<MeshRenderer>().material = onHoverExitMaterial;
            //other.GetComponent<XRGrabInteractable>().enabled = true;

            // Retrieve and apply the original color from the dictionary
            MeshRenderer meshRenderer = other.gameObject.GetComponent<MeshRenderer>();
            if (meshRenderer != null && objectOriginalColors.ContainsKey(other.gameObject))
            {
                meshRenderer.material.color = objectOriginalColors[other.gameObject];
                objectOriginalColors.Remove(other.gameObject);
            }

            SelectionState selectionState = other.GetComponent<SelectionState>();
            selectionState.isSelected = false;
        }
    }

    public void IsTouchingSelectableObject()
    {
        float sphereRadius = transform.localScale.x / 2;
        hitColliders = Physics.OverlapSphere(transform.position, sphereRadius);

        foreach (var collider in hitColliders)
        {
            if (collider.CompareTag("SelectableObject"))
            {
                collider.GetComponent<XRGrabInteractable>().enabled = true;

                // Retrieve and apply the original color from the dictionary before deactivating this gameobject
                MeshRenderer meshRenderer = collider.gameObject.GetComponent<MeshRenderer>();
                if (meshRenderer != null && objectOriginalColors.ContainsKey(collider.gameObject))
                {
                    meshRenderer.material.color = objectOriginalColors[collider.gameObject];
                    objectOriginalColors.Remove(collider.gameObject);
                }

                gameObject.GetComponent<MeshRenderer>().material = onHoverExitMaterial;

                // Register the object with the SelectionManager
                SelectionManager.Instance.RegisterSelectedObject(collider.gameObject);
            }
        }
    }
}
