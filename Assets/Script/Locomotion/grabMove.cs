using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabMove : MonoBehaviour
{
    [Header("Controller References")]
    public Transform leftController;
    public Transform rightController;
    public Transform xrOrigin;

    [Header("Movement Settings")]
    public bool enableTranslation = true;
    public bool enableRotation = true;
    public bool enableScaling = true;

    [Header("Scale Settings")]
    public float minimumScale = 0.1f;
    public float maximumScale = 10.0f;

    public bool isTwoHand = false;

    private Vector3 previousLeftControllerPosition;
    private Vector3 previousRightControllerPosition;
    private Vector3 previousMidpoint;
    private Vector3 previousDistanceVector;

    private void Update()
    {
        if (isTwoHand)
        {
            // Calculate current state
            Vector3 currentMidpoint = Vector3.Lerp(leftController.position, rightController.position, 0.5f);
            Vector3 currentDistanceVector = leftController.position - rightController.position;

            previousMidpoint = Vector3.Lerp(previousLeftControllerPosition, previousRightControllerPosition, 0.5f);
            previousDistanceVector = previousLeftControllerPosition - previousRightControllerPosition;

            // Calculate transformations
            Vector3 positionChange = currentMidpoint - previousMidpoint;
            float scaleFactor = Vector3.Distance(previousLeftControllerPosition, previousRightControllerPosition) / Vector3.Distance(leftController.position, rightController.position);
            Vector3 normal = Vector3.Cross(previousDistanceVector, currentDistanceVector);
            float angle = Vector3.SignedAngle(previousDistanceVector, currentDistanceVector, normal);

            // Apply transformations
            if (enableTranslation)
            {
                ApplyTranslation(positionChange);
            }
            if (enableRotation)
            {
                ApplyRotation(currentMidpoint, normal, -angle);
            }
            if (enableScaling)
            {
                ApplyScaling(currentMidpoint, scaleFactor);
            }

            // Update previous state
            previousLeftControllerPosition = leftController.position;
            previousRightControllerPosition = rightController.position;
        }
    }

    public void OnGrabStart()
    {
        isTwoHand = true;
        previousLeftControllerPosition = leftController.position;
        previousRightControllerPosition = rightController.position;
        previousDistanceVector = leftController.position - rightController.position;
    }

    public void OnGrabEnd()
    {
        isTwoHand = false;
    }

    private void ApplyTranslation(Vector3 translation)
    {
        xrOrigin.position -= translation;
    }

    private void ApplyRotation(Vector3 pivot, Vector3 rotationAxis, float angle)
    {
        xrOrigin.RotateAround(pivot, rotationAxis, angle);
    }

    private void ApplyScaling(Vector3 currentMidPoint, float rawScaleFactor)
    {
        // Increase the smoothing factor for faster scaling.
        float smoothingFactor = 50.0f;

        // Smooth the scale factor to avoid abrupt changes at small scales
        float smoothedScaleFactor = Mathf.Lerp(1.0f, rawScaleFactor, Time.deltaTime * smoothingFactor);

        // Calculate new scale
        Vector3 newScale = xrOrigin.localScale * smoothedScaleFactor;

        // Clamp the new scale to prevent it from going below minimumScale and maximumScale
        newScale.x = Mathf.Clamp(newScale.x, minimumScale, maximumScale);
        newScale.y = Mathf.Clamp(newScale.y, minimumScale, maximumScale);
        newScale.z = Mathf.Clamp(newScale.z, minimumScale, maximumScale);

        // Calculate scale change ratio for each axis
        float scaleXChangeRatio = newScale.x / xrOrigin.localScale.x;
        float scaleYChangeRatio = newScale.y / xrOrigin.localScale.y;
        float scaleZChangeRatio = newScale.z / xrOrigin.localScale.z;

        // Adjust position only if the scale is not at its minimum
        if (newScale != Vector3.one * minimumScale)
        {
            Vector3 positionAdjustment = (currentMidPoint - xrOrigin.position);
            positionAdjustment.x *= (1 - scaleXChangeRatio);
            positionAdjustment.y *= (1 - scaleYChangeRatio);
            positionAdjustment.z *= (1 - scaleZChangeRatio);

            xrOrigin.position += positionAdjustment;
        }

        // Apply the new scale
        xrOrigin.localScale = newScale;
    }
}
