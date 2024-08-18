using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using SpaceSketch.Scripts.Selection_and_Manipulation;

public class ColorSelectorInput : MonoBehaviour
{
    [Header("Color Properties")]
    public Image colorWheel;
    public GameObject colorMenu, colorRay, colorInteractor;
    public Color selectedColor;
    public LayerMask uiLayerMask;

    [Header("Material Types")]
    public Material reflectiveMaterial;
    public Material basicMaterial;
    public Material clayMaterial;
    public Material flatMaterial;

    [Header("")]
    public CreateObject createObject;
    public Transform raycastOrigin; // Assign VR controller's transform here
    public Slider slider;

    // [HideInInspector]
    public List<MeshRenderer> currentRenderers = new List<MeshRenderer>();
    private Dictionary<MeshRenderer, Color> originalColors = new Dictionary<MeshRenderer, Color>();
    private Texture2D colorWheelTexture;
    private bool isHovering = false;

    void Start()
    {
        // Convert the color wheel image to a Texture2D
        colorWheelTexture = colorWheel.sprite.texture;
    }

    void Update()
    {
        //SetTransparency(slider.value);

        reflectiveMaterial.color = selectedColor;
        basicMaterial.color = selectedColor;
        clayMaterial.color = selectedColor;
        flatMaterial.color = selectedColor;

        if (isHovering)
        {
            UpdateColorSelection();
        }
    }

    public void StartHovering()
    {
        isHovering = true;
        foreach (MeshRenderer renderer in currentRenderers)
        {
            StoreOriginalColor(renderer, renderer.material.color);
        }
    }

    public void StopHovering()
    {
        isHovering = false;

        foreach (MeshRenderer renderer in currentRenderers)
        {
            string uniqueName = renderer.gameObject.name;
            Color originalColor = GetOriginalColor(renderer);
            ICommand changeColorCommand = new ChangeColorCommand(uniqueName, originalColor, selectedColor);
            CommandManager.Instance.ExecuteCommand(changeColorCommand);
        }
    }
    private void StoreOriginalColor(MeshRenderer renderer, Color originalColor)
    {
        if (!originalColors.ContainsKey(renderer))
        {
            originalColors.Add(renderer, originalColor);
        }
        else
        {
            // Update the color if the renderer is already in the dictionary
            originalColors[renderer] = originalColor;
        }
    }

    private Color GetOriginalColor(MeshRenderer renderer)
    {
        if (originalColors.TryGetValue(renderer, out Color originalColor))
        {
            return originalColor;
        }

        // Return a default color if the original color is not found
        // This scenario should ideally never happen
        return Color.white;
    }

    private void UpdateColorSelection()
    {
        Ray ray = new Ray(raycastOrigin.position, raycastOrigin.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, uiLayerMask))
        {
            if (hit.collider.gameObject == colorWheel.gameObject)
            {
                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(colorWheel.rectTransform, hit.point, null, out localPoint);

                // Convert to texture coordinates
                Vector2 normalizedPoint = Rect.PointToNormalized(colorWheel.rectTransform.rect, localPoint);
                selectedColor = GetColorFromTexture(normalizedPoint);

                foreach (MeshRenderer renderer in currentRenderers)
                {
                    renderer.material.color = selectedColor;
                }
            }
        }
    }

    private Color GetColorFromTexture(Vector2 coords)
    {
        int x = Mathf.FloorToInt(coords.x * colorWheelTexture.width);
        int y = Mathf.FloorToInt(coords.y * colorWheelTexture.height);
        return colorWheelTexture.GetPixel(x, y);
    }

    public void ApplyMaterial(string materialType)
    {
        Material selectedMaterial = null;

        // Choose the base material based on the materialType string
        switch (materialType)
        {
            case "Reflective":
                selectedMaterial = reflectiveMaterial;
                break;
            case "Basic":
                selectedMaterial = basicMaterial;
                break;
            case "Clay":
                selectedMaterial = clayMaterial;
                break;
            case "Flat":
                selectedMaterial = flatMaterial;
                break;
        }

        if (selectedMaterial != null)
        {
            // Create a new instance of the selected material for each renderer
            foreach (MeshRenderer renderer in currentRenderers)
            {
                Material materialInstance = Material.Instantiate(selectedMaterial);
                materialInstance.color = selectedColor;
                renderer.material = materialInstance;
            }
        }
    }

    public void RegisterMeshRenderer(MeshRenderer obj)
    {
        if (!currentRenderers.Contains(obj))
        {
            currentRenderers.Add(obj);
        }
    }

    public void AdjustBrightness(float brightnessFactor)
    {
        float H, S, V;
        Color.RGBToHSV(selectedColor, out H, out S, out V); // Convert to HSV
        V = brightnessFactor; // Adjust the V (Value) for brightness
        selectedColor = Color.HSVToRGB(H, S, V); // Convert back to RGB

        foreach (MeshRenderer renderer in currentRenderers)
        {
            renderer.material.color = selectedColor;
        }

        // Adjust the brightness of the color wheel image
        Color wheelColor = Color.HSVToRGB(0, 0, brightnessFactor); // Pure white to black based on brightnessFactor
        colorWheel.color = wheelColor;
    }
}