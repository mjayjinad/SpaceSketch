using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabMoveFeedback : MonoBehaviour
{
    [System.Serializable]
    public class ScaleEvent : UnityEvent { }

    public Transform xrOriginTransform;
    public TextMeshProUGUI uiText;
    public XRBaseController leftController;
    public XRBaseController rightController;
    public Canvas worldSpaceCanvas;
    public ScaleEvent OnScaleBegin;
    public ScaleEvent OnScaleEnd;
    public int textureSize;

    private float maxScale = 10; // Assuming 10 is the largest scale
    private float minScale = 1f;   // Assuming 1 is the smallest scale
    private float previousPercentage = 0f;
    private bool isScaling = false;
    private float downwardOffset = 0.05f;
    public float offset;
    private GrabMove grabMoveProvider;
    private LineRenderer lineRenderer;


    private void Start()
    {
        grabMoveProvider = GetComponent<GrabMove>();
        InitializeLineRenderer();
    }

    private void Update()
    {
        if (xrOriginTransform != null && uiText != null)
        {
            float currentScale = xrOriginTransform.localScale.x;

            // Calculate percentage
            float scalePercentage = Mathf.Lerp(100f, 1f, (currentScale - minScale) / (maxScale - minScale));

            // Update UI text to display this percentage
            uiText.text = $"{scalePercentage:0}%";

            // Determine if we've crossed a 25% threshold and give haptic feedback
            CheckForHapticFeedback(scalePercentage);

            // Calculate the midpoint between the two controllers
            Vector3 midpoint = (leftController.transform.position + rightController.transform.position) / 2;

            // Apply the downward offset to the midpoint
            worldSpaceCanvas.transform.position = midpoint - new Vector3(0, downwardOffset, 0);


            if (Camera.main != null)
            {
                Transform cameraTransform = Camera.main.transform;

                // Point towards the camera
                Vector3 directionToCamera = worldSpaceCanvas.transform.position - cameraTransform.position;

                // Make the UI element face the camera, but invert the Z direction
                worldSpaceCanvas.transform.rotation = Quaternion.LookRotation(-directionToCamera, cameraTransform.up);
            }

            EnableActivateUI();
        }

        if (leftController != null && rightController != null)
        {
            // Define the offset vectors
            Vector3 leftOffset = new Vector3(offset, 0, 0); // Adjust this as needed
            Vector3 rightOffset = new Vector3(-offset, 0, 0); // Adjust this as needed

            // Apply offsets and set the start and end points of the line
            lineRenderer.SetPosition(0, rightController.transform.position + rightOffset);
            lineRenderer.SetPosition(1, leftController.transform.position + leftOffset);
        }
    }

    private void CheckForHapticFeedback(float currentPercentage)
    {
        // Define the thresholds
        float[] thresholds = new float[] { 25f, 50f, 75f, 100f };

        foreach (float threshold in thresholds)
        {
            // Check if we've crossed a threshold since the last update
            if (CrossedThreshold(previousPercentage, currentPercentage, threshold))
            {
                // Trigger haptic feedback on both controllers
                if (leftController != null)
                {
                    leftController.SendHapticImpulse(0.5f, 0.1f);
                }
                if (rightController != null)
                {
                    rightController.SendHapticImpulse(0.5f, 0.1f);
                }

                // No need to check other thresholds
                break;
            }
        }

        // Update the previousPercentage for the next frame
        previousPercentage = currentPercentage;
    }

    private bool CrossedThreshold(float previous, float current, float threshold)
    {
        return (previous < threshold && current >= threshold) || (previous > threshold && current <= threshold);
    }

    private void EnableActivateUI()
    {
        // Check if scaling is enabled
        if (grabMoveProvider.isTwoHand)
        {
            if (!isScaling)
            {
                isScaling = true;
                OnScaleBegin?.Invoke();

                // Enable the LineRenderer when scaling starts
                lineRenderer.enabled = true;
            }
        }
        else
        {
            if (isScaling)
            {
                isScaling = false;
                OnScaleEnd?.Invoke();

                // Disable the LineRenderer when scaling ends
                lineRenderer.enabled = false;
            }
        }
    }

    private void InitializeLineRenderer()
    {
        GameObject lineRendererObject = new GameObject("ScalingLineRenderer");
        if (xrOriginTransform != null)
        {
            lineRendererObject.transform.SetParent(xrOriginTransform, false);
        }

        lineRenderer = lineRendererObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Unlit/Transparent"));

        // Create a texture with a dotted pattern
        Texture2D dottedTexture = new Texture2D(textureSize, 1);
        dottedTexture.filterMode = FilterMode.Point;
        Color dotColor = Color.white;
        Color backgroundColor = new Color(0, 0, 0, 0); // Transparent background

        for (int i = 0; i < textureSize; i++)
        {
            // Alternate between opaque and transparent pixels
            if (i % 2 == 0) // Adjust dot frequency here
            {
                dottedTexture.SetPixel(i, 0, dotColor);
            }
            else
            {
                dottedTexture.SetPixel(i, 0, backgroundColor);
            }
        }
        dottedTexture.Apply();

        lineRenderer.material.mainTexture = dottedTexture;
        lineRenderer.material.color = Color.white;

        // Adjust texture tiling for the dotted effect
        float tilingFactor = 0.3f; // Increase tiling factor to make dots appear smaller
        lineRenderer.material.mainTextureScale = new Vector2(tilingFactor, 1);

        lineRenderer.widthMultiplier = 0.05f; // Smaller width for the line
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.01f; // Adjust to control the size of the dots relative to the line width
        lineRenderer.endWidth = 0.01f;   // Adjust to control the size of the dots relative to the line width
        lineRenderer.enabled = false; // Make sure the LineRenderer is enabled
    }
}