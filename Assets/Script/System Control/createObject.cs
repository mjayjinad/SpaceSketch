using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using SpaceSketch.Scripts.System_Control;

namespace SpaceSketch.Scripts.Selection_and_Manipulation
{
    public class CreateObject : MonoBehaviour
    {
        [Header("Input Actions")]
        public InputActionReference createObjectAction; // Action to create the object

        [Header("XR Controller Tranform")]
        [SerializeField] private Transform rightController;
        [SerializeField] private Transform leftController;

        [Header("Shape Logic")]
        [SerializeField] private ShapeLogic shapeLogic; // Reference to ShapeLogic

        [SerializeField] private SphereSelectInput sphereSelectInput;
        [SerializeField] private GameObject sphereSelectObj;

        [Header("Object Parameters")]
        private GameObject instantiatedObject; // Reference to the instantiated object
        public Material opaque;

        private CommandManager commandManager;
        private GameObject midpointFollower;

        private void Awake()
        {
            commandManager = CommandManager.Instance;
            // Initialize the midpoint follower
            midpointFollower = new GameObject("MidpointFollower");
        }

        private void OnEnable()
        {
            createObjectAction.action.started += HandleCreateObject;
            createObjectAction.action.canceled += HandleCreateObjectStopScaling;
            createObjectAction.action.Enable();
        }

        private void OnDisable()
        {
            createObjectAction.action.started -= HandleCreateObject;
            createObjectAction.action.canceled -= HandleCreateObjectStopScaling;
            createObjectAction.action.Disable();
        }

        private void Update()
        {
            if (instantiatedObject != null)
            {
                // Continuously update the position of the midpoint follower
                Vector3 midpoint = (rightController.position + leftController.position) * 0.5f;
                midpointFollower.transform.position = midpoint;
                midpointFollower.transform.rotation = Quaternion.LookRotation(rightController.position - leftController.position);
            }
        }

        private void HandleCreateObject(InputAction.CallbackContext context)
        {
            if (context.ReadValue<float>() > 0.5f && instantiatedObject == null)
            {
                // Disable sphere selection while creating objects
                sphereSelectInput.enabled = false;
                sphereSelectObj.SetActive(false);

                // Calculate the midpoint between the two controller positions
                Vector3 midpoint = (rightController.position + leftController.position) * 0.5f;

                // Update the midpoint follower's position and rotation
                midpointFollower.transform.position = midpoint;
                midpointFollower.transform.rotation = Quaternion.LookRotation(rightController.position - leftController.position);

                // Use ShapeLogic to determine which prefab to instantiate
                GameObject shapePrefab = shapeLogic.GetPrefabFromShape(shapeLogic.selectedShape);

                // Create the command for creating an object and execute it
                ICommand createCommand = new CreateObjectCommand(shapePrefab, midpointFollower.transform);
                commandManager.ExecuteCommand(createCommand);

                // Get a reference to the instantiated object from the command
                instantiatedObject = ((CreateObjectCommand)createCommand).CreatedObject;

                // Enable scaling of the object
                instantiatedObject.GetComponent<SelectionState>().isSelected = true;
                instantiatedObject.GetComponent<ScaleObject>().StartScaling();
            }
        }

        private void HandleCreateObjectStopScaling(InputAction.CallbackContext context)
        {
            if (instantiatedObject != null)
            {
                sphereSelectInput.enabled = true;
                sphereSelectObj.SetActive(true);

                // Detach the instantiated object from the midpoint follower so it stays at its current position
                instantiatedObject.transform.SetParent(null);

                // Create a new instance of the opaque material and set it to the instantiated object
                if (instantiatedObject.GetComponent<MeshRenderer>() != null)
                {
                    Material newOpaqueMaterial = Instantiate(opaque);
                    instantiatedObject.GetComponent<MeshRenderer>().material = newOpaqueMaterial;
                }

                // Disable scaling of the object.
                instantiatedObject.GetComponent<SelectionState>().isSelected = false;
                instantiatedObject.GetComponent<ScaleObject>().StopScaling();
                instantiatedObject = null;
            }
        }
    }
}
