using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace xrc_assignments_project_g05.Scripts.System_Control
{
    public class ShapeLogic : MonoBehaviour
    {
        public enum ShapeType
        {
            Cube,
            Sphere,
            Capsule,
            Cylinder
        }

        [SerializeField] public ShapeType selectedShape = ShapeType.Cube;

        // Prefabs for the shapes
        [SerializeField] private GameObject cubePrefab;
        [SerializeField] private GameObject spherePrefab;
        [SerializeField] private GameObject capsulePrefab;
        [SerializeField] private GameObject cylinderPrefab;

        // Reference to the feedback component
        private ShapeFeedback shapeFeedback;

        void Start()
        {
            shapeFeedback = GetComponent<ShapeFeedback>();
        }

        public void CycleShape(bool moveUpward)
        {
            int totalShapes = System.Enum.GetValues(typeof(ShapeType)).Length;

            if (moveUpward)
            {
                if ((int)selectedShape == 0)
                    selectedShape = (ShapeType)(totalShapes - 1);
                else
                    selectedShape = (ShapeType)(((int)selectedShape - 1) % totalShapes);
            }
            else
            {
                selectedShape = (ShapeType)(((int)selectedShape + 1) % totalShapes);
            }

            if (shapeFeedback != null)
            {
                shapeFeedback.ProvideFeedback(selectedShape);
            }
        }

        public GameObject GetPrefabFromShape(ShapeType shape)
        {
            switch (shape)
            {
                case ShapeType.Cube: return cubePrefab;
                case ShapeType.Sphere: return spherePrefab;
                case ShapeType.Capsule: return capsulePrefab;
                case ShapeType.Cylinder: return cylinderPrefab;
                default: return null;
            }
        }
    }
}