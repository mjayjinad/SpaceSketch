using UnityEngine;

public class CylinderMovement : MonoBehaviour
{
    public float minDistance = 0.1f;
    public float maxDistance = 0.5f;
    public float minScale = 0.5f;
    public float maxScale = 1.0f;
    public float scaleSpeed = 1.0f;
    public Transform controllerTransform;
    public ColorSelectorInput colorSelectorInput;

    private Vector3 initialScale;
    private Vector3 targetScale;
    private bool isHovering = false;

    void Start()
    {
        initialScale = transform.localScale;
        targetScale = initialScale;
    }

    private void OnDisable()
    {
        isHovering = false;
    }

    void Update()
    {
        if (isHovering)
        {
            // Calculate the current distance between the object and the controller
            float currentDistance = Vector3.Distance(transform.position, controllerTransform.position);

            // Calculate the normalized distance for scaling
            float normalizedDistance = Mathf.InverseLerp(minDistance, maxDistance, currentDistance);

            // Interpolate the target scale based on the distance
            float targetYScale = Mathf.Lerp(minScale, maxScale, normalizedDistance);
            targetScale = new Vector3(initialScale.x, targetYScale, initialScale.z);

            // Adjust the object's scale smoothly over time
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scaleSpeed * Time.deltaTime);

            // Adjust the brightness based on the object's scale
            float brightness = Mathf.InverseLerp(minScale, maxScale, targetYScale);
            colorSelectorInput.AdjustBrightness(brightness);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ColorInteractor"))
        {
            isHovering = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ColorInteractor"))
        {
            isHovering = false;
            // Optionally reset scale when interaction ends
            targetScale = initialScale;
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scaleSpeed * Time.deltaTime);
        }
    }
}
