using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace SpaceSketch.Scripts.Selection_and_Manipulation
{
    public class ScaleObject : MonoBehaviour
    {
        [Header("Input Actions")]
        public InputActionReference scaleObjectAction;  // Action to scale the object

        [Header("XR Controller Transform")]
        [SerializeField] private Transform rightController;
        [SerializeField] private Transform axisController;
        
        private Vector3 initialDistance; // Initial distance between controllers when object was instantiated
        private Vector3 initialScale; // Initial scale of the instantiated object
        private SelectionState selectionState;
        public bool isScaling = false;


        private void Awake()
        {
            rightController = GameObject.FindGameObjectWithTag("RightController").transform;
            axisController = GameObject.FindGameObjectWithTag("LeftController").transform;
        }

        private void Start()
        {
            selectionState = GetComponent<SelectionState>();
        }

        private void Update()
        {
            if (isScaling && selectionState.isSelected)
            {
                Vector3 currentDistance = rightController.position - axisController.position;
                float scaleFactor = currentDistance.magnitude / initialDistance.magnitude;
                gameObject.transform.localScale = initialScale * scaleFactor;
            }
        }

        public void StartScaling()
        {
            initialDistance = rightController.position - axisController.position;
            initialScale = gameObject.transform.localScale;
            isScaling = true;
        }

        public void StopScaling()
        {
            isScaling = false;
            selectionState.isSelected = false;
        }
    }
}