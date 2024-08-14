using UnityEngine;

namespace xrc_assignments_project_g05.Scripts.System_Control
{
    public class ShapeFeedback : MonoBehaviour
    {
        [SerializeField] private GameObject menuCanvas; // Reference to the menu canvas
        [SerializeField] private MeshRenderer cubeIcon;
        [SerializeField] private MeshRenderer sphereIcon;
        [SerializeField] private MeshRenderer capsuleIcon;
        [SerializeField] private MeshRenderer cylinderIcon;

        [SerializeField] private Color defaultColor = Color.white;
        [SerializeField] private Color tintColor = Color.yellow;

        private void Start()
        {
            cubeIcon.material.color = tintColor;
        }

        // This method is responsible for displaying the menu with the appropriate shape highlighted.
        public void ProvideFeedback(ShapeLogic.ShapeType selectedShape)
        {
            // Ensure the menu is visible
            if (menuCanvas != null && menuCanvas.activeInHierarchy)
            {
                // Highlight the selected shape
                HighlightShape(selectedShape);
            }
        }

        // This method highlights the currently selected shape in the menu.
        private void HighlightShape(ShapeLogic.ShapeType shape)
        {
            // Reset all highlights
            cubeIcon.material.color = defaultColor;
            sphereIcon.material.color = defaultColor;
            capsuleIcon.material.color = defaultColor;
            cylinderIcon.material.color = defaultColor;

            // Highlight the selected shape
            switch (shape)
            {
                case ShapeLogic.ShapeType.Cube:
                    cubeIcon.material.color = tintColor;
                    break;
                case ShapeLogic.ShapeType.Sphere:
                    sphereIcon.material.color = tintColor;
                    break;
                case ShapeLogic.ShapeType.Capsule:
                    capsuleIcon.material.color = tintColor;
                    break;
                case ShapeLogic.ShapeType.Cylinder:
                    cylinderIcon.material.color = tintColor;
                    break;
            }
        }
    }
}
